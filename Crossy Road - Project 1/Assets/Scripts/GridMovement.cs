using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridMovement : MonoBehaviour
{
    [Header("Scoring Stuff")]
    public int score = 0;
    public int highScore = 0;
    public Text currentScore;
    [Header("Moving Stuff")]
    bool isMoving;
    public GameObject targetPos;

    bool ridingLog;

    private void Start()
    {
        //setting up highscore prefs
        PlayerPrefs.SetInt("Highscore", highScore);

        ridingLog = false;
        isMoving = false;
    }

    void Update()
    {
        //Scoring stuff (updating the score script)
        currentScore.text = "Score: " + score.ToString();

        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                targetPos.transform.position = new Vector3(transform.position.x + 1, targetPos.transform.position.y, transform.position.z);
                CheckTileAndStartMoving(1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                targetPos.transform.position = new Vector3(transform.position.x - 1, targetPos.transform.position.y, transform.position.z);
                CheckTileAndStartMoving(3);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                targetPos.transform.position = new Vector3(transform.position.x, targetPos.transform.position.y, transform.position.z + 1);
                CheckTileAndStartMoving(0);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                targetPos.transform.position = new Vector3(transform.position.x, targetPos.transform.position.y, transform.position.z - 1);
                CheckTileAndStartMoving(2);
            }
        }
        else
        {
            if (Mathf.Abs(targetPos.transform.position.x - transform.position.x) <.1 && Mathf.Abs(targetPos.transform.position.z - transform.position.z) < .1)
            {
                transform.position = new Vector3(targetPos.transform.position.x, 1.5f, targetPos.transform.position.z);
                isMoving = false;
            }
            else //move until the object is close to the point, then snap it to the point
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(targetPos.transform.position.x, 1.5f, targetPos.transform.position.z), .3f);
            }
        }

        if (ridingLog)
        {
            
        }
    }

    void TurnAndMove(int direction)
    {
        isMoving = true;

        switch (direction) //0 is forward, incrementing clockwise
        {
            case 0:
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up); //face forwards
                break;
            case 1:
                transform.rotation = Quaternion.AngleAxis(90, Vector3.up); //face right
                score++;
                break;
            case 2:
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up); //face backwards
                break;
            case 3:
                transform.rotation = Quaternion.AngleAxis(-90, Vector3.up); //face left
                score--;
                break;
                
        }
    }

    bool PathCheck() //if going left, what's the tile to the left?
    {
        bool canMove = false;
        bool bushIsThere = false;

        Collider[] tiles = Physics.OverlapSphere(targetPos.transform.position, .5f); //build a collision sphere with radius .5f at targetPos

        for (int x = 0; x < tiles.Length; x++)
        {
            if (tiles[x].gameObject.CompareTag("Bush"))
            {
                bushIsThere = true; //move the player unless there's a bush there
            }
        }
        if (!bushIsThere)
        {
            canMove = true;
        }
        return canMove;
    }

    void CheckTileAndStartMoving(int direction)
    {
        if (PathCheck()) //if bool "canMove" == true
        {
            
            TurnAndMove(direction); //go to destination position
        }
        else
        {
            targetPos.transform.position = new Vector3(targetPos.transform.position.x, .6f, targetPos.transform.position.z); //reset destination position
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Log"))
        {

        }
    }
}
