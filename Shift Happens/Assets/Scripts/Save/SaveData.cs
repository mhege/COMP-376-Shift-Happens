using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData 
{
    public int level;
    public string levelName;


    public SaveData(Data data)
    {
        level = data.level;
        levelName = data.levelName;
    }
}
