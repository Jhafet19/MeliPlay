using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Configuración")] 
    public int puzzleID = 1;
    public LayerMask layerMask;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(((1 << other.gameObject.layer) & layerMask) != 0)
        {
            Debug.Log("Pressure Plate Activated");
            EventManager.Invoke<int>(GlobalEvents.OnDoorOpen, puzzleID);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & layerMask) != 0)
        {
            Debug.Log("Pressure Plate Desactived");
            EventManager.Invoke<int>(GlobalEvents.OnDoorClose, puzzleID);
        }
    }
    
}
