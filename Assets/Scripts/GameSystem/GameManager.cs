using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct LevelData
{
    public string sceneName;
    public Vector2 startPosition;
}

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour, IGameMachine
{
    public static GameManager Instance; // Singleton

    public IGame CurrentGame { get; set; }

    [Header("Configuración de Música")]
    public AudioClip MenuMusic;      
    public AudioClip GameplayMusic;  
    public AudioClip GameOverMusic;
    
    [Header("Configuración de Niveles (Hardcoded)")]
    public List<LevelData> levelsConfig;
    
    [Header("Control de Guardado")]
    public bool isLoadingFromSave = false;
    
    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Si ya existe uno, destruimos el nuevo para no tener duplicados
            return;
        }
        // -------------------------------------------------------------

        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true; 
        _audioSource.playOnAwake = false;
    }

    private void Start()
    {
        // Solo iniciamos el estado si es la primera vez (no hay juego corriendo)
        if(CurrentGame == null)
            ChangeState(new MainMenuGame(this));
    }

    private void Update()
    {
        CurrentGame?.Tick(Time.deltaTime);
    }

    public void ChangeState(IGame game)
    {
        CurrentGame?.Exit();
        CurrentGame = game;
        CurrentGame?.Enter();
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip == null) 
        {
            Debug.LogError("ERROR: Se intentó reproducir música, pero el AudioClip es NULL. ¿Lo asignaste en el Inspector?");
            return;
        }

        // 2. Si ya está sonando esa misma canción, no hacemos nada
        if (_audioSource.clip == musicClip) 
        {
            Debug.Log("AVISO: Ya está sonando esta canción, no la reinicio.");
            return;
        }

        Debug.Log($"CAMBIANDO MUSICA A: {musicClip.name}");
        _audioSource.clip = musicClip;
        _audioSource.Play();
    }
    
    public Vector2 GetStartPosition(string sceneName)
    {
        var levelData = levelsConfig.FirstOrDefault(l => l.sceneName == sceneName);
        
        if (!string.IsNullOrEmpty(levelData.sceneName))
        {
            return levelData.startPosition;
        }
        return Vector2.zero; 
    }
    
    public void UI_ResumeGame()
    {
        if (CurrentGame is PauseMenuGame pauseState)
        {
            pauseState.Resume();
        }
    }
    
    public void UI_ReturnToMenu()
    {
        ChangeState(new MainMenuGame(this));
    }
    
    public void UI_RetryLevel()
    {
        if (CurrentGame is GameOverGame gameOverState)
        {
            gameOverState.Retry();
        }
    }
}

public class MainMenuGame : IGame
{
    private GameManager _gm;
    
    public MainMenuGame(GameManager gameManager) => _gm = gameManager;

    public void Enter()
    {
        Time.timeScale = 1f;
        _gm.PlayMusic(_gm.MenuMusic);
        
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            LoaderManager.LoadLevel("Menu");
        }
    }

    public void Tick(float deltaTime) { }
    
    public void Exit() { }
}

public class PlayGame : IGame
{
    private GameManager _gm;
    private string _sceneToLoad;
    private bool _isRetry;
    
    public PlayGame(GameManager gameManager, string sceneName, bool isRetry = false)
    {
        _gm = gameManager;
        _sceneToLoad = sceneName;
        _isRetry = isRetry;
    }

    public void Enter()
    {
        _gm.PlayMusic(_gm.GameplayMusic);
        Time.timeScale = 1f;
        
        EventManager.Subscribe(GlobalEvents.OnPlayerDeath, OnDeath);
        EventManager.Subscribe<string>(GlobalEvents.OnLevelComplete, OnVictory);

        if (!string.IsNullOrEmpty(_sceneToLoad) && SceneManager.GetActiveScene().name != _sceneToLoad)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            LoaderManager.LoadLevel(_sceneToLoad);
        }   
        else
        {
            if (_isRetry) 
            {
                SetPlayerPosition();
            }
        }
    }

    public void Tick(float deltaTime)
    {
        // ESC -> Pausa
        if (Input.GetKeyDown(KeyCode.Escape)) 
            _gm.ChangeState(new PauseMenuGame(_gm, _sceneToLoad));
    }

    public void Exit()
    {
        EventManager.Unsubscribe(GlobalEvents.OnPlayerDeath, OnDeath);
        EventManager.Unsubscribe<string>(GlobalEvents.OnLevelComplete, OnVictory);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnDeath()
    {
        _gm.ChangeState(new GameOverGame(_gm, _sceneToLoad));
    }
    
    public void OnVictory(string nextLevelName)
    {
        _gm.ChangeState(new PlayGame(_gm, nextLevelName, isRetry: false));
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == _sceneToLoad) 
        {
            SetPlayerPosition();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    
    private void SetPlayerPosition()
    {
        Vector2 startPos = _gm.GetStartPosition(_sceneToLoad);
        
        if (KnightController.Instance != null)
        {
            KnightController.Instance.transform.position = startPos;
            KnightController.Instance.UpdateCheckpoint(startPos);
            KnightController.Instance.RespawnPlayer(); 
        }
    }
}

public class PauseMenuGame : IGame
{
    private GameManager _gm;
    private string _sceneToLoad;
    
    public PauseMenuGame(GameManager gameManager, string sceneName)
    {
        _gm = gameManager;
        _sceneToLoad = sceneName;
    }

    public void Enter()
    {
        Time.timeScale = 0f;
        EventManager.Invoke(GlobalEvents.OnGamePause);
        
    }
    
    public void Tick(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
            Resume();
    }

    public void Exit()
    {
        EventManager.Invoke(GlobalEvents.OnGameResume);
    }
    
    public void Resume()
    {
        _gm.ChangeState(new PlayGame(_gm, _sceneToLoad, isRetry: false));
    }
    
}

public class GameOverGame : IGame
{
    private GameManager _gm;
    private string _sceneToLoad;
    
    public GameOverGame(GameManager gameManager, string sceneName)
    {
        _gm = gameManager;
        _sceneToLoad = sceneName;
    }

    public void Enter()
    {
        _gm.PlayMusic(_gm.GameOverMusic);
        EventManager.Invoke(GlobalEvents.OnGameOver);
    }
    
    public void Tick(float deltaTime) { }
    
    public void Exit() { }
    
    public void Retry()
    {
        SceneManager.LoadScene(_sceneToLoad); 
        _gm.ChangeState(new PlayGame(_gm, _sceneToLoad, isRetry: true));
    }
}