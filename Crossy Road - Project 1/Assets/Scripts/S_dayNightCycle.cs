using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_dayNightCycle : MonoBehaviour
{
    Quaternion lightOrigin;                   //Where paddle was when the destination was set
    Quaternion lightDestination;              //Where paddle is going
    //float waitTime;                         //Time between this and next lightDestination
    public Light light;
    public Color[] timeOfDayArray;
    [SerializeField] float dayLength;
    float changeLengthRatio;


    void Start()
    {
        light = light.GetComponent<Light>();

        changeLengthRatio = dayLength / timeOfDayArray.Length;
        Debug.LogError(changeLengthRatio);
    }

    private void Update()
    {

        float lerp = Time.time % changeLengthRatio / changeLengthRatio;
        int color1 = (int)Mathf.Floor(Time.time % dayLength / changeLengthRatio);
        int color2;
        if (color1 < timeOfDayArray.Length - 1)
        {
            color2 = color1 + 1;
        }
        else
        {
            color2 = 0;
        }

        //print(color1 + ", " + color2);
        //print("lerp = " + lerp);
        //print("floor(" + Time.time + " % " + dayLength + " / " + changeLengthRatio + ")");

        light.color = Color.Lerp(timeOfDayArray[color1], timeOfDayArray[color2], lerp);
    }
}