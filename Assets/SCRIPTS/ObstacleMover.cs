using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed; // how fast the car moves
    public Vector3 direction = Vector3.right; // which way it moves, default right
    public float destroyDistance = 400f; // how far it can go before destroying

    private Vector3 startPos; // used to remember where it started

    void Start()
    {
        startPos = transform.position; // grab start point, needed to calc distance
    }

    void Update()
    {
        // moves the object
        transform.position += direction.normalized * speed * Time.deltaTime;

        // figures out how far it has travelled since spawn
        float distanceTravelled = Vector3.Distance(transform.position, startPos);

        // if it goes too far remove the object
        if (distanceTravelled >= destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
