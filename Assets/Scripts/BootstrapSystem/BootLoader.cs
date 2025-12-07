using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene("Menu");
    }
}
