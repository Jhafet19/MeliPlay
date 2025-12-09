using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameScene = "Nivel_2";
    [SerializeField] private string creditsScene = "Credits";

    [Header("Referencias UI")]
    [SerializeField] private Button loadButton;
    
    private void Start()
    {
        CheckSaveFile();
    }
    
    private void CheckSaveFile()
    {
        string path = Application.persistentDataPath + "/gameData.json";

        if (File.Exists(path))
        {
            loadButton.gameObject.SetActive(true);
        }
        else
        {
            loadButton.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        GameManager.Instance.isLoadingFromSave = false;
        GameManager.Instance.ChangeState(new PlayGame(GameManager.Instance, gameScene, false));
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene(creditsScene);
    }
    
    public void LoadSavedGame()
    {
        string path = Application.persistentDataPath + "/gameData.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameData tempDataType = JsonUtility.FromJson<GameData>(json);

            if (!string.IsNullOrEmpty(tempDataType.lastLevelPlayed))
            {
                GameManager.Instance.isLoadingFromSave = true;
                GameManager.Instance.ChangeState(new PlayGame(GameManager.Instance, tempDataType.lastLevelPlayed, false));
            }
        }
        else
        {
            Debug.Log("No hay partida guardada.");
        }
    }

 
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; 
        #endif
    }
    
}
