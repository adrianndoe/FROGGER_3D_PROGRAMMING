using UnityEngine;
using static UnityEngine.LightAnchor;
using UnityEngine.SocialPlatforms;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private float jumpVelocity;
    private bool isJumping = false;
    private float jumpTimer = 0f;
    private Vector3 startPosition;
    private Vector3 jumpVector;
    private const float JUMP_HEIGHT   = 7f;  // demo 3f
    private const float JUMP_DURATION = 1.35f; // demo 1f  // Time to reach the peak
    private const float JUMP_OFFSET = 8.5f; // demo 15.2f;
    private Vector3 lastSafePosition; //ADDED THIS
    private Vector3 parentMovement = new Vector3(0,0,0);

    private bool activeCollision;
    private Vector3 platformVector;

    private bool ignoreCollisions = false; // used to disable collisions when attaching frog to a moving platform

    //Added for collecting fly and special frog
    [HideInInspector] public bool collectedSpecialFrog = false; // THIS WAS ADDED (Used to recognize if frog gets lady frog) **Points**
    [HideInInspector] public bool collectedFly = false; // THIS WAS ADDED (Used to recognize if frog got a fly before touching lillypad) **Points**
    private bool reachedLilyPad = false; // Prevent multiple triggers (not fully implemented yet)
    [SerializeField] private GameObject frogPrefab;
    [SerializeField] private ParticleSystem particles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        jumpVelocity = (2 * JUMP_HEIGHT) / JUMP_DURATION; // Physics equation: v = (2h) / t
        activeCollision = false;
        lastSafePosition = transform.position; // THIS WAS ADDED
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f - new Vector3(0, transform.localScale.y / 2, 0), // Start slightly above the base
                        Vector3.down,
                        out RaycastHit hit,
                        0.2f
                        ) || (transform.parent.parent != null && transform.parent.parent.GetComponent<IsSafeWater>()))
        {
            if ((transform.parent.parent != null && transform.parent.parent.GetComponent<IsSafeWater>()) || hit.collider.GetComponent<IsLand>()) // Check if it has the script
            {
                    Debug.Log("On something safe!");
                isJumping = false;
                lastSafePosition = transform.position;
                if (Input.anyKeyDown && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    isJumping = true;
                    jumpTimer = 0f;
                    startPosition = transform.position;
                    if (Input.GetKeyDown(KeyCode.UpArrow)) { jumpVector = new Vector3(0, 1, 1); }
                    if (Input.GetKeyDown(KeyCode.DownArrow)) { jumpVector = new Vector3(0, 1, -1); }
                    if (Input.GetKeyDown(KeyCode.LeftArrow)) { jumpVector = new Vector3(-0.5f, 1f, 0); }
                    if (Input.GetKeyDown(KeyCode.RightArrow)) { jumpVector = new Vector3(0.5f, 1f, 0); }

                    // determine the speed of the object the player is standing on, if on an object
                    parentMovement = new Vector3(0, 0, 0);
                    if (transform.parent.parent != null && jumpVector.x != 0)
                        parentMovement = 1.0f * transform.parent.parent.GetComponent<ObstacleMover>().speed * transform.parent.parent.GetComponent<ObstacleMover>().direction;

                }
            }
            else if (hit.collider.GetComponent<IsLilyPad>() != null)
            {
                isJumping = false;
                Debug.Log("Landed safe");

                PlayerScore playerScore = FindAnyObjectByType<PlayerScore>(); //get access to player score
                Timer timer = FindAnyObjectByType<Timer>(); //get time for bonus points
                int timeScore = (int)(timer.currentTime / 0.5f) * 10; //get additional 10 points for every .5 seconds left on timer
                playerScore.AddScore(timeScore);
                Instantiate(frogPrefab, hit.collider.transform.position, Quaternion.identity);
                Instantiate(particles, hit.collider.transform.position, Quaternion.identity);
                // BONUS FOR SPECIAL FROG
                if (collectedSpecialFrog)
                {
                    playerScore.AddScore(200);
                    collectedSpecialFrog = false;
                }
                // BONUS FOR FLY
                if (collectedFly)
                {
                    playerScore.AddScore(200);
                    collectedFly = false;
                }

                //below is implementation of reaching normal  lillypad

                playerScore.AddScore(100);


                // Reset the lily pad flag after short delay so we can score again next time
                //Invoke(nameof(AllowLilyPadTrigger), 0.2f);

                if (transform.parent != null)
                    transform.parent.SetParent(null);

                ResetPosition();




                // BRUNO: I'm sure you have a reason but i couldn't figure it out this
                // it removes a life im assuming you haven't implemented whats suppose to happen when it hits a lillypad
                // as thats the only instance of Death("not dead") i would assume would be called
                //Death("not dead");
            }
        }
        if (isJumping)
        {
            Debug.Log("Parent velocity:" + parentMovement);
            if (transform.parent.parent != null) transform.parent.parent = null;   // if frogger is attached to an island (turtle, log, aligator), detatch as it jumps
            jumpTimer += Time.deltaTime * 3; // speed up jumping time by a factor of 3
            float yOffset = jumpVelocity * jumpTimer - (0.5f * Physics.gravity.magnitude * jumpTimer * jumpTimer); // calculate y value per update above starting y position
            float forwardOffset = JUMP_OFFSET * jumpTimer; // calculate how far forward the frog will be

            Vector3 newPosition = startPosition + new Vector3(jumpVector.x * forwardOffset,
                yOffset, jumpVector.z * forwardOffset) + parentMovement * jumpTimer / 3;

            if (newPosition.z < -97.3) newPosition.z = -97.3f; // don't allow jumping off the bottom edge of the screen
            if (newPosition.x >   136) newPosition.x =  136;   // don't allow jumping off the right  edge of the screen
            if (newPosition.x <  -136) newPosition.x = -136;   // don't allow jumping off the left   edge of the screen

            if (activeCollision == false)
                transform.position = newPosition; // startPosition + new Vector3(jumpVector.x * forwardOffset, yOffset, jumpVector.z * forwardOffset);
            activeCollision = false;
        }
        if (transform.position.x < -95) Death("driven out of bounds - left");
        else if (transform.position.x > 95) Death("driven out of bounds - right");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!ignoreCollisions)
        {
            Debug.Log("Triggered with: " + other.name);
            if (other.gameObject.GetComponent<IsVehicle>() == true)
                Death("vehicle - trigger");
            else if (other.gameObject.GetComponent<IsSafeWater>() != null)
            {
                Debug.Log("Collision with turtles");
                float newYCoord = other.transform.position.y + this.transform.localScale.y / 2 + 0.1f; // find the top of the landing pad with the collider in it for the floating object
             
                Vector3 newPosition = new Vector3(transform.position.x, newYCoord, other.transform.position.z);
                
                Debug.Log("COLLISION SPEED: " + other.transform.InverseTransformDirection(new Vector3(0, 0, 0)));
  //              ignoreCollisions = true;    // disable collisions for the setparent because it moves the frog into the water
                transform.parent.SetParent(other.transform, true); // set the parent without moving the object
                transform.position = newPosition;                  // update position to on top of floating object
                ignoreCollisions = false;   // after position update, re-enable collisions
                isJumping = false;
            }
            else if (other.gameObject.GetComponent<IsWater>() == true && activeCollision == false)
                Death("water");
            else if (other.GetComponent<IsAligatorOpenMouth>() != null)
                Death("aligator");

           /* if (other.GetComponent<IsLilyPad>() != null)
            {
                isJumping = false;
                Debug.Log("Frog landed on lily pad");
                //below is implementation of time score upon reachin lilly pad
                PlayerScore playerScore = FindAnyObjectByType<PlayerScore>(); //get access to player score
                Timer timer = FindAnyObjectByType<Timer>(); //get time for bonus points
                int timeScore = (int)(timer.currentTime / 0.5f) * 10; //get additional 10 points for every .5 seconds left on timer
                playerScore.AddScore(timeScore);
                
                // BONUS FOR SPECIAL FROG
                if (collectedSpecialFrog)
                {
                    playerScore.AddScore(200);
                    collectedSpecialFrog = false;
                }
                // BONUS FOR FLY
                if (collectedFly)
                {
                    playerScore.AddScore(200);
                    collectedFly = false;
                }

                //below is implementation of reaching normal  lillypad
               
                playerScore.AddScore(100);


                // Reset the lily pad flag after short delay so we can score again next time
                Invoke(nameof(AllowLilyPadTrigger), 2f);

                if (transform.parent != null)
                    transform.parent.SetParent(null);

                ResetPosition();
                //Time.timeScale = 0f;
            }*/



            // if the frog touches the pylon it will return the frog to its original location
            else if (other.GetComponent<IsPylon>() != null) 
            {
                isJumping = false;

                // Bounce frog back to last safe position
                transform.position = lastSafePosition; 
            }

        }

    }

    void LandSafeInWater(Collider _other)
    {
        if (_other != null)
        {
            Debug.Log("Landed on: " + _other.name);
            float newYCoord = _other.transform.position.y + this.transform.localScale.y / 2 + 0.1f; // find the top of the landing pad with the collider in it for the floating object
            Debug.Log(newYCoord);
            this.transform.position = new Vector3(this.transform.position.x, newYCoord, this.transform.position.z); // update position to on top of floating object
            isJumping = false;
            transform.parent.parent = _other.transform;  // stand on the moving log - when you jump off, set parent to null
        }
    }
    

    void Death(string _cause)
    {
        if (transform.parent.parent != null) transform.parent.parent = null; // detatch from the object you were on if on one
        isJumping = false;
        Debug.Log("Life Lost By " + _cause);

        //Below is Tristan Modifications for lives || Sidenote, removed carey's time stop above.
        PlayerLives lives = FindAnyObjectByType<PlayerLives>();
        if(lives != null)
        {
            lives.currentLives--;
            Debug.Log("Lives Left" + lives.currentLives);
            if(lives.currentLives > 0)
            {
                ResetPosition();
            }
            else
            {
                Debug.Log("No lives left. ending game");
                Time.timeScale = 0f; //stopping game time
            }
        }
    }

    void ResetPosition()
    {
        Debug.Log("Reset position");
        if (transform.parent != null)
            transform.parent.SetParent(null); // Always unparent before moving
        transform.position = new Vector3(0, 1, -97.3f);
    }

    void AllowLilyPadTrigger()
    {
        reachedLilyPad = false;
    }
}

