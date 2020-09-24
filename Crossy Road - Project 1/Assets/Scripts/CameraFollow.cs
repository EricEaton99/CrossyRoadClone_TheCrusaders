using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject thePlayer;

    void Update()
    {
        transform.position = thePlayer.transform.position;
    }
}
