using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerInput : MonoBehaviour
{
    public SteamVR_Action_Boolean grabPinch = null;
    public SteamVR_Action_Boolean grabGrip = null;
    public SteamVR_Action_Boolean teleport = null;
    private SteamVR_Behaviour_Pose pose = null;
    private SteamVR_Input_Sources inputSources;
    private GameObject selectedObject;
    public ControllerTeleport teleporter;
    public ControllerPointer pointer;
    public Shooting shooter;


    public delegate void OnGrabPinch(GameObject controller);

    public static event OnGrabPinch onGrabPinch;

    public delegate void OnGrabPinchReleased(GameObject controller);

    public static event OnGrabPinchReleased onGrabPinchReleased;


    public delegate void OnGrabPressed(GameObject controller);

    public static event OnGrabPressed onGrabPressed;

    public delegate void OnGrabReleased(GameObject controller);

    public static event OnGrabReleased onGrabReleased;

    void Awake()
    {
        try
        {
            pose = GetComponent<SteamVR_Behaviour_Pose>();
            inputSources = pose.inputSource;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Warning, no Behaviour Pose found\n" + ex);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        pointer.ActivatePointer();
    }

    // Update is called once per frame
    void Update()
    {

        if (grabPinch.GetStateDown(inputSources))
        {
            onGrabPinch?.Invoke(gameObject);
            shooter.Shoot(gameObject);
        }

        if (grabPinch.GetStateUp(inputSources))
        {
            onGrabPinchReleased?.Invoke(gameObject);
        }

        if (grabGrip.GetStateDown(inputSources))
        {
            if (selectedObject != null)
            {
                GrabSelectedObject();
            }

            onGrabPressed?.Invoke(gameObject);
        }

        if (grabGrip.GetStateUp(inputSources))
        {
            if (gameObject.GetComponent<FixedJoint>() != null)
            {
                UngrabTouchedObject();
            }

            onGrabReleased?.Invoke(gameObject);
        }

        if (teleport.GetStateDown(inputSources))
        {
            TeleportPressed();
        }

        if (teleport.GetStateUp(inputSources))
        {
            TeleportReleased();
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GrabbableObject>())
        {
            if (other.GetComponent("Halo"))
            {
                ((Behaviour)other.GetComponent("Halo")).enabled = true;
            }
            selectedObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent("Halo"))
        {
            ((Behaviour)other.GetComponent("Halo")).enabled = false;
        }
        if (selectedObject == other.gameObject)
        {
            selectedObject = null;
        }
    }

    private void GrabSelectedObject()
    {
        Debug.Log("Is grabbed");
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        fx.connectedBody = selectedObject.GetComponent<Rigidbody>();
    }

    private void UngrabTouchedObject()
    {
        Debug.Log("Is released");
        FixedJoint fx = gameObject.GetComponent<FixedJoint>();
        fx.connectedBody.velocity = pose.GetVelocity();
        fx.connectedBody.angularVelocity = pose.GetAngularVelocity();
        fx.connectedBody = null;
        Destroy(fx);
    }

    private void TeleportPressed()
    {
        pointer.DesactivatePointer();
        teleporter.ActivatePointer();

    }

    private void TeleportReleased()
    {
        if (teleporter.CanTeleport)
        {
            StartCoroutine(teleporter.WaitTeleportReloaded());
            GameObject cameraRig = GameObject.Find("[CameraRigMultiUser](Clone)");
            GameObject camera = GameObject.Find("Camera").gameObject;
            Vector3 positionDifference = camera.transform.position - cameraRig.transform.position;
            cameraRig.transform.position = teleporter.TargetPosition - new Vector3(positionDifference.x, 0, positionDifference.z);
        }
        teleporter.DesactivatePointer();
        pointer.ActivatePointer();
    }

    
}
