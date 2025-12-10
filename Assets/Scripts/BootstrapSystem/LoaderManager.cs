using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoaderManager : MonoBehaviour
{
    
    public static string sceneToLoad;
    
    [Header("Referencias UI")]
    public Slider progressBar;
    public TMP_Text progressText;
    
    void Start()
    {
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            progressBar.value = progress;

            if (progressText != null)
                progressText.text = (progress * 100f).ToString("F0") + "%";

            // Esperamos al siguiente frame
            yield return new WaitForEndOfFrame();
        }
    }

    public static void LoadLevel(string name)
    {
        sceneToLoad = name;
        SceneManager.LoadScene("LoadingScene");
    }
}
