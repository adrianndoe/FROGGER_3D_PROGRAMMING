using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerCount;
    public float currentTime = 45f;
    private GameObject player;  // Reference to the frog GameObject which will be found later

    void Start()
    {
        // Find the PlayerMovement script since it references the player
        PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (playerMovement != null)
        {
            // find player
            player = playerMovement.gameObject;
           
        }
        else
        {
            Debug.Log("PlayerMovement component not found in the scene.");
        }
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        timerCount.text = "Time Remaining: " + currentTime.ToString("F2") + "s";

        if (currentTime <= 0)
        {
            currentTime = 0;
            if (player != null)
            {
                Destroy(player);
                Time.timeScale = 0f;
            }
            else
            {
                Debug.Log("Frog GameObject not set!");
            }
        }
    }
}
