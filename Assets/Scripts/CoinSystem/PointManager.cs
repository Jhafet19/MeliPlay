using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField] private int totalPoints = 0;

    private void OnEnable()
    {
        // Nos suscribimos para escuchar cuando alguien da puntos
        EventManager.Subscribe<int>(GlobalEvents.OnAddPoints, AddPoints);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<int>(GlobalEvents.OnAddPoints, AddPoints);
    }

    private void AddPoints(int amount)
        {
            totalPoints += amount;
            
            Debug.Log($"Puntos sumados: {amount}. Total: {totalPoints}");
            // CAMBIO: En vez de buscar al UIManager, lanzamos un aviso general
            // "¡Atención mundo! El puntaje nuevo es 'totalPoints'"
            EventManager.Invoke<int>(GlobalEvents.OnScoreChanged, totalPoints);
        }

    public void ResetPoints()
    {
        totalPoints = 0;
        EventManager.Invoke<int>(GlobalEvents.OnScoreChanged, totalPoints);
    }
}