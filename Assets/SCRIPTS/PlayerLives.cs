using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerLives : MonoBehaviour
{
    public TextMeshProUGUI livesCount;

    public int currentLives = 3;

    public Image[] hearts;

    public GameObject gameOverPanel;
    private bool isGameOver = false;
 


    
    void Update()
    {
        
        UpdateHeartsDisplay();
        
        if(currentLives <= 0 && !isGameOver)
        {
            GameOver();
        }
        
    }

    void UpdateHeartsDisplay()
    {
        for (int i = 0; i < hearts.Length; i++)
        {

            hearts[i].enabled = i < currentLives;
        }
    }

    void GameOver()
    {
        SoundManager.PlaySound(SoundTypeEffects.GAME_OVER);
        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
