using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpecialFrogSpawner : MonoBehaviour
{
    public GameObject specialFrogPrefab;
    public float spawnIntervalMin = 15f;
    public float spawnIntervalMax = 45f;

    private GameObject activeFrog; // Keep track of current frog

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float wait = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(wait);

            // Dont spawn if one already exists
            /*if (activeFrog != null)
            {
                continue;
            }*/

            IsSafeWater[] allSafe = Object.FindObjectsByType<IsSafeWater>(FindObjectsSortMode.None);

            List<Transform> validLogs = new List<Transform>();

            foreach (IsSafeWater obj in allSafe)
            {
                if (obj.GetComponent<IsLog>() != null)
                {
                    validLogs.Add(obj.transform);
                }
            }

            if (validLogs.Count == 0)
            {
                continue;
            }

            Transform target = validLogs[Random.Range(0, validLogs.Count)];

            // Store prefab's original scale
            Vector3 originalScale = specialFrogPrefab.transform.localScale;

            GameObject frog = Instantiate(specialFrogPrefab, target.position + Vector3.up * 5f, Quaternion.identity);

            frog.transform.localScale = originalScale;
            frog.transform.SetParent(target, true);

            // Track current frog
            activeFrog = frog;

            // Add despawn logic and untrack
            SpecialFrog selfDestruct = frog.AddComponent<SpecialFrog>();
            selfDestruct.despawnTime = 15f;
            selfDestruct.onFrogDestroyed += () => activeFrog = null; // Reset when destroyed
        }
    }
}
