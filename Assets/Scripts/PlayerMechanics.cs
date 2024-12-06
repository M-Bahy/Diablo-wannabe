using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerMechanics : MonoBehaviour
{
    /////////////   
    public static bool isAttacking = false;
    public static bool barAttacking = false;
    private bool buttonCliked = false;
    private bool defenseButtonClicked = false;
    private string tag;
    private NavMeshAgent agent;
    private bool ultimateButtonClicked = false;
    public GameObject infernoPrefab;
    private bool wildButtonClicked = false;
    //////////////

    int level = 1;
    int exp = 0;
    int requiredExp = 100;
    int playerMaxHealth = 100;
    public int playerCurrenttHealth = 90;
    int numberOfHealingPortions = 0;
    int abilityPoints = 0;
    int numberOfFragments = 0;
    private Animator animator;

    public static bool isLevel1 = true;



    public TMP_Text healthText ;
    public Slider healthSlider ; 

    public TMP_Text expText ;
    public Slider expSlider ; 

    public TMP_Text levelText ;


    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject portal;
    public GameObject telepoertCircle;



    [SerializeField] GameObject wizardClone;
    [SerializeField] Camera _maincamera;
    // Start is called before the first frame update
    void Start()
    {
        ///////
        animator = GetComponent<Animator>();
        
        agent = GetComponent<NavMeshAgent>();
        tag = gameObject.tag;
        ///////
        healthSlider.value = playerCurrenttHealth;
        healthSlider.maxValue = playerMaxHealth;

        expSlider.value = exp;
        expSlider.maxValue = requiredExp;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLevel1)
        {
            BossMech boss = GameObject.Find("Tortoise_Boss_Anims").GetComponent<BossMech>();
            if(boss.gameOver){
                agent.isStopped = true;
                healthSlider.value = 0;
                healthText.text = "0";
            
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
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
        if (Input.GetKeyDown(KeyCode.Z))
        {
           exp+=30;

        }
      
        if (exp >= requiredExp  && level < 4){
            levelUp();
        }

        

        updateHUDUI();


        // if(playerCurrenttHealth == 0 ){
        //     animator.Play("dead");
        //     //game over here
        // }
        




        //cheats
        //exp cheat
        if (Input.GetKeyDown(KeyCode.X)){
            exp += 100;
        }
        //ability cheat
        if (Input.GetKeyDown(KeyCode.A)){
            abilityPoints += 1;
        }
        if (Input.GetKeyDown(KeyCode.H)){
            playerCurrenttHealth +=20;
        }
        if (Input.GetKeyDown(KeyCode.D)){
            playerCurrenttHealth -=20;
        }



        if (!buttonCliked && Input.GetMouseButtonDown(1) && !isAttacking)
        {
            BasicAttack();
        }

        if (Input.GetKeyDown(KeyCode.W) && HUD_Script.abilitiesUnlocked[1] && !HUD_Script.abilitiesCoolDown[1] && !buttonCliked)
        {
            buttonCliked = true;
            defenseButtonClicked = true;
        }
        if (Input.GetKeyDown(KeyCode.E) && HUD_Script.abilitiesUnlocked[2] && !HUD_Script.abilitiesCoolDown[2] && !buttonCliked)
        {
            buttonCliked = true;
            ultimateButtonClicked = true;
        }


        if (Input.GetKeyDown(KeyCode.Q) && HUD_Script.abilitiesUnlocked[3] && !HUD_Script.abilitiesCoolDown[3] && !buttonCliked)
        {
            buttonCliked = true;
            wildButtonClicked = true;
        }

        if (ultimateButtonClicked && Input.GetMouseButtonDown(1))
        {

            UltimateAttack();

        }
        if (defenseButtonClicked && Input.GetMouseButtonDown(1))
        {

            DefensiveAttack();

        }

        if (wildButtonClicked && Input.GetMouseButtonDown(1))
        {
            Ray ray = _maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                WildAttack(hit.point);
            }
            

        }


    }



    void BasicAttack()
    {
        animator.ResetTrigger("Attack");
        if (tag == "Sorcerer")
        {
            isAttacking = true;
            animator.Play("attack_short_001", 0, 0f);
            StartCoroutine(ResetAfterAttack());
        }
        else if(tag == "Barbarian")
        {
          
          //  isAttacking = true;
            animator.SetBool("Attack", true);
            barAttacking = true;
            StartCoroutine(ResetAfterBarbarianAttack());



            //animator.Play("Normal_Attack", 0, 0f);
            //StartCoroutine(ResetAfterAttack());
            //animator.SetBool("Attack", false);

        }
    }

    void DefensiveAttack()
    {
        if (tag == "Sorcerer")
        {


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // add one two seconds delay
                //StartCoroutine(TeleportAfterDelay(hit.point));
                agent.enabled = false;
                transform.position = hit.point;
                agent.enabled = true;
                agent.SetDestination(hit.point);


                StartCoroutine(AbilityCooldown(1, 10f)); // Cooldown for 10 seconds

            }

        }
        defenseButtonClicked = false;
        buttonCliked = false;

    }
    void UltimateAttack()
    {


        if (tag == "Sorcerer")
        {


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 spawnPosition = hit.point;
                if (!isLevel1)
                    spawnPosition.y += 5.0f;
                GameObject infernoInstance =  Instantiate(infernoPrefab, spawnPosition, Quaternion.identity);
                StartCoroutine(AbilityCooldown(2, 15f));
                Destroy(infernoInstance, 5f);

            }
        }
        ultimateButtonClicked = false;
        buttonCliked = false;
    }


    void WildAttack(Vector3 pos)
    {
        if (tag == "Sorcerer")
        {
            GameObject clone = Instantiate(wizardClone, new Vector3(pos.x, 0, pos.z), Quaternion.identity);
            Minion_Logic.wizardClone = clone;
            // make the clone explode after 5 seconds, dealing 10 damage within a radius
            // make it disappear
            StartCoroutine(AbilityCooldown(3, 10f));
        }
        else if (tag == "Barbarian")
        {
            StartCoroutine(AbilityCooldown(3, 5f));
        }

        wildButtonClicked = false;
        buttonCliked = false;
    }


    IEnumerator ResetAfterAttack()
    {

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;

    }
    IEnumerator ResetAfterBarbarianAttack()
    {
        yield return new WaitForSeconds(1.9f); // Wait for the animation duration
        animator.SetBool("Attack", false); // Reset the attack animation state
        barAttacking = false;
    }

    IEnumerator TeleportAfterDelay(Vector3 targetPosition)
    {
        // Wait for 2 seconds before teleporting
        yield return new WaitForSeconds(2f);

        // Teleport to the target position
        transform.position = targetPosition;
    }

    IEnumerator AbilityCooldown(int abilityIndex, float cooldownTime)
    {
        // Set the ability cooldown to true
        HUD_Script.abilitiesCoolDown[abilityIndex] = true;

        // Wait for the cooldown duration
        yield return new WaitForSeconds(cooldownTime);

        // Set the ability cooldown back to false
        HUD_Script.abilitiesCoolDown[abilityIndex] = false;
    }

  


    private void updateHUDUI()
    {
        if(healthSlider.value != playerCurrenttHealth){
            healthSlider.value = playerCurrenttHealth;
            
        }
        healthText.text = $"{playerCurrenttHealth:F0}";

        if(expSlider.value != exp && level < 4){
            expSlider.value = exp;
            
        }
        if( level == 4){
            exp = 0;
            expSlider.value = exp;
        }
        expText.text = $"{exp:F0}";
        

        levelText.text = $"{level:F0}";


        
    }
    private void levelUp(){

        level += 1;
        exp = exp - requiredExp;
        requiredExp +=100;
        
        if(level == 4){
            exp = 0;
        }
        
        expSlider.maxValue = requiredExp;




        playerMaxHealth +=100;
        playerCurrenttHealth = playerMaxHealth;
        // this is for the ui slider
        healthSlider.maxValue = playerMaxHealth;
    }
    
    
    public void takeDamage(int damage){
        playerCurrenttHealth -= damage;
        if(playerCurrenttHealth <= 0){
            playerCurrenttHealth = 0;
           animator.Play("dead");
           // here we should display the end game screen
           if(!isLevel1){
           BossMech boss = GameObject.Find("Tortoise_Boss_Anims").GetComponent<BossMech>();
           boss.gameOver = true;
           }
           

        }else{
            animator.Play("damage");
        }
    }
    
    
    private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.tag == "Fragment")
    {
        numberOfFragments++;
        Destroy(collision.gameObject);
        OpenPortal();
    }

    if (collision.gameObject.tag == "portal")
    {
        SceneManager.LoadScene("Level2_scene");
    }
}

    private void OpenPortal()
    {
        if(numberOfFragments >= 3){
            leftDoor.SetActive(false);
            rightDoor.SetActive(false);
            portal.SetActive(true);
            telepoertCircle.SetActive(true);
        }
    }
}
