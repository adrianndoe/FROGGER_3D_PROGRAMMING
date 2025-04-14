using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

        void Start()
    {
        Time.timeScale = 1f;
    }
  
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }
}