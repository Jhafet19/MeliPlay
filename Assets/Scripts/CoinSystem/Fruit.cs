using UnityEngine;

public class Fruit : MonoBehaviour
{
    [Header("Configuración de Fruta")]
    [SerializeField] private string uniqueID;
    public int points = 1;
    public LayerMask playerLayer;
    
    [Header("Audio SFX")]
    public AudioClip fruitSound;
   
    private void Awake()
    {
        // Validación de seguridad
        if (string.IsNullOrEmpty(uniqueID)) 
        {
            GenerateID();
        }
    }
    
    private void Start()
    {
        var manager = FindFirstObjectByType<PointManager>();
        
        if (manager != null)
        {
            if (manager.IsAlreadyCollected(uniqueID))
            {
                // Si ya estoy en la lista, me desactivo inmediatamente
                gameObject.SetActive(false);
            }
        }
    }
    
    private void Collect()
    {
        var manager = FindFirstObjectByType<PointManager>();
        if (manager != null)
        {
            manager.RegisterCollection(uniqueID);
        }
        EventManager.Invoke(GlobalEvents.OnAddFruits, points);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer))>0)
        {
            Collect();
            PlayFruitSound();
            gameObject.SetActive(false);
        }
    }
    
    [ContextMenu("Generar Nuevo ID")]
    private void GenerateID()
    {
        uniqueID = System.Guid.NewGuid().ToString();
    }
    
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(uniqueID))
        {
            GenerateID();
        }
    }
    
    public void PlayFruitSound()
    {
        if (fruitSound != null)
        {
            AudioSource.PlayClipAtPoint(fruitSound, transform.position, .7f);
        }
    }
}
