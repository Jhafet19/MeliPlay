using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [Header("Configuración")]
    public int keyID = 1; // ID de la llave (1 para la puerta 1)
    public LayerMask player;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entro");
        if ((player.value & (1 << other.gameObject.layer))>0)
        {
            Debug.Log("Jugador detectado");
            var inventory = other.GetComponentInParent<PlayerInventory>();
            Debug.Log(inventory);
            if (inventory != null)
            {
                inventory.AddKey(keyID); // Da la llave
                Debug.Log("Key registrado");
                Destroy(gameObject);     // Destruye el objeto del suelo
            }
        }
    }
}