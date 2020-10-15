using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlocking : MonoBehaviour
{
    public GameObject[] characterObjects; //stores the characters
    bool[] characterBools; //stores if the character can be selected
    int unlockedChars;


    void Start()
    {
        characterBools = new bool[23];
        for (int a = 0; a < characterObjects.Length; a++) //lock everything
        {
            characterBools[a] = false;
        }

        unlockedChars = PlayerPrefs.GetInt("unlocks");

        for (int b = 0; b <= unlockedChars; b++)
        {
            characterBools[b] = true; //5th character unlocked -> set "unlocks" as 5, then make every characterBool up to and including 5, true
        }
    }
}
