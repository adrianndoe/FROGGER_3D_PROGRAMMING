using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }
}