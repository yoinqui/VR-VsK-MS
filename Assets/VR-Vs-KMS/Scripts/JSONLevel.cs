using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class JSONLevel
{
    public List<Pin> SpawnList;
    public List<Pin> ContaminationAreaList;
    public List<Pin> ThrowablesList;

    public int SpawnCount;
    public int ContaminationAreaCount;
    public int ThrowablesCount;

    private static JSONLevel inst;

    public static JSONLevel Inst
    {
        get
        {
            if (inst == null) inst = new JSONLevel();
            return inst;
        }
    }

    public void UpdateValuesFromJSON(string jsonMap)
    {
        if (!string.IsNullOrEmpty(jsonMap))
        {
            string jsonPath = Path.Combine(Application.streamingAssetsPath, jsonMap + ".json");
            JsonUtility.FromJsonOverwrite(File.ReadAllText(jsonPath), Inst);
        }      
    }
}

public class Pin
{
    public Vector3 coordinates;
    public string type;
}
