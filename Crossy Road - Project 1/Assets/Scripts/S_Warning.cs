using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Warning : MonoBehaviour
{
    public GameObject player;
    GridMovement gridMove;
    bool warningActive; //Update() will repeatedly call the FlashOn() if a bool is not present

    public GameObject warningCube;
    void Start()
    {
        warningActive = false;
        gridMove = player.GetComponent<GridMovement>();
    }

    void Update()
    {
        if (player.transform.root != player.transform) //if on a log
        {
            if (player.transform.position.z < 2.5f) //keep track of Z only when player is on log
            {
                UpdateDangerZone(-.25f);
                if (!warningActive)
                {
                    FlashOn();
                }
            }
            else if (player.transform.position.z > 6.5f)
            {
                UpdateDangerZone(9.25f);
                if (!warningActive)
                {
                    FlashOn();
                }
            }
            else
            {
                DisableWarning();
            }
        }
        else
        {
            DisableWarning();
        }
    }

    void FlashOn()
    {
        warningActive = true;
        warningCube.SetActive(true);
        Invoke("FlashOff", .25f); //flash on twice every second
    }

    void FlashOff()
    {
        warningCube.SetActive(false);
        Invoke("FlashOn", .25f);
    }

    void UpdateDangerZone(float sideZ)
    {
        warningCube.transform.position = new Vector3(player.transform.position.x, 1, sideZ);
    }

    void DisableWarning()
    {
        CancelInvoke("FlashOff");
        CancelInvoke("FlashOn");

        warningActive = false;
        if (warningCube.activeSelf)
        {
            warningCube.SetActive(false);
        }
    }
}
