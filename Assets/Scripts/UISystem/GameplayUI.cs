using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [Header("Configuración de Puntos")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text fruitText;

    [Header("Configuración de Corazones")]
    [SerializeField] private RectTransform heartIconContainer;
    [SerializeField] private GameObject heartIconPrefab;
    [SerializeField] private int maxHearts = 3;
    
    [Header("Menus")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    
    private List<GameObject> _heartIcons = new();

    private void Awake()
    {
        InitializeHearts();
    }

    private void OnEnable()
    {
        EventManager.Subscribe<int>(GlobalEvents.OnScoreCoinChanged, UpdateScoreCoin);
        EventManager.Subscribe<int>(GlobalEvents.OnPlayerHealthChanged, UpdateHearts);
        EventManager.Subscribe<int>(GlobalEvents.OnScoreFruitChanged, UpdateScoreFruit);
        EventManager.Subscribe(GlobalEvents.OnGamePause, ShowPause);
        EventManager.Subscribe(GlobalEvents.OnGameResume, HidePause);
        EventManager.Subscribe(GlobalEvents.OnGameOver, ShowGameOver);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<int>(GlobalEvents.OnScoreCoinChanged, UpdateScoreCoin);
        EventManager.Unsubscribe<int>(GlobalEvents.OnPlayerHealthChanged, UpdateHearts);
        EventManager.Unsubscribe<int>(GlobalEvents.OnScoreFruitChanged, UpdateScoreFruit);
        EventManager.Unsubscribe(GlobalEvents.OnGamePause, ShowPause);
        EventManager.Unsubscribe(GlobalEvents.OnGameResume, HidePause);
        EventManager.Unsubscribe(GlobalEvents.OnGameOver, ShowGameOver);
    }
    
    private void InitializeHearts()
    {
        foreach (Transform child in heartIconContainer) Destroy(child.gameObject);
        _heartIcons.Clear();

        for (int i = 0; i < maxHearts; i++)
        {
            var instance = Instantiate(heartIconPrefab, heartIconContainer);
            _heartIcons.Add(instance);
            instance.SetActive(true);
        }
    }
    
    private void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < _heartIcons.Count; i++)
        {
            _heartIcons[i].SetActive(i < currentHealth);
        }
    }

    private void UpdateScoreCoin(int points)
    {
        coinText.text = "x " + points;
    }
    
    private void UpdateScoreFruit(int points)
    {
        fruitText.text = "x " + points;
    }
    
    
    private void ShowPause()
    {
        if(pausePanel != null) pausePanel.SetActive(true);
    }

    private void HidePause()
    {
        if(pausePanel != null) pausePanel.SetActive(false);
    }

    private void ShowGameOver()
    {
        if(gameOverPanel != null) gameOverPanel.SetActive(true);
    }
    
    
}
