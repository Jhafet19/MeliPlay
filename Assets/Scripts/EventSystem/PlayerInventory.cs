using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    // Lista para guardar las llaves (permite tener varias)
    [SerializeField] private List<int> keysCollected = new List<int>();

    public void AddKey(int id)
    {
        keysCollected.Add(id);
        Debug.Log($"Llave {id} recogida.");
    }

    public bool HasKey(int id)
    {
        return keysCollected.Contains(id);
    }

    public void ConsumeKey(int id)
    {
        if (keysCollected.Contains(id))
        {
            keysCollected.Remove(id); // Elimina una llave de la lista
            Debug.Log($"Llave {id} gastada.");
        }
    }
}