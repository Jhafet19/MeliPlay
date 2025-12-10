using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [Header("Configuración")]
    public int myID = 1;       // Debe ser IGUAL al ID en el script 'Door'
    public bool consumeKey = true; // Si es TRUE, la llave se gasta al abrir
    
    public LayerMask playerLayer;
    
    public GameObject bridge;
    
    [Header("Imágenes")]
    public Sprite openSprite;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckForKey(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckForKey(collision.gameObject);
    }

    private void CheckForKey(GameObject player)
    {
        // Verificamos que el objeto que colisiona es el jugador
        if (playerLayer == (playerLayer | (1 << player.layer)))
        {
            var inventory = player.GetComponentInParent<PlayerInventory>();

            // Verificamos si tiene la llave
            if (inventory != null && inventory.HasKey(myID))
            {
                OpenDoorPermanently(inventory);
            }
            else
            {
                Debug.Log($"Está cerrada. Necesitas la llave {myID}.");
            }
        }
    }

    private void OpenDoorPermanently(PlayerInventory inventory)
    {
        // 1. Gastamos la llave si está configurado así
        if (consumeKey)
        {
            inventory.ConsumeKey(myID);
        }

        // 2. Abrimos la puerta usando EventManager
        EventManager.Invoke(GlobalEvents.OnDoorOpen, myID);


        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && openSprite != null)
        {
            spriteRenderer.sprite = openSprite; 
            bridge.SetActive(true);
        }
    }
}