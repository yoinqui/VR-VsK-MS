using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PhotonTransformChildView : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool SynchronizePosition = true;
    public bool SynchronizeRotation = true;
    public bool SynchronizeScale = false;

    public List<Transform> SynchronizedChildTransform;
    private List<Vector3> localPositionList;
    private List<Quaternion> localRotationList;
    private List<Vector3> localScaleList;


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            foreach (Transform transform in SynchronizedChildTransform)
            {
                if (SynchronizePosition)
                {
                    stream.SendNext(transform.localPosition);
                }
                

                if (SynchronizeRotation)
                {
                    stream.SendNext(transform.localRotation);
                }
                

                if (SynchronizeScale)
                {
                    stream.SendNext(transform.localScale);
                }
            }
        }
        else
        {
            for (int i = 0; i < SynchronizedChildTransform.Count; i++)
            {
                if (SynchronizePosition)
                {
                    localPositionList[i] = (Vector3)stream.ReceiveNext();
                }

                if (SynchronizeRotation)
                {
                    localRotationList[i] = (Quaternion)stream.ReceiveNext();
                }

                if (SynchronizeScale)
                {
                    localScaleList[i] = (Vector3)stream.ReceiveNext();
                }
                    
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        localPositionList = new List<Vector3>();
        localRotationList = new List<Quaternion>();
        localScaleList = new List<Vector3>();

        foreach (Transform transform in SynchronizedChildTransform)
        {
            localPositionList.Add(transform.localPosition);
            localRotationList.Add(transform.localRotation);
            localScaleList.Add(transform.localScale);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            for (int i = 0; i < SynchronizedChildTransform.Count; i++)
            {
                if (SynchronizePosition)
                    SynchronizedChildTransform[i].transform.localPosition = localPositionList[i];

                if (SynchronizeRotation)
                    SynchronizedChildTransform[i].transform.localRotation = localRotationList[i];

                if (SynchronizeScale)
                    SynchronizedChildTransform[i].transform.localScale = localScaleList[i];
            }
        }
    }
}
