using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class S_ESCMenu : MonoBehaviour
{
    public GameObject eMenu;

    public bool toggle;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown("escape"))
            toggle = !toggle;

        if (toggle)
            OnEPress();
        else
            OnEQuit();
    }

    public void OnEPress()
    {
        eMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }
    public void OnEQuit()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        eMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnClickMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

}
