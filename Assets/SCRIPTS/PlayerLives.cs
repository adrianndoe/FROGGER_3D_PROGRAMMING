using UnityEngine;
using TMPro;


public class PlayerLives : MonoBehaviour
{
    public TextMeshProUGUI livesCount;

    public int currentLives = 3;


    
    void Update()
    {
        
      
        livesCount.text = "Lives: " + currentLives;
        
    }
}
