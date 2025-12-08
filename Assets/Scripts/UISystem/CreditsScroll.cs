using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    [Header("Texto a mover")]
    public RectTransform creditsText;

    [Header("Velocidad")]
    public float scrollSpeed = 50f;

    [Header("Posiciones")]
    public float startY = -600f; // Debajo
    public float endY = 800f;    // Por arriba

    void Start()
    {
        // Texto en la posición inicial
        Vector2 pos = creditsText.anchoredPosition;
        creditsText.anchoredPosition = new Vector2(pos.x, startY);
    }

    void Update()
    {
        // Mover hacia arriba
        creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        // Si ya se fue muy arriba, regresa al inicio (loop)
        if (creditsText.anchoredPosition.y >= endY)
        {
            Vector2 pos = creditsText.anchoredPosition;
            creditsText.anchoredPosition = new Vector2(pos.x, startY);
        }
    }
}
