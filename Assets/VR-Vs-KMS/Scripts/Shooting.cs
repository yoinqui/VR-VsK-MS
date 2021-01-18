using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class Shooting : MonoBehaviourPunCallbacks

{

    public Rigidbody BulletPrefab;
    public float Speed;
    public GameObject muzzle;

    private void Start()
    {
        
    }

    public void shoot()
    {
        photonView.RPC("RpcShoot", RpcTarget.All);
    }


    [PunRPC]
    void RpcShoot()
    {
        Rigidbody bulletClone = Instantiate(BulletPrefab, muzzle.transform.position + new Vector3(0,0,0), this.transform.rotation);

        bulletClone.AddRelativeForce(Vector3.forward * 1000 * Time.deltaTime, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && photonView.IsMine)
        {
            shoot();
        }
    }


}
