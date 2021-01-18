using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class Shooting : MonoBehaviourPunCallbacks
{
    public Rigidbody BulletPrefab;
    public float Speed;
    private Vector3 origin;

    private void Start()
    {
        
    }

    public void Shoot(Vector3 _origin)
    {
        origin = _origin;
        photonView.RPC("RpcShoot", RpcTarget.All);
    }


    [PunRPC]
    void RpcShoot()
    {
        Rigidbody bulletClone = Instantiate(BulletPrefab, origin + new Vector3(0,0,0), this.transform.rotation);

        bulletClone.AddRelativeForce(Vector3.forward * 1000 * Time.deltaTime, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
