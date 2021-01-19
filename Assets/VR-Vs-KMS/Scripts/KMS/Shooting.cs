﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class Shooting : MonoBehaviourPunCallbacks
{
    public Rigidbody BulletPrefab;
    public float Speed;

    private void Start()
    {
        //ControllerInput.onGrabPinch += this.Shoot;
    }

    public void Shoot(GameObject origin)
    {
        photonView.RPC("RpcShoot", RpcTarget.All, origin.transform.position);
    }


    [PunRPC]
    void RpcShoot(Vector3 origin)
    {
        Rigidbody bulletClone = Instantiate(BulletPrefab, origin + new Vector3(0,0,0), this.transform.rotation);

        bulletClone.AddRelativeForce(Vector3.forward * 1000 * Time.deltaTime, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
