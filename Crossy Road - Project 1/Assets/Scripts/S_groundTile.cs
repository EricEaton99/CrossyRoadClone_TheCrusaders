using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_groundTile : MonoBehaviour
{
    public GameObject tile_grass;
    public GameObject tile_bush;
    public GameObject tile_road;
    public GameObject tile_water;

    [SerializeField] GameObject log;
    [SerializeField] GameObject car;

    GameObject[,] tileGrid = new GameObject[10, 10];
    List<GameObject> objBacklog = new List<GameObject>();

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
        for (int i = 0; i < 10; i++)
        {

            switch (Random.Range(0, 3))
            {
                case 0:                                     //can create walls
                    for (int j = 0; j < 10; j++)
                    {
                        Debug.Log(i + ", " + j);
                        if (Random.Range(0, 2) == 0)
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
                        Debug.Log(i + ", " + j);
                        tileGrid[i, j] = Instantiate(tile_water, new Vector3(i + transform.position.x, -0.05f, j), Quaternion.identity);
                    }
                    if (Random.Range(0, 2) == 0)
                    {
                        StartCoroutine(WaitAndSpawn(log, i + transform.position.x, Random.Range(1, 2.5f)));
                    }
                    else
                    {
                        StartCoroutine(WaitAndSpawn(log, i + transform.position.x, Random.Range(-2.5f, -1)));
                    }
                    break;
                case 2:
                    for (int j = 0; j < 10; j++)
                    {
                        Debug.Log(i + ", " + j);
                        tileGrid[i, j] = Instantiate(tile_road, new Vector3(i + transform.position.x, -0.025f, j), Quaternion.identity);
                    }

                    if (Random.Range(0, 2) == 0)
                    {
                        StartCoroutine(WaitAndSpawn(car, i + transform.position.x, Random.Range(1, 2.5f)));
                    }
                    else
                    {
                        StartCoroutine(WaitAndSpawn(car, i + transform.position.x, Random.Range(-2.5f, -1)));
                    }
                    break;
                default:
                    Debug.Log("Tile index does not exist.");
                    break;
            }
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
