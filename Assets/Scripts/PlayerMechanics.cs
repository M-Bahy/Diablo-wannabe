using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    
    int level = 1;
    int exp = 0;
    int playerMaxHealth = 100;
    int playerCurrenttHealth = 90;
    int numberOfHealingPortions = 3;
    private Animator animator;





    public TMP_Text healthText ;
    public Slider healthSlider ; 
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
        healthSlider.maxValue = playerMaxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            // Trigger the animation
            if(numberOfHealingPortions>0 && playerCurrenttHealth != playerMaxHealth ){
                animator.Play("heal");
                numberOfHealingPortions -- ;
                

                playerCurrenttHealth += playerMaxHealth/2; 
                if(playerCurrenttHealth > playerMaxHealth){
                    playerCurrenttHealth = playerMaxHealth;
                }
            }
        }

        updateHealthUI();

        // if(playerCurrenttHealth == 0 ){
        //     animator.Play("dead");
        //     //game over here
        // }
        
    }

    private void updateHealthUI()
    {
        healthText.text =  playerCurrenttHealth.ToString("F0");
        if(healthSlider.value != playerCurrenttHealth){
            healthSlider.value = playerCurrenttHealth;
        }
        healthText.text = $"{playerCurrenttHealth:F0}";
    }
    private void levelUp(){
        playerMaxHealth +=100;
        playerCurrenttHealth = playerMaxHealth;
        // this is for the ui slider
        healthSlider.maxValue = playerMaxHealth;
    }
}
