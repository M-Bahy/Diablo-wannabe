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
    public GameObject aura;
    public TMP_Text bosshealthText ;
    public TMP_Text shieldhealthText ;

    public Slider shieldHealthSlider ; 

    public Slider bosshealthSlider ; 

    float generationDelay = 10f;
    float ogGenerationDelay = 0;
    bool shieldDestroyed = false;
    public bool gameOver = false;

    public bool auraActivated = false;

    AudioManagerScript audioManager;

    private void Awake() {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManagerScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        shield.SetActive(false);
        aura.SetActive(false);
        shieldHealthSlider.gameObject.SetActive(false);

        bosshealthSlider.maxValue = phaseOneHealth;
        bosshealthSlider.value = phaseOneHealth;

        shieldHealthSlider.maxValue= shieldHealth;
        shieldHealthSlider.value = shieldHealth;
        ogGenerationDelay = generationDelay;
        
    }

    public void damageBoss(int damageAmount){
        if(!gameObject.GetComponent<Boss_phase1_script>().isAllMinionsDead()){
            return;
        }
        if(phaseOne){
            phaseOneHealth -= damageAmount;
             animator.Play("GetDamage");
             audioManager.PlaySFX(audioManager.Boss_Getting_Damaged);
        }
        else{

            if (!auraActivated){
                //  Debug.Log("Ana gwa el else");
            if(shieldHealth >0 ){
              //  Debug.Log("Ana damage shield");
                shieldHealth -= damageAmount;
                
                
                if(shieldHealth <= 0){
                    phaseTwoHealth += shieldHealth;
                    shieldHealth = 0;
                    shield.SetActive(false);
                    shieldHealthSlider.gameObject.SetActive(false); 
                    shieldDestroyed = true;
                    animator.Play("GetDamage2");
                    audioManager.PlaySFX(audioManager.Boss_Getting_Damaged); 
                }
            }
            else{
                phaseTwoHealth -= damageAmount;
                animator.Play("GetDamage2");
                audioManager.PlaySFX(audioManager.Boss_Getting_Damaged);
            }
            }
            else{
                // the player here should take damage equals to the damage amount + 15
                deactivateAura();
                
                Boss_phase1_script bs = gameObject.GetComponent<Boss_phase1_script>();
                bs.player.GetComponent<PlayerMechanics>().takeDamage(damageAmount + 15);
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
                audioManager.PlaySFX(audioManager.Boss_Dies);
                //animator.Play("Resurrection");
                startPhaseTwo();
        }

        }
        else{
            if(phaseTwoHealth <= 0){
                animator.Play("END");
                audioManager.PlaySFX(audioManager.Boss_Dies);
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
        audioManager.PlaySFX(audioManager.Shield_Activated);
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
            damageBoss(11);
        }

        if(shieldDestroyed ){
            generationDelay -= Time.deltaTime;
            if(generationDelay <= 0){
                shieldHealth = 50;
              //  shieldHealthSlider.value = shieldHealth;
                shieldDestroyed = false;
                shield.SetActive(true);
                audioManager.PlaySFX(audioManager.Shield_Activated);
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

    // activate aura
    public void activateAura(){
        auraActivated = true;
        aura.SetActive(true);
    }

    public void deactivateAura(){
        auraActivated = false;
        aura.SetActive(false);
    }
}
