using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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