using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public float currentHealth;
    public UnityEvent<float, float> onHealthChange;
    public UnityEvent onDeath;
    
    [Header("Audio SFX")]
    public AudioClip hurtSound;
    private AudioSource _audioSource;

    private void Start()
    {
        currentHealth = maxHealth;
        EventManager.Invoke(GlobalEvents.OnPlayerHealthChanged, currentHealth);
        _audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        if (hurtSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(hurtSound);
        }

        EventManager.Invoke(GlobalEvents.OnPlayerHealthChanged, (int)currentHealth);
        if (currentHealth == 0)
        { 
            onDeath?.Invoke(); 
            
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