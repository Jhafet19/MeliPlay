using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public string lastLevelPlayed;
    public List<LevelDataDetail> levels = new List<LevelDataDetail>();
}

[Serializable]
public struct LevelDataDetail
{
    public string levelName;
    public int coinsCollected;
    public int fruitsCollected;
    public List<string> collectedObjectIds;
}
