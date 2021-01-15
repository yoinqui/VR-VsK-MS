using Photon.Pun;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviourPunCallbacks
{
    public string SceneToLoadAfterAppConfigLoaded;

    public void LobbySceneReturn()
    {
        // Load SceneToLoadAfterAppConfigLoaded
        if (!string.IsNullOrEmpty(SceneToLoadAfterAppConfigLoaded))
        {
            SceneManager.LoadScene(SceneToLoadAfterAppConfigLoaded);
        }
    }
}