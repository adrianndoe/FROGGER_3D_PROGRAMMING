using UnityEngine;

public class PylonSpawner : MonoBehaviour
{
    [Header("Pylon Settings")]
    public GameObject pylonPrefab;

    [Tooltip("List of possible spawn points")]
    public Transform[] spawnPoints;

    void Start()
    {
        if (pylonPrefab == null)
        {
            Debug.LogError("Pylon prefab not assigned!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        // Choose random spawn point
        int index = Random.Range(0, spawnPoints.Length);
        Transform chosenSpawn = spawnPoints[index];

        // Spawn the pylon
        Instantiate(pylonPrefab, chosenSpawn.position, chosenSpawn.rotation);

        Debug.Log($"[PYLON] Spawned at: {chosenSpawn.name}");
    }
}
