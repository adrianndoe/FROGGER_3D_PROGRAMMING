using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
 //   [HideInInspector]
    public float speed;
    public Vector3 direction = Vector3.right; // which way it moves, default right
    public float destroyDistance = 400f; // how far it can go before destroying

    private Vector3 startPos; // used to remember where it started

    public Vector3 test;   // added for player movement to track the movement of the floating object between updates

    void Start()
    {
        startPos = transform.position; // grab start point, needed to calc distance
    }

    void Update()
    {
        // moves the object
        transform.position += speed * Time.deltaTime * direction.normalized;

        test = speed * Time.deltaTime * direction.normalized;   // added for player movement to track the movement of the floating object between updates

        // figures out how far it has travelled since spawn
        float distanceTravelled = Vector3.Distance(transform.position, startPos);

        // if it goes too far remove the object
        if (distanceTravelled >= destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
