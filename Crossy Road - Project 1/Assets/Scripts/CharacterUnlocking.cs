using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlocking : MonoBehaviour
{
    public GameObject[] characterObjects; //stores the characters
    bool[] characterBools; //stores if the character can be selected
    int unlockedChars;

    GameObject[] characterSelect;
    int selectIndex;

    public GameObject characterHolder;

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

        characterSelect = new GameObject[PlayerPrefs.GetInt("unlocks")];

        UpdateCharacterSelect();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) //developer key
        {
            selectIndex = 0;
            UpdateCharacterSelect();
            
        }
    }

    public void cSelectRight()
    {
        selectIndex++;

        if (selectIndex > characterSelect.Length)
        {
            selectIndex = 0;
        }
        UpdateCharacterSelect();
    }

    public void cSelectLeft()
    {
        selectIndex--;
        {
            if (selectIndex < characterSelect.Length)
            {
                selectIndex = characterSelect.Length - 1;
            }
        }
        UpdateCharacterSelect();
    }

    void UpdateCharacterSelect()
    {
        GameObject tempCharacter;

        for (int x = 0; x < 23; x++)
        {
            tempCharacter = characterHolder.transform.GetChild(x).gameObject;
            tempCharacter.SetActive(false);
        }

        tempCharacter = characterHolder.transform.GetChild(selectIndex).gameObject;
        tempCharacter.SetActive(true);

        PlayerPrefs.SetInt("currentCharacter", selectIndex);
    }
}
