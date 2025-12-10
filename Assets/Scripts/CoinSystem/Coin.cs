using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Configuración de Moneda")]
    [SerializeField] private string uniqueID;
    public int points = 1;
    public LayerMask playerLayer;
    
    [Header("Audio SFX")]
    public AudioClip coindSound;
    
    private void Awake()
    {
        // Validación de seguridad
        if (string.IsNullOrEmpty(uniqueID)) 
        {
            GenerateID();
        }
    }
    
    private void Start()
    {
        var manager = FindFirstObjectByType<PointManager>();
        
        if (manager != null)
        {
            if (manager.IsAlreadyCollected(uniqueID))
            {
                // Si ya estoy en la lista, me desactivo inmediatamente
                gameObject.SetActive(false);
            }
        }
    }
    
    private void Collect()
    {
        var manager = FindFirstObjectByType<PointManager>();
        if (manager != null)
        {
            manager.RegisterCollection(uniqueID);
        }
        EventManager.Invoke(GlobalEvents.OnAddCoins, points);
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer))>0)
        {
            Collect();
            PlayCoinSound();
            gameObject.SetActive(false);
        }
    }

    
    [ContextMenu("Generar Nuevo ID")]
    private void GenerateID()
    {
        uniqueID = System.Guid.NewGuid().ToString();
    }
    
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(uniqueID))
        {
            GenerateID();
        }
    }
    
    
    public void PlayCoinSound()
    {
        if (coindSound != null)
        {
            AudioSource.PlayClipAtPoint(coindSound, transform.position, .7f);
        }

    }
}