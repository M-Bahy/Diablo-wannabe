using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;


public class HUD_Script : MonoBehaviour
{

    public int abilityPoints; 
    public Button[] abilityButtons; 
    public TMP_Text[] buttonTexts;
    public TMP_Text[] coolDownTimerTexts;

    public static bool[] abilitiesUnlocked;
    public static bool[] abilitiesCoolDown;
    public static float[] coolDownTimer;

    [SerializeField] private GameObject player;
    PlayerMechanics pm;

    void Start()
    {
        
        abilitiesUnlocked = new bool[4];
        abilitiesUnlocked[0] = true;
        abilitiesCoolDown = new bool[4];
        coolDownTimer = new float[4];
        coolDownTimer[0] = 0.0f;

        pm = player.GetComponent<PlayerMechanics>();
        abilityPoints = pm.abilityPoints;

        for (int i = 1; i < abilityButtons.Length; i++)
        {
            int index = i; // Capture index for the lambda expression
            abilityButtons[i].onClick.AddListener(() => TryUnlockAbility(index));
            coolDownTimer[i] = 0.0f;
        }
    }

    void TryUnlockAbility(int index)
    {
        if (!abilitiesUnlocked[index] && abilityPoints > 0)
        {
            abilitiesUnlocked[index] = true;
            pm.abilityPoints--;
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
            else if(!abilitiesCoolDown[i] && abilitiesUnlocked[i])
            {
                buttonTexts[i].color = Color.green;
            }
        }
        abilityPoints = pm.abilityPoints;

        for (int i = 0; i < coolDownTimer.Length; i++)
        {
            if (coolDownTimer[i] > 0)
            {
                coolDownTimerTexts[i].transform.parent.gameObject.SetActive(true);
                coolDownTimer[i] -= Time.deltaTime;
                coolDownTimerTexts[i].text = coolDownTimer[i].ToString("F1");
            }
            else
            {
                coolDownTimerTexts[i].transform.parent.gameObject.SetActive(false);
            }
        }
    }
}