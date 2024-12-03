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

    public static bool[] abilitiesUnlocked;


    void Start()
    {
        
           abilitiesUnlocked = new bool[4];
           abilitiesUnlocked[0] = true;
           
        


        for (int i = 1; i < abilityButtons.Length; i++)
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