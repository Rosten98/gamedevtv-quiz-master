using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScore;
    Score score;
    void Awake()
    {
        score = FindObjectOfType<Score>();
    }

    public void ShowFinalScore(){
        finalScore.text = "You have finished the test!\nYou got a score of " + 
                            score.CalculateScore() + "%"; 
    }
}
