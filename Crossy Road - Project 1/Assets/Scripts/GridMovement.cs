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

    bool isInvincible;

    int minVerticalValue;
    public GameObject endCube;

    GameManager gManager;

    int[] scoreArray = new int[12];

    public GameObject[] highlight; //highlight[0] is front, highlight[1] is back

    float highlightZ;

    bool onLog;

    private void Start()
    {
        Time.timeScale = 1;

        isMoving = false;

        minVerticalValue = -1;

        //setting up highscore prefs
        highScore = PlayerPrefs.GetInt("Highscore");

        gManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        SetUpScoreArray();

        PlayerPrefs.GetInt("unlocks");
        Debug.Log("Next character: " + scoreArray[PlayerPrefs.GetInt("unlocks") + 1]);

        highlightZ = transform.position.z;
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
            if (Mathf.Abs(targetPos.transform.position.x - transform.position.x) < .1 && Mathf.Abs(targetPos.transform.position.z - transform.position.z) < .1)
            {
                transform.position = new Vector3(targetPos.transform.position.x, 1.5f, targetPos.transform.position.z);
                isMoving = false;
            }
            else //move until the object is close to the point, then snap it to the point
            {

                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(targetPos.transform.position.x, 1.5f, targetPos.transform.position.z), .35f);
            }
        }

        currentScore.text = "Score: " + score.ToString();
        currentHighScore.text = "Highscore: " + highScore.ToString();
        //Scoring stuff, tracking the score and highscore.

        if (transform.position.z >= 10 || transform.position.z <= -1) //when the player is on out-of-bounds logs
        {
            Die();
        }

        for (int x = 0; x < scoreArray.Length; x++)
        {
            if (score >= scoreArray[x])
            {
                if (x > PlayerPrefs.GetInt("unlocks")) //only unlock if it's a locked character
                {
                    UnlockACharacter(x);
                    Debug.Log("Unlocked a character at " + scoreArray[x]);
                }
            }
        }

        //for (int x = 0; x < highlight.Length; x++)
        //{
        //    Collider[] highlightTiles = Physics.OverlapSphere(highlight[x].transform.position, .4f);

        //    //print(highlightTiles.Length);

        //    bool tileIsEnterable = false;

        //    for (int y = 0; y < highlightTiles.Length; y++)
        //    {

        //        if ( (highlightTiles[y].name.Contains("Grass") || highlightTiles[y].name.Contains("Road") || highlightTiles[y].name.Contains("Tracks") ) && transform.parent) //if the highlight tile is on grass
        //        {
        //            highlightZ = Mathf.RoundToInt(transform.position.z);
        //            //doRound = true;
        //            highlight[x].SetActive(true);
        //            //tileIsEnterable = true;                                                                       //Highlights turned off
        //            break;
        //        }
        //    }


        //    if(!tileIsEnterable)
        //    {
        //        highlightZ = transform.position.z;
        //        highlight[x].SetActive(false);
        //    }
        //}

        if (transform.parent)
        {
            switch (SetHighlights())
            {
                case 0:
                    highlight[0].SetActive(true);
                    highlight[1].SetActive(true);
                    break;
                case 1:
                    highlight[0].SetActive(false);
                    highlight[1].SetActive(true);
                    break;
                case 2:
                    highlight[0].SetActive(true);
                    highlight[1].SetActive(false);
                    break;
                case 3:
                    highlight[0].SetActive(false);
                    highlight[1].SetActive(false);
                    break;
                default:
                    print("out of range ");
                    break;
            }
            highlight[0].transform.position = new Vector3(transform.position.x + 1, .75f, Mathf.RoundToInt(transform.position.z));
            highlight[1].transform.position = new Vector3(transform.position.x - 1, .75f, Mathf.RoundToInt(transform.position.z));
        }
        else
        {
            highlight[0].SetActive(false);
            highlight[1].SetActive(false);
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

    bool PathCheck(int direction) //if going left, what's the tile to the left?
    {
        bool canMove = false;
        bool bushIsThere = false;

        //Player is moving in bounds of board
        if (targetPos.transform.position.z >= 10 || targetPos.transform.position.z <= -1) //don't go outside the boundaries
        {
            Debug.Log("Error... Horizontal game board is not there...");
        }
        else if (targetPos.transform.position.x <= minVerticalValue)
        {
            Debug.Log("Error... Vertical game board is not there...");
        }
        //Player is not moving into bush
        else
        {
            SetTarget(direction);        //Update future position}

            Collider[] tiles = Physics.OverlapSphere(targetPos.transform.position, .4f);
            print("sphere 0.4f at: " + targetPos.transform.position + " found: " + tiles.Length + " objects");

            for (int x = 0; x < tiles.Length; x++)
            {
                print("loop pass: " + x);
                if (tiles[x].gameObject.CompareTag("Bush"))
                {
                    print("found bush");
                    bushIsThere = true; //move the player unless there's a bush there
                }
            }

            if (!bushIsThere)
            {
                print("can move");
                canMove = true;
            }
            else
            {
                print("cannot move");

            }
        }
        return canMove;
    }

    void SetTarget(int direction)        //Sets movement destination
    {
        bool isForward = false;
        //bool isLog = false;
        bool toLog = false;
        Collider[] moveToObjList;
        Collider logToEnter = null;

        if (direction == 0)
        {
            targetPos.transform.position = new Vector3(transform.position.x, 0.75f, transform.position.z + 1);      //set targetPos no snap left
            return;
        }
        else if (direction == 2)
        {
            //targetPos.transform.position = transform.position + Vector3.back;
            targetPos.transform.position = new Vector3(transform.position.x, 0.75f, transform.position.z - 1);      //set targetPos no snap right
            return;
        }
        else if (direction == 1)          //if going FORWARD
        {
            isForward = true;
        }
        if (isForward)
        {
            moveToObjList = Physics.OverlapSphere(transform.position + Vector3.right, .4f);
        }
        else
        {
            moveToObjList = Physics.OverlapSphere(transform.position + Vector3.left, .4f);
            
        }
        for (int i = 0; i < moveToObjList.Length; i++)
        {
            if (moveToObjList[i].transform.CompareTag("Log"))       // || moveToObjList[i].transform.CompareTag("Water")
            {
                toLog = true;
                logToEnter = moveToObjList[i];
            }
        }

        onLog = toLog;
        if (toLog)
        {
            transform.parent = logToEnter.gameObject.transform;        //set the player as a child, so the player moves with the parent
            if (isForward)
            {
                targetPos.transform.position = transform.position + Vector3.right;      //set targetPos no snap forward
            }
            else
            {
                targetPos.transform.position = transform.position + Vector3.left;      //set targetPos no snap back
            }
        }
        else
        {
            transform.parent = null;
            if (isForward)
            {
                targetPos.transform.position = new Vector3(transform.position.x + 1, 0.75f, Mathf.RoundToInt(transform.position.z));      //set targetPos with snap forward
            }
            else
            {
                targetPos.transform.position = new Vector3(transform.position.x - 1, 0.75f, Mathf.RoundToInt(transform.position.z));      //set targetPos with snap back
            }
        }
    }

    void CheckTileAndStartMoving(int direction)
    {
        if (PathCheck(direction)) //if bool "canMove" == true
        {
            //Collider[] grassTiles = Physics.OverlapSphere(targetPos.transform.position, .4f);

            //for (int x = 0; x < grassTiles.Length; x++)
            //{
            //    if ((grassTiles[x].name.Contains("Grass") || grassTiles[x].name.Contains("Road")) && transform.parent != null) //if on a log and going to grass
            //    {
            //        transform.parent = null;
            //        targetPos.transform.parent = null; //unparent before moving

            //        //Invoke("RoundTargetPosition", .05f); //log will mess up rounded position, so reset it
            //    }
            //}

            TurnAndMove(direction); //go to destination position

        }
        else
        {
            ResetTarget();
        }
    }

    int SetHighlights()        //Sets movement destination
    {
        bool isForward = false;
        bool toLogForward = false;
        bool toLogBackward = false;
        Collider logToEnter = null;
        
        Collider[] forwardObjList = Physics.OverlapSphere(transform.position + Vector3.right, .4f);
        Collider[] backwardObjList = Physics.OverlapSphere(transform.position + Vector3.left, .4f);

        for (int i = 0; i < forwardObjList.Length; i++)
        {
            if (forwardObjList[i].transform.CompareTag("Log") || forwardObjList[i].transform.CompareTag("Water") || forwardObjList[i].transform.CompareTag("Bush"))       // || moveToObjList[i].transform.CompareTag("Water")
            {
                toLogForward = true;
            }
        }
        for (int i = 0; i < backwardObjList.Length; i++)
        {
            if (backwardObjList[i].transform.CompareTag("Log") || backwardObjList[i].transform.CompareTag("Water") || backwardObjList[i].transform.CompareTag("Bush"))       // || moveToObjList[i].transform.CompareTag("Water")
            {
                toLogBackward = true;
            }
        }

        if(!toLogForward && !toLogBackward)
        {
            return 0;
        }else if (toLogForward && !toLogBackward)
        {
            return 1;
        }
        else if (!toLogForward && toLogBackward)
        {
            return 2;
        }
        else{
            return 3;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Exit")) //ending platform
        {
            ActivateFlip();
        }
        else if (other.gameObject.name.Contains("Grass"))
        {
            RoundTargetPosition();
        }

        if (other.gameObject.CompareTag("Log"))
        {
            transform.parent = other.transform; //set the player as a child, so the player moves with the parent
            targetPos.transform.parent = other.transform;
            CancelInvoke();
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            Invoke("Die", .1f);
        }

        if (other.gameObject.CompareTag("Car"))
        {
            Debug.Log("Death by traffic");
            Die();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Log")) //for jumping between logs/trains
        {
            transform.parent = other.transform;
            targetPos.transform.parent = other.transform;
            CancelInvoke();
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            Invoke("Die", .1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Log"))
        {
            transform.parent = null;
            targetPos.transform.parent = null;
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            CancelInvoke();
        }
    }

    void ResetTarget()
    {
        targetPos.transform.position = new Vector3(targetPos.transform.position.x, .6f, targetPos.transform.position.z); //reset destination position
    }

    void Die()
    {
        if (!isInvincible)
        {
            if (score >= highScore) //update high score after death
            {
                highScore = score;
                PlayerPrefs.SetInt("Highscore", highScore);
                PlayerPrefs.SetString("HighPlayerName", PlayerPrefs.GetString("PlayerName"));
                PlayerPrefs.Save();
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SceneManager.LoadScene("Main Menu");
        }
    }

    void ResetPositions()
    {
        endCube.transform.position = new Vector3((endCube.transform.position.x + 10), endCube.transform.position.y, endCube.transform.position.z);
    }

    void ActivateFlip()
    {
        Debug.Log("Flipping...");
        gManager.OnFlipButtonClick();
        minVerticalValue += 10;
        ResetPositions();
    }

    void SetUpScoreArray()
    {
        int unlockScore = 10;
        for (int x = 1; x < scoreArray.Length; x++)
        {
            if (x < 4) //0, 20, 30, (40)
            {
                unlockScore += 10;
            }
            else if (x < 8) //55, 70, 85, (100)
            {
                unlockScore += 15;
            }
            else //125, 150, 175, (200)
            {
                unlockScore += 25;
            }
            scoreArray[x] = unlockScore;
        }
        Debug.Log(scoreArray[11]);
    }

    void UnlockACharacter(int index)
    {
        PlayerPrefs.SetInt("unlocks", index);
    }

    void RoundTargetPosition()
    {
        targetPos.transform.position = new Vector3(targetPos.transform.position.x, .75f, Mathf.RoundToInt(targetPos.transform.position.z));
    }
}
