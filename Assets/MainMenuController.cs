using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject levelSelectPanel;

    public void PlayGame()
    {
        levelSelectPanel.SetActive(true);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("LEVEL1"); // Replace with your actual scene name
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("LEVEL2");
    }

}
