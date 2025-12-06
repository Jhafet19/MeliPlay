using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Meliplay
    [SerializeField] private string gameScene;

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
    }
}
