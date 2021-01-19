using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public float baseHealthPoints = 100.0f;
    private float healthPoints;

    public delegate void OnDeath(GameObject player);

    /// <summary>
    /// The Death event.
    /// Use like this :
    /// "LifeManager.onDeath += this.myMethode;"
    /// </summary>
    public static event OnDeath onDeath;

    // Start is called before the first frame update
    void Start()
    {
        healthPoints = baseHealthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthPoints <= 0)
        {
            healthPoints = baseHealthPoints;
            if (onDeath != null)
            {
                onDeath(this.gameObject);
            }
        }
    }

    public void ReduceHealth(float damage)
    {
        healthPoints -= damage;
    }
}
