using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Configuracion de Salud")]
    public int maxHealth = 3;
    public float currentHealth;
    
    [Header("Audio SFX")]
    public AudioClip hurtSound;
    private AudioSource _audioSource;
    
    //public UnityEvent<float, float> onHealthChange;
    //public UnityEvent onDeath;

    private void Start()
    {
        currentHealth = maxHealth;
        EventManager.Invoke(GlobalEvents.OnPlayerHealthChanged, currentHealth);
        _audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        Debug.Log("VIDA RESTANTE: " + currentHealth);
        if (hurtSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(hurtSound);
        }

        EventManager.Invoke(GlobalEvents.OnPlayerHealthChanged, (int)currentHealth);
        if (currentHealth == 0)
        { 
            EventManager.Invoke(GlobalEvents.OnPlayerDeath);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var hitbox = other.gameObject.GetComponent<Hitbox>();
        if (hitbox) 
        {
            TakeDamage(hitbox.damage);
            KnightController.Instance.GetHit(other.transform.position);
        }
    }
}