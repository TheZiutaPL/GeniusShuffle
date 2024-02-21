using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class SaveData
{
    public PlayerStats[] levelStats;

    public SaveData(int levels)
    {
        levelStats = new PlayerStats[levels];
    }
}

public class SaveManager : MonoBehaviour
{
    private bool saveFound = false;

    private SaveData loadedSave;

    private void Awake()
    {
        Load();
    }

    private void Start()
    {
        if (saveFound)
            CollectionManager.SetLevelStats(loadedSave.levelStats);
    }

    public static SaveData LevelsToData()
    {
        List<CampaignLevel> campaignLevels = CollectionManager.GetLevels();

        int levelCount = campaignLevels.Count;
        SaveData saveData = new SaveData(levelCount);
        
        for (int i = 0; i < levelCount; i++)
        {
            saveData.levelStats[i] = campaignLevels[i].LevelStats;
        }

        return saveData;
    }

    public static void Save(SaveData saveData)
    {
        Debug.Log("Saved game");

        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "/playerData.dat");
        bf.Serialize(file, saveData);
        file.Close();
    }

    private void Load()
    {
        saveFound = File.Exists(Application.persistentDataPath + "/playerData.dat");

        Debug.Log("Loaded files: " + saveFound);

        if (saveFound)
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);
            loadedSave = (SaveData)bf.Deserialize(file);
            file.Close();
        }
    }
}
