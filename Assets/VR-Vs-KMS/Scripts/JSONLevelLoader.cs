using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]

public class JSONLevelLoader
{
    private List<Pin> pins;

    private static JSONLevelLoader inst;

    public static JSONLevelLoader Inst
    {
        get
        {
            if (inst == null) inst = new JSONLevelLoader();
            return inst;
        }
    }

    public void UpdateValuesFromJSON(string jsonMap)
    {
        if (!string.IsNullOrEmpty(jsonMap))
        {
            string jsonPath = Path.Combine(Application.streamingAssetsPath, jsonMap+ ".json");
            JsonUtility.FromJsonOverwrite(File.ReadAllText(jsonPath), Inst);
        }
        
    }

    private class Pin
    {
        public Vector3 coordinates;
        public string type;
    }

}
