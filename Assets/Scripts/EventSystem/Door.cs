using System;
using UnityEngine;

public class Door : MonoBehaviour
{
   [Header("Configuracion")] 
   public int myID = 1;

   [Header("Imágenes")]
   public Sprite closedSprite;
   public Sprite openSprite;
    
   [Header("Componentes")]
   public SpriteRenderer spriteRenderer;
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
         _collider.enabled = false;
      }
   }

   private void CloseDoor(int idReceived)
   {
      if (idReceived == myID)
      {
         spriteRenderer.sprite = closedSprite;
         _collider.enabled = true;
      }
   }
}
