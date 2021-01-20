using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "VRPlayer")
        {
            Debug.Log("test");
            if (collision.gameObject.GetComponent<LifeManager>())
            {
                collision.gameObject.GetComponent<LifeManager>().ReduceHealth(1);
            }
            
        }

        if (collision.gameObject.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "VRPlayer")
        {
            Debug.Log("test");
            if (other.gameObject.GetComponent<LifeManager>())
            {
                other.gameObject.GetComponent<LifeManager>().ReduceHealth(1);
            }

        }
    }
}
