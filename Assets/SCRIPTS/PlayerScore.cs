using UnityEngine;
using TMPro;


public class PlayerScore : MonoBehaviour
{
    public TextMeshProUGUI scoreCountText;
    private int score;
  


    
    public void AddScore(int points)
    {
    //doing score from time first 
    score += points;
    UpdateUI();
    }



    void UpdateUI()
    {
        if(scoreCountText != null)
        {
            scoreCountText.text = "Score: " + score;
        }
        
    }
}
