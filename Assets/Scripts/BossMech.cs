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
    public TMP_Text shieldhealthText ;

    public Slider shieldHealthSlider ; 

    public Slider bosshealthSlider ; 
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        shield.SetActive(false);
        shieldHealthSlider.gameObject.SetActive(false);

        bosshealthSlider.maxValue = phaseOneHealth;
        bosshealthSlider.value = phaseOneHealth;

        shieldHealthSlider.maxValue= shieldHealth;
        shieldHealthSlider.value = shieldHealth;
        
    }

    public void damageBoss(){
        if(phaseOne){
            phaseOneHealth -=11;

        }
        else{
            Debug.Log("Ana gwa el else");
            if(shieldHealth >0 ){
                Debug.Log("Ana damage shield");
                shieldHealth -= 11;
                
                
                if(shieldHealth < 0){
                    phaseTwoHealth += shieldHealth;
                    shieldHealth = 0;
                }
            }
            else{
                phaseTwoHealth -=11;
            }

        }
        updateHUDUI();
        checkIfBossDie();
    }

    private void checkIfBossDie()
    {
        if(phaseOne){
            if(phaseOneHealth <= 0){
                animator.Play("Death");
                //animator.Play("Resurrection");
                startPhaseTwo();
        }

        }
 
    }

    private void startPhaseTwo()
    {

        phaseOne = false;
        bosshealthSlider.maxValue = phaseTwoHealth;
        bosshealthSlider.value = phaseTwoHealth;

        shield.SetActive(true);
        shieldHealthSlider.gameObject.SetActive(true);

        shieldHealthSlider.maxValue= shieldHealth;
        shieldHealthSlider.value = shieldHealth;
        updateHUDUI();

        

    


        
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
            shieldHealthSlider.value = shieldHealth;
            shieldhealthText.text = $"{shieldHealth:F0}";

            bosshealthSlider.value = phaseTwoHealth;
            bosshealthText.text = $"{phaseTwoHealth:F0}";
        }

        
    }
}
