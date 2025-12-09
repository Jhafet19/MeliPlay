using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [Header("Configuración")]
    public int keyID = 1; // ID de la llave (1 para la puerta 1)

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si es el jugador (asegúrate de que tu Player tenga el Tag "Player")
        if (other.CompareTag("Player"))
        {
            var inventory = other.GetComponent<PlayerInventory>();
            
            if (inventory != null)
            {
                inventory.AddKey(keyID); // Da la llave
                Destroy(gameObject);     // Destruye el objeto del suelo
            }
        }
    }
}