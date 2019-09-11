using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;

public class UpgradeHandler : MonoBehaviour
{
    public static PlayerData data;

    public static UpgradeHandler instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            data = new PlayerData();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!LoadData())
        //if(true)
        {
            CreateDefaultData();
            SaveData();
        }
        else Debug.Log("Loaded");

        //Debug.Log(data.towerUpgrades["Radar"]["range"]);
    }

    private void CreateDefaultData()
    {
        data.Reset();
        // handle whatever else might be necessary for default data management here
    }

    // note about saving/loading: need to fix as unity's JsonUtility does not parse dictionaries properly

    public bool SaveData()
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/PlayerData.td";
            FileStream stream = new FileStream(path, FileMode.Create);

            string json = JsonConvert.SerializeObject(data);
            StringContainer strCont = new StringContainer();
            strCont.json = json;
            formatter.Serialize(stream, strCont);
            stream.Close();
            return true;
        }
        catch
        {
            Debug.Log("Failed to create or load save file");
            return false;
        }
    }

    private bool LoadData()
    {
        string path = Application.persistentDataPath + "/PlayerData.td";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            StringContainer strCont = (StringContainer) formatter.Deserialize(stream);
            data = JsonConvert.DeserializeObject<PlayerData>(strCont.json);
            stream.Close();
            return true;
        }
        else
        {
            Debug.Log("Save file not found");
            return false;
        }
    }

    [System.Serializable]
    public class StringContainer
    {
        public string json;
    }
}
