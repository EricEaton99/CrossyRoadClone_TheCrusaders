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
    


    private void Start()
    {
        frontTile = groundTile1;
        name.text = PlayerPrefs.GetString("PlayerName");
        highscore.text = PlayerPrefs.GetString("PlayerName") + PlayerPrefs.GetInt("Highscore").ToString();
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
    public void OnClickStart()
    {
        
        PlayerPrefs.SetString("Name", name.text);

        SceneManager.LoadScene("Main Game");
        print("The Start button was clicked");
    }
    public void OnClickHelp()
    {
        helpPanel.SetActive(true);
        print("The Help button was clicked");
    }
    public void OnHelpClose()
    {
        helpPanel.SetActive(false);
        print("The Help (close) button was clicked");
    }
    public void OnClickQuit()
    {
        Application.Quit();
        print("The Quit button was clicked");
    }
    public void OnInputEdit()
    {
        if(name.text == "")
        {
            playerNameText.text = "";
        }
        playerNameText.text = "Welcome " + name.text + "!";
        print("Player's name is " + PlayerPrefs.GetString("PlayerName"));
        PlayerPrefs.Save();
    }
}

