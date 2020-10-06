using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject thePlayer;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 6f);
    }

    void Update()
    {
        if (4.5f < thePlayer.transform.position.z && thePlayer.transform.position.z < 6.5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, thePlayer.transform.position.z - 1);
            print("move cam");
        }
        transform.position = new Vector3(thePlayer.transform.position.x +1, transform.position.y, transform.position.z);
    }
}
