using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;


    [SerializeField]
    private InputField mapInputField;
    [SerializeField]
    private Dropdown deviceDropdown;
    [SerializeField]
    private GameObject vRErrorMessage;
    [SerializeField]
    private Toggle readyToggle;
    [SerializeField]
    private Button startButton;

    [Tooltip("The Ui Panel to let the user select his device and indicate that his is ready to start the game")]
    [SerializeField]
    private GameObject controlPanel;

    [Tooltip("The Ui Panel to add the player on room join")]
    [SerializeField]
    private Transform playersListTransform;

    public GameObject playerNamePrefab;

    private bool isEveryPlayerReady = false;

    #region Photon Callbacks


    /// <summary>
    /// Called when the local player left the room. 
    /// </summary>
    public override void OnLeftRoom()
    {
        // TODO: load the Lobby Scene

    }


    /// <summary>
    /// Called when Other Player enters the room and Only other players
    /// </summary>
    /// <param name="other"></param>
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
                                                                      // TODO: 
        GameObject playerNameGO = Instantiate(playerNamePrefab, playersListTransform);
        playerNameGO.name = other.NickName;
        playerNameGO.GetComponent<Text>().text = other.NickName;
    }

    /// <summary>
    /// Called when Other Player leaves the room and Only other players
    /// </summary>
    /// <param name="other"></param>
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
                                                                   // TODO: 
        Destroy(GameObject.Find(other.NickName).GetComponent<Text>());
    }
    #endregion

    #region MonoBehaviour CallBacks


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", PhotonNetwork.LocalPlayer.NickName);
    }


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        // try connecting to the PUN server
        //Connect();
        // Now called by the UI.
        JSONLevel.Inst.SpawnCount = 0;
        JSONLevel.Inst.ContaminationAreaCount = 0;
        JSONLevel.Inst.ThrowablesCount = 0;
        controlPanel.SetActive(true);

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    mapInputField.interactable = true;

        //    Debug.LogFormat("OnPlayerEnteredRoom() {0}", PhotonNetwork.LocalPlayer.NickName);
        //    GameObject playerNameGO = Instantiate(playerNamePrefab, playersListTransform);
        //    playerNameGO.name = PhotonNetwork.LocalPlayer.NickName;
        //    playerNameGO.GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
        //} 

        foreach (var player in PhotonNetwork.PlayerList)
        {
            GameObject playerNameGO = Instantiate(playerNamePrefab, playersListTransform);
            playerNameGO.name = player.NickName;
            playerNameGO.GetComponent<Text>().text = player.NickName;
        }

        mapInputField.onEndEdit.AddListener(delegate { photonView.RPC("SetAll", RpcTarget.AllBuffered, mapInputField.text, "map"); });

        SetReady(readyToggle.isOn, PhotonNetwork.LocalPlayer.NickName);

        readyToggle.onValueChanged.AddListener(delegate {
            SetReady(readyToggle.isOn, PhotonNetwork.LocalPlayer.NickName);
            photonView.RPC("SetAll", RpcTarget.All, checkReady().ToString(), "ready"); ;
        });
        
        if (GameConfig.Inst.DeviceUsed.ToLower() == "htc")
        {
            deviceDropdown.value = 1;
        }
        else
        {
            deviceDropdown.value = 0;
        }
    }

    private void Update()
    {
        // Code to leave the room by pressing CTRL + the Leave button
        if (Input.GetButtonUp("Leave"))
        {
            Debug.Log("Leave event");
            LeaveRoom();
        }

        if (PhotonNetwork.IsMasterClient && isEveryPlayerReady)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }

        if (!UnityEngine.XR.XRSettings.isDeviceActive && deviceDropdown.value == 1)
        {
            vRErrorMessage.SetActive(true);
        } else
        {
            vRErrorMessage.SetActive(false);
        }
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Our own function to implement for leaving the Room
    /// </summary>
    public void LeaveRoom()
    {
        // TODO: 
        PhotonNetwork.LeaveRoom();
    }

    public void updatePlayerNumberUI()
    {
        // TODO: Update the playerNumberUI

    }

    public void SetReady(bool ready, string playerName)
    {
        //ready = (bool)PhotonNetwork.LocalPlayer.CustomProperties["Ready"];
        //ready = ready ? false : true;
        PhotonHashtable hash = new PhotonHashtable();
        hash.Add("Ready", ready);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        photonView.RPC("UpdatePlayerListColor", RpcTarget.AllBuffered, ready, playerName);
    }

    public bool checkReady()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!(bool)player.CustomProperties["Ready"])
            {
                return false;
            }
        }
        return true;
    }

    public void StartButtonPressed()
    {
        photonView.RPC("RpcStartGame", RpcTarget.All);
    }

    public void StartGame()
    {
        JSONLevel.Inst.UpdateValuesFromJSON(mapInputField.text);
        GameConfig.Inst.DeviceUsed = deviceDropdown.value == 1 ? "htc" : "pc";
        Debug.Log("We load the scene 'GameScene' ");
        // TODO the room has been joined, so we can load the Scene for startig the application
        //SceneManager.LoadScene("AppConfigLoaderScene");
        
        PhotonNetwork.LoadLevel("GameScene");
    }

    [PunRPC]
    void RpcStartGame()
    {
        StartGame();
    }

    [PunRPC]
    void SetAll(string value, string var)
    {
        switch (var)
        {
            case "ready":
                isEveryPlayerReady = bool.Parse(value);
                break;
            case "map":
                mapInputField.text = value;
                break;
            default:

                break;
        }
    }

    [PunRPC]
    void UpdatePlayerListColor(bool ready, string playerName)
    {
        if (ready)
        {
            GameObject.Find(playerName).GetComponent<Text>().color = Color.green;
        } 
        else
        {
            GameObject.Find(playerName).GetComponent<Text>().color = Color.red;
        }
    }
    #endregion
}
