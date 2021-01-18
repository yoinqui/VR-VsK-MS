using UnityEngine;
using UnityEngine.SceneManagement;

public class GameConfigLoader : MonoBehaviour
{
    public string AppConfigFilePath;
    public string SceneToLoadAfterAppConfigLoaded;

    public void Awake()
    {
        Debug.LogError("InitScene Load !");
        GameConfig.Inst.UpdateValuesFromJSON(AppConfigFilePath);

        JSONLevelLoader.Inst.UpdateValuesFromJSON(AppConfigFilePath);

        ///////////////////////////// LOAD OTHER SCENE /////////////////////////////

        // Load SceneToLoadAfterAppConfigLoaded
        if (!string.IsNullOrEmpty(SceneToLoadAfterAppConfigLoaded))
        {
            SceneManager.LoadScene(SceneToLoadAfterAppConfigLoaded);
        }
    }
}
