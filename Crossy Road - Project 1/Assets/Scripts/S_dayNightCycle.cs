using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_dayNightCycle : MonoBehaviour
{
    float timeStartedLerping;        //When the destination was set
    Quaternion lightOrigin;                   //Where paddle was when the destination was set
    Quaternion lightDestination;              //Where paddle is going
    float waitTime;                         //Time between this and next lightDestination
    public GameObject light;
    public Color[] timeOfDayArray;
    int timeOfDay;



    void Start()
    {
        timeStartedLerping = Time.time;
        timeOfDay = Random.Range(0, timeOfDayArray.Length);
    }

    private void Update()
    {
        //light.GetComponent<Light>().color = LerpColor();     //move the paddle to where the ball is going
    }

    Color LerpColor()
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentComplete = timeSinceStarted / 1;

        var result = Color.Lerp(timeOfDayArray[timeOfDay], timeOfDayArray[timeOfDay+1], percentComplete);
        return (result);
    }
}
