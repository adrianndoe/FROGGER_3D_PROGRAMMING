using System.Collections;
using UnityEngine;

public class FlySpawner : MonoBehaviour
{
    public GameObject flyPrefab;
    public Transform[] spawnPoints; // 5 lilypad positions
    public float minSpawnDelay = 15f;
    public float maxSpawnDelay = 45f;
    public float flyLifetime = 10f;

    private GameObject currentFly;

    void Start()
    {
        StartCoroutine(SpawnFlyRoutine());
    }

    IEnumerator SpawnFlyRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            if (currentFly == null)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                currentFly = Instantiate(flyPrefab, spawnPoint.position, Quaternion.identity);

                // use the behaviour from the fly behavior script 
                FlyBehavior flyScript = currentFly.AddComponent<FlyBehavior>();
                flyScript.despawnTime = flyLifetime;
            }
        }
    }
}
