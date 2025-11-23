using UnityEngine;

public class GameManager : MonoBehaviour, IGameMachine
{
    public IGame CurrentGame { get; set; }

    private void Start() => ChangeState(new MainMenuGame(this));

    public void ChangeState(IGame game)
    {
        CurrentGame?.Exit();
        CurrentGame = game;
        CurrentGame?.Enter();
    }

    private void Update() => CurrentGame?.Tick(Time.deltaTime);
}

public struct MainMenuGame : IGame
{
    public GameManager GameManager { get; set; }
    public MainMenuGame(GameManager gameManager)
    {
        GameManager = gameManager;
    }
    public void Enter()
    {
        Debug.Log("Enter Main Menu Game");
    }
    public void Tick(float deltaTime)
    {
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.ChangeState(new PlayGame());
        }
        */
    }
    
    public void Exit()
    {
        Debug.Log("Exit Main Menu Game");
    }
}

public struct PlayGame : IGame
{
    public GameManager GameManager { get; set; }
    
    public PlayGame(GameManager gameManager)
    {
        GameManager = gameManager;
    }
    public void Enter()
    {
        Debug.Log("Enter Play Game");
    }
    public void Tick(float deltaTime)
    {
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.ChangeState(new PauseMenuGame());
        }
        */
    }
    
    public void Exit()
    {
        Debug.Log("Exit Play Game");
    }
}

public struct PauseMenuGame : IGame
{
    public GameManager GameManager { get; set; }
    
    public PauseMenuGame(GameManager gameManager)
    {
        GameManager = gameManager;
    }
    
    public void Enter()
    {
        Debug.Log("Enter Pause Menu Game");
    }
    
    public void Tick(float deltaTime)
    {
        /*if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.ChangeState(new PlayGame());
        }
        */
    }
    
    public void Exit()
    {
        Debug.Log("Exit Pause Menu Game");
    }
}

public struct GameOverGame : IGame
{
    public GameManager GameManager { get; set; }
    
    public GameOverGame(GameManager gameManager)
    {
        GameManager = gameManager;
    }
    
    public void Enter()
    {
        Debug.Log("Enter Game Over Game");
    }
    
    public void Tick(float deltaTime)
    {
        /*if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.ChangeState(new MainMenuGame());
        }
        */
    }
    
    public void Exit()
    {
        Debug.Log("Exit Game Over Game");
    }
}


