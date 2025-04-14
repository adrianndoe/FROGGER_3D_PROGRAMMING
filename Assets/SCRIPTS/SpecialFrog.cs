using UnityEngine;
using System.Collections;

public class SpecialFrog : MonoBehaviour
{
    public float despawnTime = 2f; // Time before disappearing if not collected
    private bool collected = false;
    public System.Action onFrogDestroyed;
    void Start()
    {
        StartCoroutine(DespawnAfterDelay());
    }

    IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(despawnTime);

        if (!collected)
        {
            Debug.Log("[SPECIAL FROG] Despawning after timeout");
            onFrogDestroyed?.Invoke(); // tell spawner frog is gone
            Destroy(gameObject);
        }

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    PlayerMovement player = other.GetComponent<PlayerMovement>();
    //    if (player != null)
    //    {
    //        Debug.Log("[SPECIAL FROG] Collected by player");
    //        collected = true;

    //        player.collectedSpecialFrog = true; // SET FLAG ON PLAYER

    //        Destroy(gameObject);
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        var movement = other.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.collectedSpecialFrog = true;  //  set the flag
            Destroy(gameObject);
        }
    }

}
