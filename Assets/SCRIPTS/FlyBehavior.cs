using UnityEngine;

public class FlyBehavior : MonoBehaviour
{
    public float despawnTime = 10f;
    private bool collected = false;

    void Start()
    {
        // start countdown to auto-despawn if not collected within the despawn time
        Invoke(nameof(DestroyIfNotCollected), despawnTime);
    }
    // destroys object if it is not collected within the despawn time
    void DestroyIfNotCollected()
    {
        if (!collected)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<PlayerMovement>() != null)
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            player.collectedFly = true;

            Destroy(gameObject);
        }
    }
}
