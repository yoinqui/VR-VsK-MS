using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LifeManager : MonoBehaviour
{
    public float baseHealthPoints = 10.0f;
    private float healthPoints;
    public GameObject lifeBar;

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
            if (onDeath != null)
            {
                onDeath(this.gameObject);
                DataGame.Inst.UpdateNbContaminatedPlayer(this.gameObject);
            }
            healthPoints = baseHealthPoints;
        }
    }

    public void ReduceHealth(float damage)
    {
        healthPoints -= damage;
        Debug.Log(healthPoints);
        LifeBarUpdate();
    }

    public void LifeBarUpdate()
    {
        lifeBar.GetComponent<Image>().fillAmount = healthPoints / 10;
    }
}