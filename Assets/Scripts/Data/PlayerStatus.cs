using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class PlayerStatus 
{
    private static PlayerStatusData statusData;

    private static readonly object padlock = new object();
    private static PlayerStatus instance = null;
    public static PlayerStatus Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new PlayerStatus();
                    instance.LoadStatusData();
                }
                return instance;
            }
        }
    }

    internal void Initialize()
    {
        /// dummy function, initializes Instances and calls Load()
    }


    #region Saving and Loading    
    public void LoadJson()
    {
        FoodDataInfo foodData = new FoodDataInfo();

        TextAsset textAsset = Resources.Load<TextAsset>(GetJsonPath());
        JsonUtility.FromJsonOverwrite(textAsset.text, foodData);

        FoodObject.Instance.FoodDatas = foodData.FoodDatas;
    }    
    
    private void LoadStatusData()
    {
        /// Check if there is a StatusData.sd file. 
        if (StatusFileExists())
        {
            IFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(GetStatusDataPath(), FileMode.Open, FileAccess.Read);
            statusData = (PlayerStatusData)formatter.Deserialize(stream);
            stream.Close();
        }
        else
        {
            Debug.Log("No file found. Creating status data file..");
            CreateStatusDataFile();
        }
    }

    public void SaveStatusData()
    {
        /// Check if there is a StatusData.sd file. 
        if (StatusFileExists())
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(GetStatusDataPath(), FileMode.Create, FileAccess.Write);

            formatter.Serialize(stream, statusData);
            stream.Close();
        }
        else
            CreateStatusDataFile();
    }

    private void CreateStatusDataFile()
    {
        IFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(GetStatusDataPath());
        statusData = new PlayerStatusData();
        formatter.Serialize(file, statusData);
        file.Close();
    }

    private bool StatusFileExists()
    {
        /// Check if StatusData file exists (already saved data).
        return File.Exists(GetStatusDataPath());
    }

    private string GetStatusDataPath()
    {
        return Path.Combine(Application.persistentDataPath + "/StatusData.sd");
    }

    private string GetJsonPath()
    {
        return GlobalVariables.JSON_FILES_PATH + GlobalVariables.JSON_FOOD;
    }
    #endregion

    #region HighScore
    public int GetHighScore()
    {
        return statusData.HighScore;
    }

    public void SetHighScore(int highScore)
    {
        statusData.HighScore = highScore;
    }
    #endregion
}


public class FoodDataInfo
{
    public FoodData[] FoodDatas;
}