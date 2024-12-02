using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;


public class HUD_Script : MonoBehaviour
{

    public int abilityPoints = 0; 
    public Button[] abilityButtons; 
    public TMP_Text[] buttonTexts;

    private bool[] abilitiesUnlocked;


    void Start()
    {
        abilitiesUnlocked = new bool[abilityButtons.Length];

       
        for (int i = 0; i < abilityButtons.Length; i++)
        {
            int index = i; // Capture index for the lambda expression
            abilityButtons[i].onClick.AddListener(() => TryUnlockAbility(index));
        }
    }

    void TryUnlockAbility(int index)
    {
        if (!abilitiesUnlocked[index] && abilityPoints > 0)
        {
            abilitiesUnlocked[index] = true;
            abilityPoints--;
            UnlockAbility(index);
        }
    }

    void UnlockAbility(int index)
    {
        buttonTexts[index].color = Color.green;
        abilityButtons[index].GetComponent<Image>().color = Color.black;
    }


}