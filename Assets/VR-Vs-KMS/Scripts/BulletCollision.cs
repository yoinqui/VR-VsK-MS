﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("test");
            if (collision.gameObject.GetComponent<LifeManager>())
            {
                collision.gameObject.GetComponent<LifeManager>().ReduceHealth(1);
            }
            
        }
       
        Destroy(this.gameObject);
        
    }
    
}
