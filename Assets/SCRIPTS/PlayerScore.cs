using UnityEngine;
using TMPro;


public class PlayerScore : MonoBehaviour
{
    public TextMeshProUGUI scoreCountText;
    public TextMeshProUGUI resultCountText;
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

        if(resultCountText != null)
        {
            resultCountText.text = "Final Score: " + score;
        }
        
    }

    public int GetScore()
{
    return score;
}

}
