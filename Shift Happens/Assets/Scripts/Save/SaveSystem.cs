using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    

    public static void SaveData(Data data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.save";

        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData sd = new SaveData(data);

        formatter.Serialize(stream, sd);
        stream.Close();

        Debug.Log("SaveGame");

    }


    public static SaveData LoadData()
    {
        string path = Application.persistentDataPath + "/data.save";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData sd = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return sd;
        }
        else
        {
            Debug.Log("Error");
            return null;
        }
    }


}
