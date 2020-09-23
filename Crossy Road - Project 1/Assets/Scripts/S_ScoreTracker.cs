using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class S_ScoreTracker : MonoBehaviour
{
    public int score = 0;
    public int highScore = 0;
    public Text currentScore;

    void Start()
    {
        PlayerPrefs.SetInt("Highscore", highScore);
    }

    void Update()
    {
        currentScore.text = "Score: " + score.ToString();
    }

}
