using System.Collections.Generic;
using System.Linq;
using SavingSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointManager : MonoBehaviour, ISavable
{
    [SerializeField] private int totalCoins = 0;
    [SerializeField] private int totalFruits = 0;
    private string _currentLevelName;
    private List<string> _collectedIdsInThisSession = new List<string>();
    
    private void Awake()
    {
        _currentLevelName = SceneManager.GetActiveScene().name;
        if (GameManager.Instance.isLoadingFromSave)
        {
            SaveLoadManager.LoadData();
        }else
        {
            ResetCoins();
            ResetFruits();
            _collectedIdsInThisSession.Clear(); 
        }
    }
    
    private void Start()
    {
        RefreshUI();
    }
    
    private void RefreshUI()
    {
        EventManager.Invoke<int>(GlobalEvents.OnScoreCoinChanged, totalCoins);
        EventManager.Invoke<int>(GlobalEvents.OnScoreFruitChanged, totalFruits);
    }
    
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
    
    public void RegisterCollection(string objectID)
    {
        if (!_collectedIdsInThisSession.Contains(objectID))
        {
            _collectedIdsInThisSession.Add(objectID);
        }
    }
    
    public bool IsAlreadyCollected(string objectID)
    {
        return _collectedIdsInThisSession.Contains(objectID);
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

    public void Save(ref GameData gameData)
    {
        gameData.lastLevelPlayed = _currentLevelName;

        var existingDataIndex = gameData.levels.FindIndex(x => x.levelName == _currentLevelName);
        
        LevelDataDetail newData = new LevelDataDetail
        {
            levelName = _currentLevelName,
            coinsCollected = totalCoins,
            fruitsCollected = totalFruits,
            collectedObjectIds = new List<string>(_collectedIdsInThisSession)
        };

        if (existingDataIndex != -1)
        {
            gameData.levels[existingDataIndex] = newData;
        }
        else
        {
            gameData.levels.Add(newData);
        }
    }

    public void Load(ref GameData gameData)
    {
        var data = gameData.levels.FirstOrDefault(x => x.levelName == _currentLevelName);
        if (!string.IsNullOrEmpty(data.levelName))
        {
            totalCoins = data.coinsCollected;
            totalFruits = data.fruitsCollected;
            if (data.collectedObjectIds != null)
            {
                _collectedIdsInThisSession = new List<string>(data.collectedObjectIds);
            }
            else
            {
                _collectedIdsInThisSession = new List<string>();
            }
            EventManager.Invoke<int>(GlobalEvents.OnScoreCoinChanged, totalCoins);
            EventManager.Invoke<int>(GlobalEvents.OnScoreFruitChanged, totalFruits);
        }
    }
}