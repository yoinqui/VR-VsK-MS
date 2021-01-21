using Photon.Pun;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviourPunCallbacks
{
    public string SceneToLoadAfterAppConfigLoaded;

    public void RoomSceneReturn()
    {

        SceneManager.LoadScene("RoomScene");
    }

    public void LobbySceneReturn()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("LobbyScene");
    }
}