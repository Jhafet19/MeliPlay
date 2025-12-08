using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int points = 1;

    public LayerMask playerLayer;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((playerLayer.value & (1 << other.gameObject.layer))>0)
        {
            EventManager.Invoke<int>(GlobalEvents.OnAddFruits, points);
            gameObject.SetActive(false);
        }
    }
}
