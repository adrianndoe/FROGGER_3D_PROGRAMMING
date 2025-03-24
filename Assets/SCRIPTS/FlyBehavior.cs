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
    // simply destroys the fly if the frog collides with it
    void OnTriggerEnter(Collider other)
    {
        // Check if object is the frog by name so that means the cube object and not the empty object containing the collider
        if (other.GetComponent<IsFrog>() != null)
        {
            collected = true;

            //  **Add points here for when frog hits fly** + 200 (For the final we would add some audio queue and maybe animation here)
            PlayerScore playerScore = FindAnyObjectByType<PlayerScore>();
            playerScore.AddScore(200);
            Debug.Log("Frog collected the fly!");

            Destroy(gameObject); // Instantly remove the fly when collected
        }
    }
}
