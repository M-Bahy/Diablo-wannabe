using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class BossMech : MonoBehaviour
{
    bool phaseOne = true;


    private Animator animator;

    public int phaseOneHealth = 50;
    public int shieldHealth = 50;
    public int phaseTwoHealth = 50;



    public GameObject shield ;

    public TMP_Text bosshealthText ;
    public Slider bosshealthSlider ; 
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();


        bosshealthSlider.maxValue = phaseOneHealth;
        bosshealthSlider.value = phaseOneHealth;
        
    }

    public void damageBoss(){
        phaseOneHealth -=10;
        updateHUDUI();
        checkIfBossDie();
    }

    private void checkIfBossDie()
    {
        if(phaseOneHealth <= 0){
            animator.Play("Death");
            //animator.Play("Resurrection");
            startPhaseTwo();
        }
    }

    private void startPhaseTwo()
    {

        phaseOne = false;
        bosshealthSlider.maxValue = phaseTwoHealth;
        bosshealthSlider.value = phaseTwoHealth;
        updateHUDUI();
        shield.SetActive(true);


        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)){
            damageBoss();
        }

    }

    private void updateHUDUI()
    {
        if(phaseOne){
            bosshealthSlider.value = phaseOneHealth;
            bosshealthText.text = $"{phaseOneHealth:F0}";
        }
        else{
            bosshealthSlider.value = phaseTwoHealth;
            bosshealthText.text = $"{phaseTwoHealth:F0}";
        }

        
    }
}
