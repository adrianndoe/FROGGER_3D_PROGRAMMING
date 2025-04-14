using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int lilyPadCollected = 0;

    Scene currentScene;
        void Start()
    {
        Time.timeScale = 1f;
         currentScene = SceneManager.GetActiveScene();

    }

    private void Update()
    {
        if (lilyPadCollected >= 5)
        {
            if (currentScene.name == "LEVEL1")
            {
                lilyPadCollected = 0;
                SceneManager.LoadScene("LEVEL2");
            }
            else
            {
                lilyPadCollected = 0;
                SceneManager.LoadScene("MAINMENU");
            }
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }
}