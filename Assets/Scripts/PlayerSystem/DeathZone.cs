using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public int damageAmount = 1;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Health playerHealth = other.GetComponentInParent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
        
        if (playerHealth.currentHealth > 0)
        {
            KnightController.Instance.RespawnPlayer();
        }
    }
}
