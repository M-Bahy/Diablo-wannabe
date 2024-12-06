using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class BossMech : MonoBehaviour
{
    public bool phaseOne = true;


    private Animator animator;

    public int phaseOneHealth = 50;
    public int shieldHealth = 50;
    public int phaseTwoHealth = 50;



    public GameObject shield ;

    public TMP_Text bosshealthText ;
    public TMP_Text shieldhealthText ;

    public Slider shieldHealthSlider ; 

    public Slider bosshealthSlider ; 

    float generationDelay = 10f;
    float ogGenerationDelay = 0;
    bool shieldDestroyed = false;
    public bool gameOver = false;

    public bool auraActivated = false;
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
        ogGenerationDelay = generationDelay;
        
    }

    public void damageBoss(){
       
        if(phaseOne){
            phaseOneHealth -=11;
             animator.Play("GetDamage");
        }
        else{

            if (!auraActivated){
                //  Debug.Log("Ana gwa el else");
            if(shieldHealth >0 ){
              //  Debug.Log("Ana damage shield");
                shieldHealth -= 11;
                
                
                if(shieldHealth <= 0){
                    phaseTwoHealth += shieldHealth;
                    shieldHealth = 0;
                    shield.SetActive(false);
                    shieldHealthSlider.gameObject.SetActive(false); 
                    shieldDestroyed = true;
                    animator.Play("GetDamage2"); 
                }
            }
            else{
                phaseTwoHealth -=11;
                animator.Play("GetDamage2");
            }
            }
            else{
                // the player here should take damage equals to the damage amount + 15
                auraActivated = false;
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
        else{
            if(phaseTwoHealth <= 0){
                animator.Play("END");
                phaseTwoHealth = 0;
                bosshealthSlider.value = phaseTwoHealth;
                bosshealthText.text = $"{phaseTwoHealth:F0}";
                gameOver = true;
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
        if (gameOver){
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.P)){
            damageBoss();
        }

        if(shieldDestroyed ){
            generationDelay -= Time.deltaTime;
            if(generationDelay <= 0){
                shieldHealth = 50;
              //  shieldHealthSlider.value = shieldHealth;
                shieldDestroyed = false;
                shield.SetActive(true);
                shieldHealthSlider.gameObject.SetActive(true);
                generationDelay = ogGenerationDelay;
                updateHUDUI();
        }
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
