using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public float currentHealth;
    public UnityEvent<float, float> onHealthChange;
    public UnityEvent onDeath;

    private void Start()
    {
        currentHealth = maxHealth;
        onHealthChange?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        onHealthChange?.Invoke(currentHealth, maxHealth); // para UI
        Debug.Log($"Current health: {currentHealth}");
        if (currentHealth == 0) onDeath?.Invoke();
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
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





