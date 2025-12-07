using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameScene = "Nivel_1";

    public void StartGame()
    {
        GameManager.Instance.ChangeState(new PlayGame(GameManager.Instance, gameScene));
    }
}
