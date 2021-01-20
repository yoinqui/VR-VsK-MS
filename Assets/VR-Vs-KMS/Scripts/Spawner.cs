using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LifeManager.onDeath += SpawnAfterDeath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnAfterDeath(GameObject player)
    {
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");

        int randomNumber = Random.Range(0, spawns.Length);

        if (GameObject.ReferenceEquals(gameObject, spawns[randomNumber]))
        {
            player.SetActive(false);
            player.transform.position = transform.position;
            player.SetActive(true);
        }
    }
}
