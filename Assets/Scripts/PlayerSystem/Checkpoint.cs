using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public LayerMask playerLayer;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer))>0)
        {
            KnightController.Instance.UpdateCheckpoint(transform.position);
            
            Debug.Log("Checkpoint Actualizado!");
        }
    }
}
