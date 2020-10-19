using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject thePlayer;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 5f);
    }

    void Update()
    { 
        if (thePlayer.transform.position.z <= 3f)
        {
            //Debug.LogError(thePlayer.transform.position.z);
            transform.position = new Vector3(thePlayer.transform.position.x + 1, transform.position.y, 3f);
        }else if (thePlayer.transform.position.z >= 6f)
        {
            //Debug.LogError(thePlayer.transform.position.z);
            transform.position = new Vector3(thePlayer.transform.position.x + 1, transform.position.y, 6f);
        }
        else
        {
            transform.position = new Vector3(thePlayer.transform.position.x + 1, transform.position.y, thePlayer.transform.position.z);
        }
    }
}
