using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject groundTile1;
    [SerializeField] GameObject groundTile2;
    GameObject frontTile;

    [Header("Main Menu")]
    public GameObject helpPanel;
    public Text highscore;
    public InputField name;
    public Text playerNameText;
    public GameObject highscoreTitle;
    public GameObject leaderboardPanel;
    public GameObject cSelect; //Character select
    


    private void Start()
    {
        frontTile = groundTile1;
        highscore.text = PlayerPrefs.GetString("HighPlayerName").ToString() + ": " + PlayerPrefs.GetInt("Highscore").ToString();

        groundTile1.GetComponent<S_groundTile>().OnBeginPlayPath();
        groundTile2.GetComponent<S_groundTile>().Shuffle();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) //developer key
        {
            OnClickClearPrefs();
        }

        if(PlayerPrefs.GetInt("Highscore") == 0)
        {
            highscore.gameObject.SetActive(false);
            highscoreTitle.SetActive(false);
        }
        else
        {
            highscore.gameObject.SetActive(true);
            highscoreTitle.SetActive(true);
        }

    }

    public void OnFlipButtonClick()
    {
        frontTile.transform.position = Vector3.right * (frontTile.transform.position.x + 20);
        frontTile.GetComponent<S_groundTile>().ClearArray();
        frontTile.GetComponent<S_groundTile>().Shuffle();

        if (frontTile == groundTile1)
        {
            frontTile = groundTile2;
        }
        else
        {
            frontTile = groundTile1;
        }
    }

    //Main Menu stuff
    //Controlling the player prefs and things to open and close main menu UI elements.
    public void OnClickStart()
    {
        PlayerPrefs.Save();

        SceneManager.LoadScene("Main Game");
        print("The Start button was clicked");
    }
    public void OnClickHelp()
    {
        helpPanel.SetActive(true);
        leaderboardPanel.SetActive(false);
        print("The Help button was clicked");
    }
    public void OnHelpClose()
    {
        helpPanel.SetActive(false);
        print("The Help (close) button was clicked");
    }
    public void OnCSelectOpen()
    {
        cSelect.SetActive(true);
    }
    public void OnCSelectClose()
    {
        cSelect.SetActive(false);
    }
    public void OnGlobalOpen()
    {
        leaderboardPanel.SetActive(true);
        helpPanel.SetActive(false);
    }
    public void OnGlobalClose()
    {
        leaderboardPanel.SetActive(false);
    }
    public void OnClickQuit()
    {
        Application.Quit();
        print("The Quit button was clicked");
    }
    public void OnInputEdit()
    {
        PlayerPrefs.SetString("PlayerName", name.text);
        if (name.text == "")
        {
            playerNameText.text = "";
        }
        playerNameText.text = "Welcome " + name.text + "!";
        print("Player's name is " + PlayerPrefs.GetString("PlayerName"));
        
    }
    public void OnClickClearPrefs()
    {
        PlayerPrefs.DeleteAll();
        highscore.text = PlayerPrefs.GetString("PlayerName").ToString() + ": " + PlayerPrefs.GetInt("Highscore").ToString();
        PlayerPrefs.DeleteKey("unlocks"); //delete character save data when DEL cheat code is used
        Debug.Log("Characters and highscore deleted.");
    }
}

