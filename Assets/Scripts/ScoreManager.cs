using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText;           //Displayed during game
    public TextMeshProUGUI highScoreText;       //Displayed on gameOver screen
    public TextMeshProUGUI gameOverScoreText;   //Displayed on gameOver screen

    private int score;      //Current Score
    private int highscore;   //Highest Score

    // Variables for writing to file
    private StreamWriter writer;
    private string filePath;
    private string fileName;

    //Awake is called as the scene starts
    private void Awake()    //This will allow you to instantly access this code from any other script
    {
        instance = this;

        // Variables for writing to file
        fileName = "scores.txt";
        filePath = Application.persistentDataPath + "/" + fileName;
        writer = new StreamWriter(filePath, true);

        // Load the players highscore
        LoadHighScore();
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;  // Reset the score
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();      // Update the score value/text
        UpdateHighscore();  // Update the highscore value/text
    }

    private void LoadHighScore()
    {
        // Check if there is a highscore playerpref saved from previous sessions
        if (PlayerPrefs.HasKey("Highscore"))
        {
            highscore = PlayerPrefs.GetInt("Highscore");                // Update PlayerPref
            highScoreText.text = "Highscore: " + highscore.ToString();  // Update Text
        }
    }

    public void UpdateScore()
    {
        // Update the score value
        score = GetScore();

        scoreText.text = score.ToString();  // Update text
        gameOverScoreText.text = ("Score: ") + score.ToString();  // Update text for game-over screen

        UpdateHighscore();  //Check for new highscore every time score increases
    }

    public void UpdateHighscore()
    {
        // Check ff the player beats the highscore
        if (score > highscore)
        {
            highscore = score;  //Set the new highscore

            highScoreText.text = "Highscore: " + highscore.ToString();  //Text of highscore on gameOver screen
        }
    }

    public void SaveScore()
    {
        // Write the data a text file
        writer.WriteLine(score);
        writer.Flush();
        writer.Close();
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
