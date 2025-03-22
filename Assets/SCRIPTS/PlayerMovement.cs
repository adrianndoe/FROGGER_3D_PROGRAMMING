using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private const float JUMP_FORCE = 13.6f;
    private const float ANGLE = 0.70710678f;  // 45 degrees
    private Vector3 _jumpForward = new Vector3(0, ANGLE, ANGLE);
    private Vector3 _jumpRight = new Vector3(ANGLE, ANGLE, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            // only move if the player is on something already
            if (Physics.Raycast(this.transform.position, Vector3.down, this.transform.localScale.y / 2 + 0.1f))
            {
                // multiply the one of 2 unit vectors by the force to apply and a scale for direction to create a movement inpulse
                if (Input.GetKeyDown(KeyCode.UpArrow)) _rb.AddForce(_jumpForward.normalized * JUMP_FORCE, ForceMode.Impulse);
                else if (Input.GetKeyDown(KeyCode.LeftArrow)) _rb.AddForce(Vector3.Scale(_jumpRight.normalized, new Vector3(-1, 1, 1)) * JUMP_FORCE, ForceMode.Impulse);
                else if (Input.GetKeyDown(KeyCode.RightArrow)) _rb.AddForce(_jumpRight.normalized * JUMP_FORCE, ForceMode.Impulse);
                else if (Input.GetKeyDown(KeyCode.DownArrow)) _rb.AddForce(Vector3.Scale(_jumpForward.normalized, new Vector3(1, 1, -1)) * JUMP_FORCE, ForceMode.Impulse);
            }
        }
    }
}

