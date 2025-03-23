using UnityEngine;
using static UnityEngine.LightAnchor;
using UnityEngine.SocialPlatforms;
using System;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    //private const float JUMP_FORCE = 13.6f;
    //private const float ANGLE = 0.70710678f;  // 45 degrees
    //private Vector3 _jumpForward = new Vector3(0, ANGLE, ANGLE);
    //private Vector3 _jumpRight = new Vector3(ANGLE, ANGLE, 0);

    private float jumpVelocity;
    private bool isJumping = false;
    private float jumpTimer = 0f;
    private Vector3 startPosition;
    private Vector3 jumpVector;
    private const float JUMP_HEIGHT   = 3f;
    private const float JUMP_DURATION = 1f; // Time to reach the peak
    private const float JUMP_OFFSET = 15.2f; //15.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        jumpVelocity = (2 * JUMP_HEIGHT) / JUMP_DURATION; // Physics equation: v = (2h) / t

    }

    // Update is called once per frame
    void Update()
    {
        // check if frog is on something
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f - new Vector3(0, transform.localScale.y / 2, 0), // Start slightly above the base
                        Vector3.down,
                        out RaycastHit hit,
                        0.2f
                        ))
        {
            if (hit.collider.GetComponent<IsLand>() != null) // Check if it has the script
            {
//                Debug.Log("Landed on isLand prefab with IsLandScript!");
                isJumping = false;
                if (Input.anyKeyDown && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    isJumping = true;
                    jumpTimer = 0f;
                    startPosition = transform.position;
                    if (Input.GetKeyDown(KeyCode.UpArrow)) { jumpVector = new Vector3(0, 1, 1); }
                    if (Input.GetKeyDown(KeyCode.DownArrow)) { jumpVector = new Vector3(0, 1, -1); }
                    if (Input.GetKeyDown(KeyCode.LeftArrow)) { jumpVector = new Vector3(-1, 1, 0); }
                    if (Input.GetKeyDown(KeyCode.RightArrow)) { jumpVector = new Vector3(1, 1, 0); }
                }
            }
            else if (hit.collider.GetComponent<IsSafeWater>() != null)
            {
                isJumping = false;
                Debug.Log("Landed safe");
                Death("not dead");
            }
            else if (hit.collider.GetComponent<IsWater>() != null)
            { 
//                Debug.Log("Landed on isWater prefab");
                isJumping = false;
                Death("drowning");
            }
            else if (hit.collider.GetComponent<IsAligatorOpenMouth>() != null)
            {
                //                Debug.Log("Landed on isWater prefab");
                isJumping = false;
                Death("aligator");
            }

        }
        if (isJumping)
        {
            jumpTimer += Time.deltaTime * 3; // speed up jumping time by a factor of 3
            float yOffset = jumpVelocity * jumpTimer - (0.5f * Physics.gravity.magnitude * jumpTimer * jumpTimer); // calculate y value per update above starting y position
            float forwardOffset = JUMP_OFFSET * jumpTimer; // calculate how far forward the frog will be

            Vector3 newPosition = startPosition + new Vector3(jumpVector.x * forwardOffset, yOffset, jumpVector.z * forwardOffset); // map the new position
            transform.position = newPosition; // startPosition + new Vector3(jumpVector.x * forwardOffset, yOffset, jumpVector.z * forwardOffset);
        }


        // this way moves the frog way too slow
        //if (Input.anyKeyDown)
        //{
        //    // only move if the player is on something already
        //    if (Physics.Raycast(this.transform.position, Vector3.down, this.transform.localScale.y / 2 + 0.1f))
        //    {
        //        // multiply the one of 2 unit vectors by the force to apply and a scale for direction to create a movement inpulse
        //        if (Input.GetKeyDown(KeyCode.UpArrow)) _rb.AddForce(_jumpForward.normalized * JUMP_FORCE, ForceMode.Impulse);
        //        else if (Input.GetKeyDown(KeyCode.LeftArrow)) _rb.AddForce(Vector3.Scale(_jumpRight.normalized, new Vector3(-1, 1, 1)) * JUMP_FORCE, ForceMode.Impulse);
        //        else if (Input.GetKeyDown(KeyCode.RightArrow)) _rb.AddForce(_jumpRight.normalized * JUMP_FORCE, ForceMode.Impulse);
        //        else if (Input.GetKeyDown(KeyCode.DownArrow)) _rb.AddForce(Vector3.Scale(_jumpForward.normalized, new Vector3(1, 1, -1)) * JUMP_FORCE, ForceMode.Impulse);
        //    }
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IsVehicle>() == true)
            Death("vehicle - trigger");
    }

    void Death(string _cause)
    {
        Debug.Log("Death by " + _cause);
        Time.timeScale = 0f; // This will stop time
    }
}

