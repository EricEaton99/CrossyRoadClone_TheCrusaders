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

    bool inWater;
    bool isInvincible;

    int minVerticalValue;
    public GameObject endCube;

    GameManager gManager;

    bool onLog;

    private void Start()
    {
        Time.timeScale = 1;

        inWater = false;
        isMoving = false;
        onLog = false;

        minVerticalValue = -2;

        //setting up highscore prefs
        highScore = PlayerPrefs.GetInt("Highscore");

        gManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) //developer key
        {
            if (!isInvincible)
            {
                isInvincible = true;
            }
            else
            {
                isInvincible = false;
            }
            Debug.Log("isInvincible = " + isInvincible);
        }

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
            if (Mathf.Abs(targetPos.transform.position.x - transform.position.x) < .1 && Mathf.Abs(targetPos.transform.position.z - transform.position.z) < .1)
            {
                transform.position = new Vector3(targetPos.transform.position.x, 1.5f, targetPos.transform.position.z);
                isMoving = false;
            }
            else //move until the object is close to the point, then snap it to the point
            {

                transform.position = Vector3.Lerp(transform.position, new Vector3(targetPos.transform.position.x, 1.5f, targetPos.transform.position.z), .35f);
                if (targetPos.transform.position.z != Mathf.RoundToInt(targetPos.transform.position.z) && !onLog)
                {
                    targetPos.transform.position = new Vector3(targetPos.transform.position.x,
                        targetPos.transform.position.y, Mathf.RoundToInt(targetPos.transform.position.z));
                }
            }
        }

        currentScore.text = "Score: " + score.ToString();
        currentHighScore.text = "Highscore: " + highScore.ToString();
        //Scoring stuff, tracking the score and highscore.

        if (transform.position.z >= 10 || transform.position.z <= -1) //when the player is on out-of-bounds logs
        {
            Die();
        }
    }

    void TurnAndMove(int direction)
    {
        isMoving = true;

        switch (direction)
        {
            case 0:
                transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
                break;
            case 1:
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                score++;
                break;
            case 2:
                transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                break;
            case 3:
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                score--;
                break;

        }
    }

    bool PathCheck() //if going left, what's the tile to the left?
    {
        bool canMove = false;
        bool bushIsThere = false;

        if (targetPos.transform.position.z >= 10 || targetPos.transform.position.z <= -1) //don't go outside the boundaries
        {
            Debug.Log("Error... Horizontal game board is not there...");
        }
        else if (targetPos.transform.position.x <= minVerticalValue)
        {
            Debug.Log("Error... Vertical game board is not there...");
        }
        else
        {
            if (targetPos.transform.position.x != transform.position.x) //check only if going forward or backward
            {
                targetPos.transform.position = new Vector3(targetPos.transform.position.x,
                targetPos.transform.position.y, Mathf.RoundToInt(targetPos.transform.position.z));
            }

            Collider[] tiles = Physics.OverlapSphere(targetPos.transform.position, .5f);

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
            onLog = true;
        }
        else if (other.gameObject.CompareTag("Car"))
        {
            if (!isInvincible)
            {
                Debug.Log("Death by traffic");
                Die();
            }
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            if (!isInvincible)
            {
                inWater = true;

                if (transform.root == transform) //if player is not on a log
                {
                    Invoke("Die", .05f);
                }
            }
        }
        else if (other.gameObject.CompareTag("Exit")) //ending platform
        {
            ActivateFlip();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Log"))
        {
            transform.parent = other.transform;
            targetPos.transform.parent = other.transform; //jump from one log to another, shift direction as needed
            CancelInvoke();
            inWater = false;
            onLog = true;
        }
        if (other.gameObject.CompareTag("Water"))
        {
            Invoke("Die", .05f); //since player collides with water while on a log, this code must be in Stay
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Log"))
        {
            transform.parent = null;
            targetPos.transform.parent = null;
            onLog = false;
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            onLog = false;
            inWater = false;
            CancelInvoke();
        }
    }

    void ResetTarget()
    {
        targetPos.transform.position = new Vector3(targetPos.transform.position.x, .6f, targetPos.transform.position.z); //reset destination position
    }

    void Die()
    {
        if (score >= highScore) //update high score after death
        {
            highScore = score;
            PlayerPrefs.SetInt("Highscore", highScore);
            PlayerPrefs.Save();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("Main Menu");
    }

    void ResetPositions()
    {
        endCube.transform.position = new Vector3((endCube.transform.position.x + 10), endCube.transform.position.y, endCube.transform.position.z);
    }

    void ActivateFlip()
    {
        Debug.Log("Flipping engage!");
        gManager.OnFlipButtonClick();
        minVerticalValue += 10;
        ResetPositions();
    }

}
