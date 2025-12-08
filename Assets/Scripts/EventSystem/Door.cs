using System;
using UnityEngine;

public class Door : MonoBehaviour
{
   [Header("Configuracion")] 
   public int myID = 1;
   public string nextLevelName = "Nivel_2"; // Escribe aquí el nombre EXACTO de tu escena
   private bool _isOpen = false;
   private Animator _animator;
   
   [Header("Imágenes")]
   public Sprite closedSprite;
   public Sprite openSprite;
    
   [Header("Componentes")]
   public SpriteRenderer spriteRenderer;
   public LayerMask playerLayer;
   private BoxCollider2D _collider; 

   private void Awake()
   {
      //_animator = GetComponent<Animator>();
      _collider = GetComponent<BoxCollider2D>();
   }
   
   private void OnEnable()
   {        
      EventManager.Subscribe<int>(GlobalEvents.OnDoorOpen, OpenDoor);
      EventManager.Subscribe<int>(GlobalEvents.OnDoorClose, CloseDoor);
   }

   private void OnDisable()
   {
      EventManager.Unsubscribe<int>(GlobalEvents.OnDoorOpen, OpenDoor);
      EventManager.Unsubscribe<int>(GlobalEvents.OnDoorClose, CloseDoor);
   }
   
   private void OpenDoor(int idReceived)
   {
      if (idReceived == myID)
      {
         spriteRenderer.sprite = openSprite; 
         _collider.isTrigger = true;
         _isOpen = true;
      }
   }

   private void CloseDoor(int idReceived)
   {
      if (idReceived == myID)
      {
         spriteRenderer.sprite = closedSprite;
         _collider.isTrigger = false;
         _isOpen = false;
      }
   }
   
   private void OnTriggerEnter2D(Collider2D other)
   {
      Debug.Log("Algo tocó la puerta: " + other.name);
      // Verifica si la puerta está abierta y si el objeto que entra en colisión es el jugador
      if (_isOpen && (playerLayer.value & (1 << other.gameObject.layer)) > 0)
      {
         Debug.Log("¡Nivel Completado!");
         EventManager.Invoke(GlobalEvents.OnLevelComplete, nextLevelName);
      }
   }
}
