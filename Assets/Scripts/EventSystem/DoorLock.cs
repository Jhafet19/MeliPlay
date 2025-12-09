using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [Header("Configuración")]
    public int myID = 1;       // Debe ser IGUAL al ID en el script 'Door'
    public bool consumeKey = true; // Si es TRUE, la llave se gasta al abrir

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
        if (player.CompareTag("Player"))
        {
            var inventory = player.GetComponent<PlayerInventory>();

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

        // 2. Abrimos la puerta usando tu EventManager
        EventManager.Invoke(GlobalEvents.OnDoorOpen, myID);
        Debug.Log("¡Puerta abierta con llave!");

        // 3. TRUCO IMPORTANTE:
        // Desactivamos el script 'Door' para que se 'desuscriba' de los eventos.
        // Así, si luego pisas una placa y te sales, la puerta NO escuchará la orden de cerrar.
        Door doorVisuals = GetComponent<Door>();
        if (doorVisuals != null)
        {
            doorVisuals.enabled = false; 
        }

        // 4. Nos destruimos a nosotros mismos para no volver a gastar llaves ni procesar colisiones
        Destroy(this);
    }
}