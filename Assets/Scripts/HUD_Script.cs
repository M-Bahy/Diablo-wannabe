using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;
using System;
using System.Reflection;


public class HUD_Script : MonoBehaviour
{

    public int abilityPoints; 
    public Button[] abilityButtons; 
    public TMP_Text[] buttonTexts;
    public TMP_Text[] coolDownTimerTexts;

    public static bool[] abilitiesUnlocked = new bool[4];
    public static bool[] abilitiesCoolDown = new bool[4];
    public static float[] coolDownTimer = new float[4];

    public static GameObject player;
    PlayerMechanics pm;

    void Start()
    {
        
        // abilitiesUnlocked = new bool[4];
        abilitiesUnlocked[0] = true;
        // abilitiesCoolDown = new bool[4];
        // coolDownTimer = new float[4];
        coolDownTimer[0] = 0.0f;

       // pm = player.GetComponent<PlayerMechanics>();
        Debug.Log(pm==null);
        abilityPoints = PlayerMechanics.abilityPoints;

        for (int i = 1; i < abilityButtons.Length; i++)
        {
            int index = i; // Capture index for the lambda expression
            abilityButtons[i].onClick.AddListener(() => TryUnlockAbility(index));
            coolDownTimer[i] = 0.0f;
            coolDownTimerTexts[i].transform.parent.gameObject.SetActive(false);
        }
    }

    void TryUnlockAbility(int index)
    {
        if (!abilitiesUnlocked[index] && abilityPoints > 0)
        {
            abilitiesUnlocked[index] = true;
            PlayerMechanics.abilityPoints--;
            UnlockAbility(index);
        }
    }

    void UnlockAbility(int index)
    {
        buttonTexts[index].color = Color.green;
        abilityButtons[index].GetComponent<Image>().color = Color.black;
    }
   
    void Update()
    {
        // if the ability is cool down make text white
        for (int i = 0; i < abilitiesCoolDown.Length; i++)
        {
            if (abilitiesCoolDown[i] && abilitiesUnlocked[i])
            {
                buttonTexts[i].color = Color.white;
            }
            else if (!abilitiesCoolDown[i] && abilitiesUnlocked[i])
            {
                buttonTexts[i].color = Color.green;
                abilityButtons[i].GetComponent<Image>().color = Color.black;
            }
            else
            {
                Color hexColor;
                if (ColorUtility.TryParseHtmlString("#766E6E", out hexColor))
                {
                    abilityButtons[i].GetComponent<Image>().color = hexColor;
                }
            }
        }
        abilityPoints = PlayerMechanics.abilityPoints;

        for (int i = 0; i < coolDownTimer.Length; i++)
        {
            if (coolDownTimer[i] > 0)
            {
                if (!coolDownTimerTexts[i].transform.parent.gameObject.active)
                    coolDownTimerTexts[i].transform.parent.gameObject.SetActive(true);
                coolDownTimer[i] -= Time.deltaTime;
                coolDownTimerTexts[i].text = coolDownTimer[i].ToString("F1");
            }
            else
            {
                if (coolDownTimerTexts[i].transform.parent.gameObject.active)
                    coolDownTimerTexts[i].transform.parent.gameObject.SetActive(false);
            }
        }

        if (abilityPoints > 0)
        {
            //loop over the button array, if the ability is unlocked, then change the color to show that the player can click to unlock the ability
            for (int i = 1; i < abilityButtons.Length; i++)
            {
                if (!abilitiesUnlocked[i])
                {
                    //abilityButtons[i].GetComponent<Image>().color = Color.black;
                    //animate the color of the button to show the user that they can unlock the ability
                    //abilityButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, Mathf.PingPong(Time.time, 1));
                    Color startColor = Color.red; // Locked state color (e.g., red)
                    Color endColor = Color.yellow; // "Unlockable" highlight color (e.g., yellow)

                    // PingPong creates a smooth transition between 0 and 1
                    float t = Mathf.PingPong(Time.time * 2f, 1f); // Speed up the transition with a multiplier

                    abilityButtons[i].GetComponent<Image>().color = Color.Lerp(startColor, endColor, t);



                }
            }
        }
    }

    // reset all the cooldowns
    public static void ResetCoolDowns()
    {
        for (int i = 0; i < coolDownTimer.Length; i++)
        {
            coolDownTimer[i] = 0.0f;
            abilitiesCoolDown[i] = false;
        }
    }
}