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
    


    private void Start()
    {
        frontTile = groundTile1;
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
        SceneManager.LoadScene("Main Game");
    }
    public void OnClickHelp()
    {
        helpPanel.SetActive(true);
    }
    public void OnHelpClose()
    {
        helpPanel.SetActive(false);
    }
}

