using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridMovement : MonoBehaviour
{
    [Header("Scoring Stuff")]
    public int score = 0;
    public int highScore = 0;
    public Text currentScore;
    public Text currentHighScore;
    [Header("Movement Stuff")]
    bool isMoving;
    public GameObject targetPos;


    private void Start()
    {
        highScore = PlayerPrefs.GetInt("Highscore");
        isMoving = false;
    }

    void Update()
    {//Scoring stuff, tracking the score and highscore.
        if(score>=highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("Highscore", highScore);
            PlayerPrefs.Save();
        }
        
        currentScore.text = "Score: " + score.ToString();
        currentHighScore.text = "Highscore: " + highScore.ToString();

        if (!isMoving) //allow movement when player is stationary
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


    }

    void TurnAndMove(int direction)
    {
        isMoving = true;

        switch (direction) //0 is forward, incrementing clockwise
        {
            case 0:
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                break;
            case 1:
                transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                score++;
                break;
            case 2:
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                break;
            case 3:
                transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
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
        if (PathCheck()) //if there is not a bush in the way
        {
            TurnAndMove(direction); //go to destination position
        }
        else
        {
            targetPos.transform.position = new Vector3(targetPos.transform.position.x, .6f, targetPos.transform.position.z); //reset destination position
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Log"))
        {
            transform.parent = other.transform; //set the player as a child, so the player moves with the parent
            targetPos.transform.parent = other.transform;
        }
        else if (other.gameObject.CompareTag("Car"))
        {
            Debug.Log("Death by traffic");
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            if (transform.root == transform)
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Log"))
        {
            transform.parent = null;
            targetPos.transform.parent = null;

            targetPos.transform.position = new Vector3(Mathf.Round(targetPos.transform.position.x),
                targetPos.transform.position.y, Mathf.Round(targetPos.transform.position.z));
            //NEEDS WORK-- When going from water to land, estimate landing location
        }
    }
}
