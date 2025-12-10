using System;
using SavingSystem;
using UnityEngine;

public class Door : MonoBehaviour
{
   [Header("Configuracion")] 
   public int myID = 1;
   public string nextLevelName = "Nivel_2"; // Escribe aquí el nombre EXACTO de tu escena
   private bool _isOpen = false;
   private bool _playerIsInside = false;
   private Animator _animator;
   
   [Header("Imágenes")]
   public Sprite closedSprite;
   public Sprite openSprite;
    
   [Header("Componentes")]
   public SpriteRenderer spriteRenderer;
   public LayerMask playerLayer;
   private BoxCollider2D _collider;
   [SerializeField] private GameObject objectToActivate;

   private void Awake()
   {
      //_animator = GetComponent<Animator>();
      _collider = GetComponent<BoxCollider2D>();
      if(objectToActivate != null)
         objectToActivate.SetActive(false);
   }

   private void Update()
   {
      if(_isOpen && Input.GetKeyDown(KeyCode.W))
      {
         LoaderManager.LoadLevel("Nivel_2");
         SaveLoadManager.SaveData();
         EventManager.Invoke(GlobalEvents.OnLevelComplete, nextLevelName);
      }
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
      if ((playerLayer.value & (1 << other.gameObject.layer)) > 0)
      {
         _playerIsInside = true;
         if(_isOpen && objectToActivate != null)
            objectToActivate.SetActive(true);
      }
   }
   
   private void OnTriggerExit2D(Collider2D other)
   {
      if ((playerLayer.value & (1 << other.gameObject.layer)) > 0)
      {
         _playerIsInside = false;
            
         if (objectToActivate != null)
            objectToActivate.SetActive(false); // Ocultar texto
      }
   }
}
