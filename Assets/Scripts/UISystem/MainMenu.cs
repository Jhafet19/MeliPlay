using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameScene = "Nivel_1";
    [SerializeField] private string creditsScene = "Credits";

    public void StartGame()
    {
        GameManager.Instance.ChangeState(new PlayGame(GameManager.Instance, gameScene));
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene(creditsScene);
    }

    /*
    ¿PARA SALIR DEL JUEGO? -> checar <-
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#endif
    }
    */
}
