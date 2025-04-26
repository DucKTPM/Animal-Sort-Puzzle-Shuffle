
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelDataManager : MonoBehaviour
{
    
    [SerializeField] private List<TextAsset> levelDataList;
    private int currentLevelIndex
    {
        get => PlayerPrefs.GetInt("currentLevelIndex", 1);
        set
        {
            PlayerPrefs.SetInt("currentLevelIndex", value);
            PlayerPrefs.Save(); 
        }
    }
    

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
    }

    public int CurrentLevelIndex => currentLevelIndex;

    public void NextLevelIndex()
    {
        currentLevelIndex++;
    }

    public LevelData ReadLevelData()
    {
        /**
        // Application.persistentDataPath; <= Dùng cái này chứ k dc dùng Application.dataPath
        //Mà load json như này k được. Build sẽ lỗi ngay
        string path = Path.Combine(Application.dataPath,"LevelData", "Level_"+currentLevelIndex+".json");//BUG=>
        
        LevelData levelData = new LevelData();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
             levelData = JsonUtility.FromJson<LevelData>(json);
            
           //  Debug.Log("Number slot:" + levelData.standConfig.Length);
           //  foreach (var standConfig in levelData.standConfig)
           //  {
           //      Debug.Log(standConfig.idBirds);
           //      foreach (var value in standConfig.idBirds)
           //      {
           //          Debug.Log(value);
           //      }
           //  }
           // // Debug.Log(levelData.extralConfig.Length);
            
        }
        else
        {
            Debug.Log($"No level data found"+ path);
        }**/
        var levelStringData = levelDataList[currentLevelIndex%levelDataList.Count].text;
        var levelData= JsonUtility.FromJson<LevelData>(levelStringData);
        return levelData;
        
    }
}

[Serializable]
public class StandData
{
    public int type;
    public int numSlot;
}

[Serializable]
public class StandConfig
{
    public int[] idBirds;
    public StandData standData;
    public int side;
}

[Serializable]
public class ExtralsConfig
{
    public int type;
    public PositionData positionData;
    public int[] extralValues;
}
[Serializable]
public class PositionData
{
    public int posType;
    public int indexStand;
    public int indexBird;
}

[Serializable]
public class LevelData
{
    public StandConfig[] standConfig;
    public ExtralsConfig[] extralsConfig;
}


