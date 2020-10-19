using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_groundTile : MonoBehaviour
{
    public GameObject[] tilesetSummer = new GameObject[4];
    public GameObject[] tilesetFall = new GameObject[4];
    public GameObject[] tilesetWinter = new GameObject[4];
    public GameObject tile_path;
    public GameObject tile_goal;
    public AudioClip carSound;

    [SerializeField] GameObject log;
    [SerializeField] GameObject car;
    [SerializeField] GameObject trainEngine;
    [SerializeField] GameObject trainCar;
    [SerializeField] GameObject trainFlat;

    GameObject[,] seasonTileset = new GameObject[3, 4];
    GameObject[,] tileGrid = new GameObject[10, 10];
    List<GameObject> objBacklog = new List<GameObject>();
    int path = 5;

    void Start()
    {
        for (int j = 0; j < 4; j++)
        {
            //print("tilesetSummer[j].name = " + tilesetSummer[j].name);
            seasonTileset[0, j] = tilesetSummer[j];
        }
        for (int j = 0; j < 4; j++)
        {
            //print("tilesetFall[j].name = " + tilesetFall[j].name);
            seasonTileset[1, j] = tilesetFall[j];
        }
        for (int j = 0; j < 4; j++)
        {
            //print("tilesetWinter[j].name = " + tilesetWinter[j].name);
            seasonTileset[2, j] = tilesetWinter[j];
        }


        //Shuffle();
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
        //Debug.LogError("NewTile");
        int goal = 5;
        int season = Random.Range(0, 3);
        //print("Season = " + season);
        for (int i = 0; i < 10; i++)
        {
            //Debug.Log("path is " + path);
            switch (Random.Range(0, 4))
            {
                case 0:
                    goal = FindNewPath(goal);
                    //Debug.Log("goal is " + goal);

                    GenerateField(goal, i, season);
                    break;
                case 1:
                    GenerateWater(i, season);
                    break;
                case 2:
                    GenerateRoad(i, season);
                    break;
                case 3:
                    GenerateTracks(i, season);
                    break;
                default:
                    //Debug.Log("Tile index does not exist.");
                    break;
            }
            path = goal;        //set path to goal for next row
        }
    }


    int FindNewPath(int goal)
    {
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
            //Debug.Log("int: Path, is out of range");
        }
        return (goal);
    }

    void GenerateField(int goal, int i, int season)
    {
        for (int j = 0; j < 10; j++)
        {
            //Debug.Log(i + ", " + j);
            if (((j > path && j < goal) || (j < path && j > goal)) && !(goal < 0))
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
                //print("seasonTileset[season, 0].name = " + seasonTileset[season, 0].name);
                tileGrid[i, j] = Instantiate(seasonTileset[season, 0], new Vector3(i + transform.position.x, 0, j), Quaternion.identity);
            }
            else
            {
                //print("seasonTileset[season, 1].name = " + seasonTileset[season, 1].name);
                tileGrid[i, j] = Instantiate(seasonTileset[season, 1], new Vector3(i + transform.position.x, 0, j), Quaternion.Euler(0, Random.Range(0, 3) * 90, 0));
            }
        }
    }

    void GenerateWater(int i, int season)
    {
        for (int j = 0; j < 10; j++)
        {
            //Debug.Log(i + ", " + j);
            tileGrid[i, j] = Instantiate(seasonTileset[season, 2], new Vector3(i + transform.position.x, -0.05f, j), Quaternion.identity);
        }
        if (Random.Range(0, 2) == 0)
        {
            SpawnPreload(log, i + transform.position.x, Random.Range(1, 2.5f));
            //StartCoroutine(WaitAndSpawn(log, i + transform.position.x, Random.Range(1, 2.5f)));
            path = -1;
        }
        else
        {
            SpawnPreload(log, i + transform.position.x, Random.Range(-2.5f, -1));
            //StartCoroutine(WaitAndSpawn(log, i + transform.position.x, Random.Range(-2.5f, -1)));
            path = 10;
        }
    }

    void GenerateRoad(int i, int season)
    {
        for (int j = 0; j < 10; j++)
        {
            //Debug.Log(i + ", " + j);
            tileGrid[i, j] = Instantiate(seasonTileset[season, 3], new Vector3(i + transform.position.x, -0.025f, j), Quaternion.identity);
        }

        if (Random.Range(0, 2) == 0)
        {
            SpawnPreload(car, i + transform.position.x, Random.Range(1, 2.5f));
            //StartCoroutine(WaitAndSpawn(car, i + transform.position.x, Random.Range(1, 2.5f)));
            path = -1;
        }
        else
        {
            SpawnPreload(car, i + transform.position.x, Random.Range(-2.5f, -1));

            //StartCoroutine(WaitAndSpawn(car, i + transform.position.x, Random.Range(-2.5f, -1)));
            path = 10;
        }
    }

    void GenerateTracks(int i, int season)
    {
        for (int j = 0; j < 10; j++)
        {
            //Debug.Log(i + ", " + j);
            tileGrid[i, j] = Instantiate(seasonTileset[season, 3], new Vector3(i + transform.position.x, -0.025f, j), Quaternion.identity);
        }

        if (Random.Range(0, 2) == 0)
        {
            SpawnPreload(trainEngine, i + transform.position.x, Random.Range(1, 2.5f));
            //StartCoroutine(WaitAndSpawn(car, i + transform.position.x, Random.Range(1, 2.5f)));
            path = -1;
        }
        else
        {
            SpawnPreload(trainEngine, i + transform.position.x, Random.Range(-2.5f, -1));

            //StartCoroutine(WaitAndSpawn(car, i + transform.position.x, Random.Range(-2.5f, -1)));
            path = 10;
        }
    }


    public void OnBeginPlayPath()
    {
        Instantiate(tile_path, new Vector3(transform.position.x, 0, -1), Quaternion.identity);
        //Debug.LogError("NewTile");
        int goal = 5;
        int season = Random.Range(0, 3);
        //print("Season = " + season);
        for (int i = 0; i < 10; i++)
        {
            if (i < 1)
            {
                for (int j = 0; j < 10; j++)
                {
                    //print("seasonTileset[season, 1].name = " + seasonTileset[season, 1].name);
                    tileGrid[i, j] = Instantiate(seasonTileset[season, 1], new Vector3(i + transform.position.x, 0, j), Quaternion.Euler(0, Random.Range(0, 3) * 90, 0));
                }
            }
            else if (i < 5)
            {
                for (int j = 0; j < 10; j++)
                {
                    if(j > 3 && j < 6)
                    {
                        tileGrid[i, j] = Instantiate(seasonTileset[season, 0], new Vector3(i + transform.position.x, 0, j), Quaternion.identity);
                    }
                    else
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            //print("seasonTileset[season, 0].name = " + seasonTileset[season, 0].name);
                            tileGrid[i, j] = Instantiate(seasonTileset[season, 0], new Vector3(i + transform.position.x, 0, j), Quaternion.identity);
                        }
                        else
                        {
                            //print("seasonTileset[season, 1].name = " + seasonTileset[season, 1].name);
                            tileGrid[i, j] = Instantiate(seasonTileset[season, 1], new Vector3(i + transform.position.x, 0, j), Quaternion.Euler(0, Random.Range(0, 3) * 90, 0));
                        }
                    }
                }
            }
            else
            {
                //Debug.Log("path is " + path);
                switch (Random.Range(0, 3))
                {
                    case 0:
                        goal = FindNewPath(goal);
                        //Debug.Log("goal is " + goal);

                        GenerateField(goal, i, season);
                        break;
                    case 1:
                        GenerateWater(i, season);
                        break;
                    case 2:
                        GenerateRoad(i, season);
                        break;
                    case 3:
                        Debug.Log("Train");
                        break;
                    default:
                        //Debug.Log("Tile index does not exist.");
                        break;
                }
                path = goal;        //set path to goal for next row
            }
        }
    }


    private void SpawnPreload(GameObject objToSpawn, float rowLocation, float speed)
    {
        GameObject tempCar;
        float posBase = 12 - 14 * Mathf.Clamp01(speed);
        float posForward = 0;

        if (objToSpawn == car)
        {
            for (int i = 0; i < 4; i++)
            {
                posForward += 2.2f / Mathf.Abs(speed) * Random.Range(1, 4) * speed;
                float startingPoint = posBase + posForward;
                tempCar = SpawnCarAt(rowLocation, startingPoint, speed);
            }
        }
        else if (objToSpawn == log)
        {
            for (int i = 0; i < 4; i++)
            {
                posForward += 2.2f / Mathf.Abs(speed) * Random.Range(1, 4) * speed;
                float startingPoint = posBase + posForward;
                tempCar = SpawnLogAt(rowLocation, startingPoint, speed);
            }
        }
        else if (objToSpawn == trainEngine)
        {
            for (int i = 0; i < 4; i++)
            {
                posForward += 2.2f / Mathf.Abs(speed) * Random.Range(1, 4) * speed;
                float startingPoint = posBase + posForward;
                tempCar = SpawnLogAt(rowLocation, startingPoint, speed);
            }
        }

        StartCoroutine(WaitAndSpawn(objToSpawn, rowLocation, speed));
    }

    IEnumerator WaitAndSpawn(GameObject objToSpawn, float rowLocation, float speed)
    {
        yield return new WaitForSeconds(2.2f / Mathf.Abs(speed) * Random.Range(1.6f, 4.1f));
        GameObject tempCar;
        if (objToSpawn == car)
        {
            float startingPoint = 12 - 14 * Mathf.Clamp01(speed);
            tempCar = SpawnCarAt(rowLocation, startingPoint, speed);
        }
        else if (objToSpawn == log)
        {
            float startingPoint = 12 - 14 * Mathf.Clamp01(speed);
            tempCar = SpawnLogAt(rowLocation, startingPoint, speed);
        }
        else if (objToSpawn == trainEngine)
        {
            float startingPoint = 12 - 14 * Mathf.Clamp01(speed);
            tempCar = SpawnTrainEngineAt(rowLocation, startingPoint, speed);
        }
        StartCoroutine(WaitAndSpawn(objToSpawn, rowLocation, speed));
    }

    IEnumerator WaitAndDestroy(GameObject objToDestroy, float waitTime)
    {
        objBacklog.Add(objToDestroy);
        yield return new WaitForSeconds(waitTime);
        objBacklog.Remove(objToDestroy);
        Destroy(objToDestroy);
    }


    GameObject SpawnCarAt(float rowLocation, float startingPoint, float speed)
    {
        GameObject tempCar = Instantiate(car, new Vector3(rowLocation, 0.85f, startingPoint), Quaternion.Euler(-90, 180 - 180 * Mathf.Clamp01(speed), 0));      //0.85 = y

        tempCar.GetComponent<Rigidbody>().velocity = Vector3.forward * speed;
        tempCar.GetComponent<AudioSource>().pitch = Random.Range(1.49f, 2.01f);

        StartCoroutine(WaitAndDestroy(tempCar, 14 / Mathf.Abs(speed)));
        return tempCar;
    }


    GameObject SpawnLogAt(float rowLocation, float startingPoint, float speed)
    {
        GameObject tempLog = Instantiate(log, new Vector3(rowLocation, 0.6f, startingPoint), Quaternion.Euler(-90, 180 - 180 * Mathf.Clamp01(speed), 0));      //0.6 = y

        tempLog.GetComponent<Rigidbody>().velocity = Vector3.forward * speed;

        StartCoroutine(WaitAndDestroy(tempLog, 14 / Mathf.Abs(speed)));
        return tempLog;
    }

    GameObject SpawnTrainEngineAt(float rowLocation, float startingPoint, float speed)
    {
        GameObject tempEngine = Instantiate(trainEngine, new Vector3(rowLocation, 0.95f, startingPoint), Quaternion.Euler(-90, 180 - 180 * Mathf.Clamp01(speed), 0));      //0.6 = y

        tempEngine.GetComponent<Rigidbody>().velocity = Vector3.forward * speed;

        StartCoroutine(WaitAndDestroy(tempEngine, 14 / Mathf.Abs(speed)));
        return tempEngine;
    }
}
