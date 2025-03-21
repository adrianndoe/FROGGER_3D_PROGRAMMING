using UnityEngine;
using System.Collections;

public class ObsticalSpawner : MonoBehaviour
{
    [Header("Prefabs & Spawn Settings")]
    public GameObject[] prefabs; // the diff cars we can spawn
    public Transform spawnPointA; // one side of the road
    public Transform spawnPointB; // other side of the road

    [Header("Gap & Timing")]
    public float minGap = 1f; // smallest space allowed between objects (used for checking distance)
    public float maxGap = 3f; // largest gap allowed
    public float minDelay = 0.5f; // how little time we wait before spawning another object
    public float maxDelay = 3f; // how long we wait at most

    [Header("Car Settings")]
    public float moveSpeed = 3f; // how fast the cars move
    public float destroyDistance = 400f; // how far they go before getting destroyed

    private GameObject lastCar; // track the last car we spawned so we don't spawn inside it

    private Transform chosenSpawnPoint; // which spawner this one is using
    private Vector3 moveDirection; // left or right depending on which spawner we use

    void Start()
    {
        // randomly pick one of the spawners to use for this run
        bool useA = Random.value > 0.5f;
        chosenSpawnPoint = useA ? spawnPointA : spawnPointB;
        moveDirection = useA ? Vector3.right : Vector3.left;

        StartCoroutine(SpawnWhenClear()); // start spawning loop
    }

    IEnumerator SpawnWhenClear()
    {
        while (true)
        {
            Vector3 spawnPos = chosenSpawnPoint.position;

            if (lastCar == null)
            {
                // first car, just spawn it
                lastCar = InstantiateCar(spawnPos, moveDirection);
            }
            else
            {
                // check how big the last car was
                BoxCollider col = lastCar.GetComponent<BoxCollider>();
                if (col == null)
                {
                    yield return null;
                    continue;
                }

                // calculate how far the last car needs to be before we can safely spawn a new one
                float objectWidth = col.size.x * lastCar.transform.localScale.x;
                float requiredDistance = objectWidth + Random.Range(minGap, maxGap);

                // wait until the last car has moved far enough
                while (lastCar != null && Vector3.Distance(lastCar.transform.position, spawnPos) < requiredDistance)
                {
                    yield return null;
                }

                // wait a bit before spawning next one
                float delay = Random.Range(minDelay, maxDelay);
                yield return new WaitForSeconds(delay);

                lastCar = InstantiateCar(spawnPos, moveDirection);
            }

            yield return null;
        }
    }

    GameObject InstantiateCar(Vector3 position, Vector3 direction)
    {
        // pick a random car
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        GameObject movingObject = Instantiate(prefab, position, Quaternion.identity);

        // rotate car to face correct direction based on the spawner 
        if (direction == Vector3.right)
        {
            movingObject.transform.rotation = Quaternion.Euler(0, 90f, 0);
        }
        else if (direction == Vector3.left)
        {
            movingObject.transform.rotation = Quaternion.Euler(0, -90f, 0);
        }

        // give it movement
        ObstacleMover mover = movingObject.AddComponent<ObstacleMover>();
        mover.speed = moveSpeed;
        mover.direction = direction;
        mover.destroyDistance = destroyDistance;

        return movingObject;
    }
}
