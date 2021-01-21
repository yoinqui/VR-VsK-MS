using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

public class VR_CameraRigMultiuser : MonoBehaviourPunCallbacks
{
    // reference to SteamController
    public GameObject SteamVRLeft, SteamVRRight, SteamVRCamera, SteamVRBody;
    public GameObject UserOtherLeftHandModel, UserOtherRightHandModel, UserOtherHeadModel;
    private GameObject goFreeLookCameraRig;


    // Use this for initialization
    void Start()
    {
        updateGoFreeLookCameraRig();
        steamVRactivation();
    }

    /// <summary>
    /// deactivate the FreeLookCameraRig since we are using the HTC version
    /// Execute only in client side
    /// </summary>
    protected void updateGoFreeLookCameraRig()
    {
        // Client execution ONLY LOCAL
        if (!photonView.IsMine) return;

        goFreeLookCameraRig = null;

        try
        {
            // Get the Camera to set as the follow camera
            goFreeLookCameraRig = GameObject.Find("FreeLookCameraRig");
            // Deactivate the FreeLookCameraRig since we are using the SteamVR camera
            goFreeLookCameraRig.SetActive(false);
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Warning, no goFreeLookCameraRig found\n" + ex);
        }
    }


    /// <summary>
    /// If we are the client who is using the HTC, activate component of SteamVR in the client using it
    /// If we are not the client using this specific HTC, deactivate some scripts.
    /// </summary>
    protected void steamVRactivation()
    {
        // client execution for ALL

        // Left activation if UserMe, deactivation if UserOther
        if (!photonView.IsMine)
        {
            SteamVRLeft.GetComponent<SteamVR_Behaviour_Pose>().enabled = false;
            SteamVRLeft.GetComponent<ControllerInput>().enabled = false;
        }

        // Left SteamVR_RenderModel activation if UserMe, deactivation if UserOther
        if (!photonView.IsMine)
        {
            Transform model;
            try
            {
                // Get the Camera to set as the follow camera
                model = SteamVRLeft.transform.Find("Model");
                // Deactivate the FreeLookCameraRig since we are using the SteamVR camera
                model.gameObject.GetComponent<SteamVR_RenderModel>().enabled = false;
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Warning, no Model found\n" + ex);
            }
        }

        // Right activation if UserMe, deactivation if UserOther
        if (!photonView.IsMine)
        {
            SteamVRRight.GetComponent<SteamVR_Behaviour_Pose>().enabled = false;
            SteamVRRight.GetComponent<ControllerInput>().enabled = false;
        }

        // Right SteamVR_RenderModel activation if UserMe, deactivation if UserOther
        if (!photonView.IsMine)
        {
            Transform model;
            try
            {
                // Get the Camera to set as the follow camera
                model = SteamVRRight.transform.Find("Model");
                // Deactivate the FreeLookCameraRig since we are using the SteamVR camera
                model.gameObject.GetComponent<SteamVR_RenderModel>().enabled = false;
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Warning, no Model found\n" + ex);
            }
        }

        // Camera activation if UserMe, deactivation if UserOther
        if (!photonView.IsMine)
        {
            SteamVRCamera.GetComponent<Camera>().enabled = false;
            SteamVRCamera.GetComponent<AudioListener>().enabled = false;
            gameObject.transform.Find("ParticleEmiter").gameObject.GetComponent<ParticleSystem>().Stop();
        } 
        else
        {
            gameObject.transform.Find("ParticleEmiter").gameObject.SetActive(false);
            SteamVRBody.SetActive(false);
        }

        if (!photonView.IsMine)
        {
            // ONLY for player OTHER

            // Create the model of the LEFT Hand for the UserOther, use a SteamVR model  Assets/SteamVR/Models/vr_glove_left_model_slim.fbx
            var modelLeft = Instantiate(UserOtherLeftHandModel);
            // Put it as a child of the SteamVRLeft Game Object
            modelLeft.transform.parent = SteamVRLeft.transform;

            // Create the model of the RIGHT Hand for the UserOther Assets/SteamVR/Models/vr_glove_right_model_slim.fbx
            var modelRight = Instantiate(UserOtherRightHandModel);
            // Put it as a child of the SteamVRRight Game Object
            modelRight.transform.parent = SteamVRRight.transform;

            // Create the model of the Head for the UserOther
            var head = Instantiate(UserOtherHeadModel);
            // Put it as a child of the SteamVRRight Game Object
            head.transform.parent = SteamVRCamera.transform;
            head.transform.localScale = new Vector3(2, 2, 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SteamVRBody.transform.position = Vector3.MoveTowards(SteamVRBody.transform.position, SteamVRCamera.transform.position + new Vector3(0, (float)-1, 0), 1);
    }
}
