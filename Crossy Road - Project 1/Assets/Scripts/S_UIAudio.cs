using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_UIAudio : MonoBehaviour
{
    public AudioClip hoverSound;
    public AudioClip clickSound;
    public AudioSource myFx;


    public void ButtonHover()
    {
        myFx.PlayOneShot(hoverSound);
    }

    public void ButtonClick()
    {
        myFx.PlayOneShot(clickSound);
    }
}
