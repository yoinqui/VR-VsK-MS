using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerShooter : MonoBehaviourPunCallbacks
{
    public Rigidbody BulletPrefab;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Shoot()
    {
        photonView.RPC("RpcControllerShoot", RpcTarget.All);
    }


    [PunRPC]
    void RpcControllerShoot()
    {
        Rigidbody bulletClone = Instantiate(BulletPrefab, transform.position + new Vector3(0, 0, 0), transform.rotation);

        bulletClone.AddRelativeForce(Vector3.forward * 1000 * Time.deltaTime, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
