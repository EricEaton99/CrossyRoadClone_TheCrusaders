using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridMovement : MonoBehaviour
{
    [Header("Scoring Stuff")]
    public int score = 0;
    public int highScore;
    public Text currentScore;
    public Text currentHighScore;
    [Header("Moving Stuff")]
    bool isMoving;
    public GameObject targetPos;
    bool isRidingLog;

    

    private void Start()
    {
        isMoving = false;
        //setting up highscore prefs
        
        highScore = PlayerPrefs.GetInt("Highscore");
        isRidingLog = false;
    }

    void Update()
    {
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
        currentScore.text = "Score: " + score.ToString();
        currentHighScore.text = "Highscore: " + highScore.ToString();
        //Scoring stuff, tracking the score and highscore.
        if (score >= highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("Highscore", highScore);
            PlayerPrefs.Save();
            

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
        Collider[] tiles;

        if (targetPos.transform.position.z >= 10 || targetPos.transform.position.z <= -1) //don't go outside the boundaries
        {
            Debug.Log("Error... Game board is not there...");
        }
        else
        {
            if (!isRidingLog)
            {
                tiles = Physics.OverlapSphere(targetPos.transform.position, .5f); //build a collision sphere with radius .5f at targetPos
            }
            else
            {
                ExitLogBushCheck(); //Round the position to a whole number BEFORE scanning for trees
                tiles = Physics.OverlapSphere(targetPos.transform.position, .5f);
            }

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
            ResetTarget();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Log"))
        {
            transform.parent = other.transform; //set the player as a child, so the player moves with the parent
            targetPos.transform.parent = other.transform;
            isRidingLog = true;
        }
        else if (other.gameObject.CompareTag("Car"))
        {
            Debug.Log("Death by traffic");
            //SceneManager.LoadScene("Main Menu");
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            if (transform.root == transform) //if object does not have a parent, IE parented to the log
            {
                Debug.Log("Death by hydration");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Log"))
        {
            transform.parent = other.transform;
            targetPos.transform.parent = other.transform; //jump from one log to another, shift direction as needed
            isRidingLog = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Log"))
        {
            transform.parent = null;
            targetPos.transform.parent = null;

            isRidingLog = false;
        }
    }

    void ResetTarget()
    {
        targetPos.transform.position = new Vector3(targetPos.transform.position.x, .6f, targetPos.transform.position.z); //reset destination position
    }

    void ExitLogBushCheck()
    {
        targetPos.transform.position = new Vector3(Mathf.Round(targetPos.transform.position.x),
                targetPos.transform.position.y, Mathf.Round(targetPos.transform.position.z));
    }
}
