using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private RectTransform heartIconContainer;
    [SerializeField] private GameObject heartIconPrefab;
    private List<GameObject> heartIcons = new();
    [SerializeField] private int totalHearts = 3;
    private void Start()
    {
        for (int i = 0; i < totalHearts; i++)
        {
            var instance = Instantiate(heartIconPrefab, heartIconContainer);
            heartIcons.Add(instance);
            instance.SetActive(true);
        }
    }

    public void UpdateHealth(float health, float maxHealth)
    {
        int hearts = (int)health;
        for (var i = 0; i < heartIcons.Count; i++) heartIcons[i].SetActive(i < hearts);
    }
}
