using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CameraAndColor : MonoBehaviour
{
    public GameObject thePlayer;
    GameObject character;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 5f);

        character = thePlayer.transform.GetChild(0).GetChild(0).gameObject; //get thePlayer's grandchild, the character model; works with any character

        Renderer[] rend = character.transform.GetComponentsInChildren<Renderer>(); //get ALL renderers in children objects

        Color newColor = Color.white;
        int color = Random.Range(0, 7); //0 to 6
        switch (color)
        {
            case 0:
                newColor = Color.red;
                break;
            case 1:
                newColor = Color.yellow;
                break;
            case 2:
                newColor = Color.green;
                break;
            case 3:
                newColor = Color.cyan;
                break;
            case 4:
                newColor = Color.blue;
                break;
            case 5:
                newColor = Color.magenta;
                break;
            case 6:
                newColor = Color.black;
                break;
        }
        for (int x = 0; x < rend.Length; x++)
        {
            rend[x].material.color = newColor;
        }
    }

    void Update()
    {
        if (thePlayer.transform.position.z <= 3f)
        {
            //Debug.LogError(thePlayer.transform.position.z);
            transform.position = new Vector3(thePlayer.transform.position.x + 1, transform.position.y, 3f);
        }
        else if (thePlayer.transform.position.z >= 6f)
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
