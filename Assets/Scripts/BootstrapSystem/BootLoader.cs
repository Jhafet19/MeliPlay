using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootLoader : MonoBehaviour
{
    private void Start()
    {
        LoaderManager.LoadLevel("Menu");
    }
}
