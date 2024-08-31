using Purrcifer.Data.Player;
using UnityEngine;
using Purrcifer.Data.Xml;
using System.Threading;

public class DataCarrier : MonoBehaviour
{
    private static DataCarrier _instance;
    [SerializeField] private GameSaveFileRuntime _runtime;

    public static DataCarrier Instance => _instance;
    public static GameSaveFileRuntime RuntimeData => _instance._runtime;

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }

        string path = XML_Serialization.AppDirPath + "GS.xml";
        bool fileExists = XML_Serialization.DataExists(XML_Serialization.AppDirPath + "GS.xml");

        if (!fileExists)
        {
            _runtime = GameSaveFileRuntime.GetDefault();
        }
        else
        {
            GameSaveFileXML xml = XML_Serialization.Deserialize<GameSaveFileXML>(path);
            _runtime = new GameSaveFileRuntime(xml);
        }
    }

    //private void SaveData()
    //{

    //}

    public static DataCarrier Generate()
    {
        GameObject go = new GameObject("----Data----");
        return go.AddComponent<DataCarrier>();
    }
}
