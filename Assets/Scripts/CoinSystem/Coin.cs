using UnityEngine;

public class Coin : MonoBehaviour
{
    public int points = 10;

    void Update()
    {
        transform.Rotate(0f, 0f, 360f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Invoke<int>(GlobalEvents.OnAddPoints, points);
            gameObject.SetActive(false);
        }
    }
}