using System.Collections;
using System.Collections.Generic;
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
    }
    public void OnEQuit()
    {
        eMenu.SetActive(false);
    }

}
