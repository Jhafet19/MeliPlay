using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;

    private int totalPoints = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddPoints(int amount)
    {
        totalPoints += amount;
        UIManager.Instance.UpdateScore(totalPoints);
    }

    public int GetPoints()
    {
        return totalPoints;
    }
}