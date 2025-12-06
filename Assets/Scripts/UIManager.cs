using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text scoreText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateScore(int points)
    {
        scoreText.text = "Puntos: " + points;
    }
}