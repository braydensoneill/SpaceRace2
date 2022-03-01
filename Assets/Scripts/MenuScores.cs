using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class MenuScores : MonoBehaviour
{
    // Text variables
    public TextMeshProUGUI recentScoreText;
    public TextMeshProUGUI highscoreText;

    // File variables
    private string fileName;
    private string filePath;
    private int fileLineCount;
    private int fileLength;
    private string[] lines;


    // Score variables
    private int highscore;
    private string recent1;
    private string recent2;
    private string recent3;
    private string recent4;
    private string recent5;

    // Start is called before the first frame update
    void Start()
    {
        Highscore();    // Change the text for the highscore
        Recents();      // Change the text for the recent scores
    }

    private void Highscore()
    {
        highscore = PlayerPrefs.GetInt("Highscore");    // Find the saved highscore value
        highscoreText.text = highscore.ToString();      // Update the text accordingly
    }

    private void Recents()
    {
        // Create the path for the score history
        fileName = "scores.txt";
        filePath = Application.persistentDataPath + "/" + fileName;
        StreamReader reader = new StreamReader(filePath);
        
        // Place each line of the file into an array
        lines = File.ReadAllLines(filePath);

        if (lines.Length > 0)   //At least one game was played
            recent1 = lines[lines.Length - 1];

        if (lines.Length > 1)   //At least two games were played
            recent2 = lines[lines.Length - 2];

        if (lines.Length > 2)   //At least three games were played
            recent3 = lines[lines.Length - 3];

        if (lines.Length > 3)   //At least four games were played
            recent4 = lines[lines.Length - 4];

        if (lines.Length > 4)   //At least five games were played
            recent5 = lines[lines.Length - 5];

        // Set the text of the recent scores
        recentScoreText.text =
            recent1 + ",    " +
            recent2 + ",    " +
            recent3 + ",    " +
            recent4 + ",    " +
            recent5;

        // Close the reader
        reader.Close();
    }
}
