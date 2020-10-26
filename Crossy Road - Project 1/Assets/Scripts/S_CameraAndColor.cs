using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CameraAndColor : MonoBehaviour
{
    public GameObject thePlayer;
    GameObject characterHolder;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 5f);

        characterHolder = thePlayer.transform.GetChild(0).GetChild(0).gameObject; //get thePlayer's grandchild, the character model; works with any character

        GameObject character = characterHolder.transform.GetChild(PlayerPrefs.GetInt("currentCharacter")).gameObject;
        character.SetActive(true);
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
