using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsController : MonoBehaviour
{
    
 public void LoadNextLevel()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("LEVEL2"); 
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu");
    }
}
