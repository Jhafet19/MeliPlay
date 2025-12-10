using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SavingSystem
{
    public class SaveLoadManager : MonoBehaviour
    {
        public static void SaveData()
        {
            var path = Application.persistentDataPath + "/gameData.json";

            Debug.Log("GUARDADO EN: " + path);
            
            var data = new GameData();

            foreach (var savable in GetSaveDataProviders())
            {
                savable?.Save(ref data);
            }

            var json = JsonUtility.ToJson(data, true);
            System.IO.File.WriteAllText(path, json);

            Debug.Log("Saved data");
        }

        private static IEnumerable<ISavable> GetSaveDataProviders()
        {
            var savables = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)
                .Where(m => typeof(ISavable).IsAssignableFrom(m.GetType()))
                .Select(m => m as ISavable);
            return savables;
        }

        public static void LoadData()
        {
            var path = Application.persistentDataPath + "/gameData.json";

            if (!System.IO.File.Exists(path))
            {
                Debug.Log("No se encontró archivo de guardado. Iniciando partida nueva.");
                return;
            }

            var json = System.IO.File.ReadAllText(path);
            var gameData = JsonUtility.FromJson<GameData>(json);

            if (gameData is null)
            {
                Debug.LogError("Cannot load. Data object is null");
                return;
            }

            foreach (var savable in GetSaveDataProviders())
            {
                savable?.Load(ref gameData);
            }

            Debug.Log("Loaded data");
        }
    }
}