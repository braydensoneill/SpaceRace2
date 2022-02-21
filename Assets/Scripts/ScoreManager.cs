using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText;   //Displayed during game
    public TextMeshProUGUI highScoreText;   //Displayed on gameOver screen
    public TextMeshProUGUI gameOverScoreText;   //Displayed on gameOver screen

    private int score;   //Current Score
    private int highscore;   //Highest Score

    //Awake is called as the scene starts
    private void Awake()    //This will allow you to instantly access this code from any other script
    {
        instance = this;

        LoadHighScore();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 1;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
        UpdateHighscore();
    }

    private void LoadHighScore()
    {
        if (PlayerPrefs.HasKey("Highscore"))
        {
            highscore = PlayerPrefs.GetInt("Highscore");
            highScoreText.text = "Highscore: " + highscore.ToString();
        }
    }

    public void UpdateScore()
    {
        score = GetScore();

        scoreText.text = score.ToString();  //Text of score during game
        gameOverScoreText.text = score.ToString();  //Text of Score on gameOver screen

        UpdateHighscore();  //Check for new highscore every time score increases
    }

    public void UpdateHighscore()
    {
        if (score > highscore)   //If the player beats the highscore
        {
            highscore = score;  //Set the new highscore

            highScoreText.text = "Highscore: " + highscore.ToString();  //Text of highscore on gameOver screen

            PlayerPrefs.SetInt("Highscore", highscore); //Save the new Highscore so it can be used after closing/restarting the game
        }
    }

    public void ResetScore()
    {
        score = 0;  //Reset score to 0
    }

    public int GetScore()
    {
        return score;   //Get the value of score. Needed for other scripts
    }

    public void AddScore(int i)
    {
        score += i; //Add a desired amount of points to the score variable
    }
}
