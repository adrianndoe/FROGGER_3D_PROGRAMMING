using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerCount;
    public float currentTime = 45f;
    private GameObject player;  // Reference to the frog GameObject which will be found later

    public GameObject resultsPanel; //to display score after timer is up
    public TextMeshProUGUI finalScoreText; //text displaying score

    private PlayerScore playerScore;

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

        //for player end score
        playerScore = FindAnyObjectByType<PlayerScore>();
        if(playerScore == null)
        {
            Debug.Log("Player Score not found");
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
                SoundManager.PlaySound(SoundTypeEffects.OUT_OF_TIME);
                Destroy(player);
                Time.timeScale = 0f;
            }
            else
            {
                Debug.Log("Frog GameObject not set!");
            }

 //           SoundManager.PlaySound(SoundTypeEffects.GAME_OVER);
            ShowResults();
            Time.timeScale = 0f;
        }

    }

    void ShowResults()
    {
        if (resultsPanel != null)
        {
            resultsPanel.SetActive(true);
            if(finalScoreText != null && playerScore != null)
            {
                finalScoreText.text = "Final Score: " + playerScore.GetScore();
            }
        }
    }
}
