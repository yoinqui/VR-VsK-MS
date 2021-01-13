using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;

public class UserManager : MonoBehaviourPunCallbacks
{
    public static GameObject UserMeInstance;

    public Material PlayerLocalMat;
    /// <summary>
    /// Represents the GameObject on which to change the color for the local player
    /// </summary>
    public GameObject GameObjectLocalPlayerColor;

    /// <summary>
    /// The FreeLookCameraRig GameObject to configure for the UserMe
    /// </summary>
    GameObject goFreeLookCameraRig = null;


    void Awake()
    {
        if (photonView.IsMine)
        {
            Debug.LogFormat("Avatar UserMe created for userId {0}", photonView.ViewID);
            UserMeInstance = gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("isLocalPlayer:" + photonView.IsMine);
        updateGoFreeLookCameraRig();
        followLocalPlayer();
        activateLocalPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Get the GameObject of the CameraRig
    /// </summary>
    protected void updateGoFreeLookCameraRig()
    {
        try
        {
            // Get the Camera to set as the followed camera
            goFreeLookCameraRig = GameObject.Find("FreeLookCameraRig");
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Warning, no goFreeLookCameraRig found\n" + ex);
        }
    }

    /// <summary>
    /// Make the CameraRig following the LocalPlayer only.
    /// </summary>
    protected void followLocalPlayer()
    {
        if (photonView.IsMine)
        {
            if (goFreeLookCameraRig != null)
            {
                // find Avatar EthanHips
                Debug.Log("test : " + GameObject.Find("ThirdPersonControllerMultiUser(Clone)").name);
                Transform transformFollow = GameObject.Find("ThirdPersonControllerMultiUser(Clone)/EthanSkeleton/EthanHips").transform;
                // call the SetTarget on the FreeLookCam attached to the FreeLookCameraRig
                goFreeLookCameraRig.GetComponent<FreeLookCam>().SetTarget(transformFollow);                       
                Debug.Log("ThirdPersonControllerMultiuser follow:" + transformFollow);
            }
        }
    }

    protected void activateLocalPlayer()
    {
        // enable the ThirdPersonUserControl if it is a Loacl player = UserMe
        // disable the ThirdPersonUserControl if it is not a Loacl player = UserOther
        //...
        if (photonView.IsMine)
        {
            gameObject.GetComponent<ThirdPersonUserControl>().enabled = true;
            try
            {
                // Change the material of the Ethan Glasses
                GameObjectLocalPlayerColor.GetComponent<MeshRenderer>().material = PlayerLocalMat;
            }
            catch (System.Exception)
            {

            }
        } 
        else
        {
            gameObject.GetComponent<ThirdPersonUserControl>().enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

    }

}
