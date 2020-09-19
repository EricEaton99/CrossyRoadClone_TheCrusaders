using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_groundTile : MonoBehaviour
{
    public GameObject tile_grass;
    public GameObject tile_bush;
    public GameObject tile_road;
    public GameObject tile_water;
    public GameObject tile_path;
    public GameObject tile_goal;

    [SerializeField] GameObject log;
    [SerializeField] GameObject car;

    GameObject[,] tileGrid = new GameObject[10, 10];
    List<GameObject> objBacklog = new List<GameObject>();
    int path = 5;

    private void Start()
    {
        Shuffle();


    }

    public void ClearArray()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Destroy(tileGrid[i, j]);
            }
        }
        StopAllCoroutines();
        for(int i = 0; i < objBacklog.Count; i++)
        {
            Destroy(objBacklog[i]);
        }
    }

    public void Shuffle()
    {
        Instantiate(tile_path, new Vector3(transform.position.x, 0, -1), Quaternion.identity);
        Debug.LogError("NewTile");
        int goal = 5;
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("path is " + path);
            switch (Random.Range(0, 3))
            {
                case 0:                                     //can create walls
                    
                    if (path < 0)       //Ensures that there is always a path forward
                    {
                        path = Random.Range(5, 10);
                        goal = -1;
                    }
                    else if (path < 3)
                    {
                        goal = path + Random.Range(0, 3);
                    }
                    else if (path < 7)
                    {
                        goal = path + Random.Range(-3, 3);
                    }
                    else if (path < 10)
                    {
                        goal = path + Random.Range(-3, 0);
                    }
                    else if (path == 10)
                    {
                        path = Random.Range(0, 5);
                        goal = -1;
                    }
                    else
                    {
                        Debug.Log("int: Path, is out of range");
                    }

                    Debug.Log("goal is " + goal);


                    for (int j = 0; j < 10; j++)
                    {
                        //Debug.Log(i + ", " + j);
                        if(((j > path && j < goal) || (j < path && j > goal)) && !(goal < 0))
                        {
                            tileGrid[i, j] = Instantiate(tile_path, new Vector3(i + transform.position.x, 0, j), Quaternion.identity);
                        }
                        else if (j == path)
                        {
                            tileGrid[i, j] = Instantiate(tile_path, new Vector3(i + transform.position.x, 0, j), Quaternion.identity);
                        }
                        else if (j == goal)
                        {
                            tileGrid[i, j] = Instantiate(tile_goal, new Vector3(i + transform.position.x, 0, j), Quaternion.identity);
                        }
                        else if (Random.Range(0, 2) == 0)
                        {
                            tileGrid[i, j] = Instantiate(tile_grass, new Vector3(i + transform.position.x, 0, j), Quaternion.identity);
                        }
                        else
                        {
                            tileGrid[i, j] = Instantiate(tile_bush, new Vector3(i + transform.position.x, 0, j), Quaternion.identity);
                        }
                    }
                    break;
                case 1:
                    for (int j = 0; j < 10; j++)
                    {
                        //Debug.Log(i + ", " + j);
                        tileGrid[i, j] = Instantiate(tile_water, new Vector3(i + transform.position.x, -0.05f, j), Quaternion.identity);
                    }
                    if (Random.Range(0, 2) == 0)
                    {
                        StartCoroutine(WaitAndSpawn(log, i + transform.position.x, Random.Range(1, 2.5f)));
                        path = -1;
                    }
                    else
                    {
                        StartCoroutine(WaitAndSpawn(log, i + transform.position.x, Random.Range(-2.5f, -1)));
                        path = 10;
                    }
                    break;
                case 2:
                    for (int j = 0; j < 10; j++)
                    {
                        //Debug.Log(i + ", " + j);
                        tileGrid[i, j] = Instantiate(tile_road, new Vector3(i + transform.position.x, -0.025f, j), Quaternion.identity);
                    }

                    if (Random.Range(0, 2) == 0)
                    {
                        StartCoroutine(WaitAndSpawn(car, i + transform.position.x, Random.Range(1, 2.5f)));
                        path = -1;
                    }
                    else
                    {
                        StartCoroutine(WaitAndSpawn(car, i + transform.position.x, Random.Range(-2.5f, -1)));
                        path = 10;
                    }
                    break;
                default:
                    Debug.Log("Tile index does not exist.");
                    break;
            }
            path = goal;        //set path to goal for next row
        }
    }


    IEnumerator WaitAndSpawn(GameObject objToSpawn, float location, float speed)
    {
        yield return new WaitForSeconds(2.2f / Mathf.Abs(speed) * Random.Range(1, 4));
        GameObject tempCar;
        if (objToSpawn == car)
        {
            tempCar = Instantiate(objToSpawn, new Vector3(location, 0.85f, 12 - 14 * Mathf.Clamp01(speed)), Quaternion.Euler(-90, 180 - 180 * Mathf.Clamp01(speed), 0));      //0.8 = y
        }
        else
        {
            tempCar = Instantiate(objToSpawn, new Vector3(location, 0.6f, 12 - 14 * Mathf.Clamp01(speed)), Quaternion.Euler(-90, 180 - 180 * Mathf.Clamp01(speed), 0));      //0.6 = y
        }

        tempCar.GetComponent<Rigidbody>().velocity = Vector3.forward * speed;
        StartCoroutine(WaitAndDestroy(tempCar, 14 / Mathf.Abs(speed)));
        StartCoroutine(WaitAndSpawn(objToSpawn, location, speed));
    }

    IEnumerator WaitAndDestroy(GameObject objToDestroy, float waitTime)
    {
        objBacklog.Add(objToDestroy);
        yield return new WaitForSeconds(waitTime);
        objBacklog.Remove(objToDestroy);
        Destroy(objToDestroy);
    }

}
