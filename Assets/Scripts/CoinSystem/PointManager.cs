using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField] private int totalCoins = 0;
    [SerializeField] private int totalFruits = 0;

    private void OnEnable()
    {
        EventManager.Subscribe<int>(GlobalEvents.OnAddCoins, AddCoins);
        EventManager.Subscribe<int>(GlobalEvents.OnAddFruits, AddFruits);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<int>(GlobalEvents.OnAddCoins, AddCoins);
        EventManager.Unsubscribe<int>(GlobalEvents.OnAddFruits, AddFruits);
    }

    private void AddCoins(int amount)
    {
        totalCoins += amount;
        
        EventManager.Invoke<int>(GlobalEvents.OnScoreCoinChanged, totalCoins);
    }
    
    private void AddFruits(int amount)
    {
        totalFruits += amount;
        
        EventManager.Invoke<int>(GlobalEvents.OnScoreFruitChanged, totalFruits);
    }

    public void ResetCoins()
    {
        totalCoins = 0;
        EventManager.Invoke<int>(GlobalEvents.OnScoreCoinChanged, totalCoins);
    }
    
    public void ResetFruits()
    {
        totalFruits = 0;
        EventManager.Invoke<int>(GlobalEvents.OnScoreFruitChanged, totalFruits);
    }
}