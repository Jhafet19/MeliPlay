using UnityEngine;

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
        // --- PATRÓN SINGLETON (Para que no se destruya entre escenas) ---
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ¡Esto hace la magia!
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

// --- ESTADOS (Clases, no Structs) ---

public class MainMenuGame : IGame
{
    private GameManager _gm;
    public MainMenuGame(GameManager gameManager) => _gm = gameManager;

    public void Enter()
    {
        Debug.Log("Estado: Menú Principal");
        _gm.PlayMusic(_gm.MenuMusic); // Pone música de menú
    }

    public void Tick(float deltaTime)
    {
        // Presiona P para jugar
        if (Input.GetKeyDown(KeyCode.P)) 
            _gm.ChangeState(new PlayGame(_gm));
    }
    
    public void Exit() { }
}

public class PlayGame : IGame
{
    private GameManager _gm;
    public PlayGame(GameManager gameManager) => _gm = gameManager;

    public void Enter()
    {
        Debug.Log("Estado: Jugando");
        _gm.PlayMusic(_gm.GameplayMusic); // Pone música de juego
    }

    public void Tick(float deltaTime)
    {
        // E -> Game Over
        if (Input.GetKeyDown(KeyCode.E)) 
            _gm.ChangeState(new GameOverGame(_gm));
            
        // ESC -> Pausa
        if (Input.GetKeyDown(KeyCode.Escape)) 
            _gm.ChangeState(new PauseMenuGame(_gm));
    }
    
    public void Exit() { }
}

public class PauseMenuGame : IGame
{
    private GameManager _gm;
    public PauseMenuGame(GameManager gameManager) => _gm = gameManager;

    public void Enter()
    {
        Debug.Log("Estado: Pausa");
        // No cambiamos música aquí para dejar la de fondo
    }
    
    public void Tick(float deltaTime)
    {
        // R -> Volver a jugar (resume)
        if (Input.GetKeyDown(KeyCode.R)) 
            _gm.ChangeState(new PlayGame(_gm));
    }
    
    public void Exit() { }
}

public class GameOverGame : IGame
{
    private GameManager _gm;
    public GameOverGame(GameManager gameManager) => _gm = gameManager;

    public void Enter()
    {
        Debug.Log("Estado: Game Over");
        _gm.PlayMusic(_gm.GameOverMusic); // Música triste
    }
    
    public void Tick(float deltaTime)
    {
        // M -> Volver al Menú
        if (Input.GetKeyDown(KeyCode.M)) 
            _gm.ChangeState(new MainMenuGame(_gm));
    }
    
    public void Exit() { }
}