using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour, IGameMachine
{
    public static GameManager Instance; // Singleton

    public IGame CurrentGame { get; set; }

    [Header("Configuración de Música")]
    public AudioClip MenuMusic;      
    public AudioClip GameplayMusic;  
    public AudioClip GameOverMusic;  
    
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
        if (musicClip == null || _audioSource.clip == musicClip) return;

        _audioSource.clip = musicClip;
        _audioSource.Play();
    }
}

public class MainMenuGame : IGame
{
    private GameManager _gm;
    private const string FirstLevelName = "Nivel_1";
    
    public MainMenuGame(GameManager gameManager) => _gm = gameManager;

    public void Enter()
    {
        _gm.PlayMusic(_gm.MenuMusic);
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void Tick(float deltaTime)
    {
        // Presiona P para jugar
        if (Input.GetKeyDown(KeyCode.P)) 
            _gm.ChangeState(new PlayGame(_gm, FirstLevelName));
    }
    
    public void Exit() { }
}

public class PlayGame : IGame
{
    private GameManager _gm;
    private string _sceneToLoad;
    
    public PlayGame(GameManager gameManager, string sceneName)
    {
        _gm = gameManager;
        _sceneToLoad = sceneName;
    }

    public void Enter()
    {
        _gm.PlayMusic(_gm.GameplayMusic);
        if (!string.IsNullOrEmpty(_sceneToLoad) && SceneManager.GetActiveScene().name != _sceneToLoad)
        {
            SceneManager.LoadScene(_sceneToLoad);
        }
        EventManager.Subscribe(GlobalEvents.OnPlayerDeath, OnDeath);
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
    }
    
    private void OnDeath()
    {
        _gm.ChangeState(new GameOverGame(_gm, _sceneToLoad));
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
        // R -> Volver a jugar (resume)
        if (Input.GetKeyDown(KeyCode.R)) 
            _gm.ChangeState(new PlayGame(_gm, ""));
    }

    public void Exit()
    {
        EventManager.Invoke(GlobalEvents.OnGameResume);
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
    
    public void Tick(float deltaTime)
    {
        // M -> Volver al Menú
        if (Input.GetKeyDown(KeyCode.M)) 
            _gm.ChangeState(new MainMenuGame(_gm));
    }
    
    public void Exit() { }
}