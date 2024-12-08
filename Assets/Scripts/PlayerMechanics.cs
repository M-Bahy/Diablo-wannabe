using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;


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
    private GameObject activeShield;
    public GameObject shieldPrefab;
    //////////////

    int level = 1;
    int exp = 0;
    int requiredExp = 100;
    public int playerMaxHealth = 100;
    public int playerCurrenttHealth = 90;
    int numberOfHealingPortions = 0;
    public int abilityPoints = 0;
    int numberOfFragments = 0;
    private Animator animator;
    private Collider collider;
    public static bool isLevel1 = true;



    public TMP_Text healthText ;
    public Slider healthSlider ; 

    public TMP_Text expText ;
    public Slider expSlider ; 

    public TMP_Text levelText ;

    [SerializeField] TMP_Text abilityPointsText;

    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject portal;
    public GameObject telepoertCircle;



    [SerializeField] GameObject wizardClone;
    [SerializeField] Camera _maincamera;
    [SerializeField] GameObject Fireball;

    AudioManagerScript audioManager;

    private void Awake() {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManagerScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        ///////
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
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

        if (tag == "Barbarian" && animator.GetBool("isSprint"))
        {
            collider.isTrigger = true;
        }
        if (tag == "Barbarian" && Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(agent.destination.x, 0, agent.destination.z)) < 4f && animator.GetBool("isSprint"))
            
        {   
            collider.isTrigger = false;
            //Vector3 newPosition = transform.position;
            // agent.SetDestination(newPosition);
            animator.SetBool("isSprint", false);
            animator.SetBool("isWalking", true);
        }
       if(tag == "Barbarian" && transform.position.x >= 27 && transform.position.x <= 50 && transform.position.z >= 22 && transform.position.z <= 27)
        {
            agent.SetDestination(agent.transform.position);
       }
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
      
        if (exp >= requiredExp  && level < 4){
            levelUp();
        }

        

        updateHUDUI();
        // OpenPortal();

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
            Ray ray = _maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                BasicAttack(hit.point);
            }
        }

        if (tag == "Sorcerer" && Input.GetKeyDown(KeyCode.W) && HUD_Script.abilitiesUnlocked[1] && !HUD_Script.abilitiesCoolDown[1] && !buttonCliked)
        {
            buttonCliked = true;
            defenseButtonClicked = true;
        }
        if (Input.GetKeyDown(KeyCode.E) && HUD_Script.abilitiesUnlocked[2] && !HUD_Script.abilitiesCoolDown[2] && !buttonCliked)
        {
            buttonCliked = true;
            ultimateButtonClicked = true;
        }


        if (tag == "Sorcerer" && Input.GetKeyDown(KeyCode.Q) && HUD_Script.abilitiesUnlocked[3] && !HUD_Script.abilitiesCoolDown[3] && !buttonCliked)
        {
            buttonCliked = true;
            wildButtonClicked = true;
        }
        if (tag == "Barbarian" && Input.GetKeyDown(KeyCode.W) && HUD_Script.abilitiesUnlocked[1] && !HUD_Script.abilitiesCoolDown[1] && barAttacking == false)
        {
            DefensiveAttack();
        }
        if (tag == "Barbarian" && Input.GetKeyDown(KeyCode.Q) && HUD_Script.abilitiesUnlocked[3] && !HUD_Script.abilitiesCoolDown[3] && barAttacking == false)
        {
            WildAttack( new Vector3());
        }
        if (ultimateButtonClicked && Input.GetMouseButtonDown(1))
        {
            Ray ray = _maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                UltimateAttack(hit.point); 
            }
          

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



    void BasicAttack(Vector3 pos)
    {
        //animator.ResetTrigger("Attack");
        if (tag == "Sorcerer")
        {
            audioManager.PlaySFX(audioManager.Fireball_Shot);
            StartCoroutine(AbilityCooldown(0, 1f)); // Cooldown for 1 second
            StartCoroutine(SpawnFireballWithDelay(0.5f, pos));
        }
        else if(tag == "Barbarian")
        {
            Collider[] hitColliders = Physics.OverlapSphere(pos, 5.0f); // Adjust radius as needed
            bool enemyFound = false;
            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Summoned_Minions"))
                {
                    enemyFound = true;
                    break;
                }
            }
            if (enemyFound)
            {
                animator.SetBool("Attack", true);
                barAttacking = true;
                StartCoroutine(ResetAfterBarbarianAttack());
                StartCoroutine(AbilityCooldown(0, 1f));
            }



            //animator.Play("Normal_Attack", 0, 0f);
            //StartCoroutine(ResetAfterAttack());
            //animator.SetBool("Attack", false);

        }
    }

    private IEnumerator SpawnFireballWithDelay(float delay, Vector3 pos)
    {
        // Check if there's an enemy at the specified position
        Collider[] hitColliders = Physics.OverlapBox(pos, new Vector3(5.0f, 5.0f, 5.0f)); // Adjust radius as needed
        bool enemyFound = false;
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Minion") || collider.CompareTag("Demon") || collider.CompareTag("Summoned_Minions"))
            {
                enemyFound = true;
                break;
            }
        }

        if (enemyFound)
        {
            /////////////////////
            // Calculate the target direction, keeping rotation limited to the Y-axis
            Vector3 direction = pos - transform.position;
            direction.y = 0; // Ignore vertical difference
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float rotationSpeed = 5f; // Adjust rotation speed as needed

            while (true)
            {
                // Gradually rotate towards the target on the Y-axis
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed * 360);

                // Exit loop when close enough to target rotation
                if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
                {
                    break;
                }

                yield return null; // Wait for the next frame
            }

            // Ensure exact final alignment
            transform.rotation = targetRotation;
            /////////////////////
            animator.SetBool("isAttacking", true);
            yield return new WaitForSeconds(delay);


            //transform.LookAt(pos); LINE BEING REPLACED
            //rb.isKinematic = true;
            GameObject fireballInstance = new GameObject();
            if (isLevel1)
                fireballInstance = Instantiate(Fireball, new Vector3(transform.position.x+1, 1, transform.position.z+1), Quaternion.identity);
            else
            {
                fireballInstance = Instantiate(Fireball, new Vector3(transform.position.x + 1, 8, transform.position.z + 1), Quaternion.identity);
                fireballInstance.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); // Adjust the scale values as needed
            }

            FbScript fireballLogic = fireballInstance.GetComponent<FbScript>();
            if (fireballLogic != null)
            {
                fireballLogic.SetTarget(pos);
            }
        }
        yield return new WaitForSeconds(delay);
        //rb.isKinematic = false;
        animator.SetBool("isAttacking", false);
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
                if (isLevel1)
                {
                    transform.position = hit.point;
                    agent.enabled = true;
                    agent.SetDestination(hit.point);
                }
                else
                {
                    transform.position = new Vector3(hit.point.x, 4.119328f, hit.point.z);
                    agent.enabled = true;
                    agent.SetDestination(transform.position);
                }
                


                StartCoroutine(AbilityCooldown(1, 10f)); // Cooldown for 10 seconds

            }
            defenseButtonClicked = false;
            buttonCliked = false;

        }
        else if(tag == "Barbarian")
        {
            audioManager.PlaySFX(audioManager.Shield_Activated);
           activeShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
           activeShield.transform.parent = transform;
           StartCoroutine(AbilityCooldown(1, 10f));
           StartCoroutine(DeactivateShieldAfterTime(3f));

            
        }


    }
    void UltimateAttack(Vector3 pos)
    {


        if (tag == "Sorcerer")
        {


          
                Vector3 spawnPosition = pos;
                if (!isLevel1)
                    spawnPosition.y += 5.0f;
                audioManager.PlaySFX(audioManager.Inferno_Activated);
                GameObject infernoInstance =  Instantiate(infernoPrefab, spawnPosition, Quaternion.identity);
                StartCoroutine(AbilityCooldown(2, 15f));
                Destroy(infernoInstance, 5f);

            
        }
        else if(tag == "Barbarian")
        {
            animator.SetBool("isSprint", true);
            agent.SetDestination(pos);

            StartCoroutine(AbilityCooldown(2, 10f));

           // ChargeToPosition(pos);


        }
        ultimateButtonClicked = false;
        buttonCliked = false;
    }


    void WildAttack(Vector3 pos)
    {
        if (tag == "Sorcerer")
        {
            audioManager.PlaySFX(audioManager.Clone_Activated);
            GameObject clone = new GameObject();
            if (isLevel1)
                clone = Instantiate(wizardClone, new Vector3(pos.x, 0, pos.z), Quaternion.identity);
            else
            {
                clone = Instantiate(wizardClone, new Vector3(pos.x, 4.119328f, pos.z), Quaternion.identity);
                clone.transform.localScale = new Vector3(1f, 1f, 1f); // Adjust the scale values as needed
            }
            Minion_Logic.wizardClone = clone;
            DemonLogic.wizardClone = clone;
            StartCoroutine(AbilityCooldown(3, 10f));
            wildButtonClicked = false;
            buttonCliked = false;
        }
        else if (tag == "Barbarian")
        {
            audioManager.PlaySFX(audioManager.Charging);
            animator.SetBool("Special_Attack", true);
            barAttacking = true;
            StartCoroutine(AbilityCooldown(3, 5f));
            StartCoroutine(ResetAfterBarbarianSpecialAttack());

        }

     
    }


    //private void ChargeToPosition(Vector3 targetPosition)
    //{
    //    // Set the movement speed for the charge (faster than normal movement)
    //    agent.speed = 2f; // Adjust speed for charging

    //    // Ensure the character is moving
    //    while (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetPosition.x, 0, targetPosition.z)) > 4f )

    //    {
    //        // Move towards the target position while checking for obstacles/enemies
    //        agent.SetDestination(targetPosition);

    //        // Check for collisions along the way
    //        //Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
    //        //foreach (var collider in hitColliders)
    //        //{
    //        //    if (collider.CompareTag("Enemy"))
    //        //    {
    //        //        // Damage the enemy or apply effects
    //        //        collider.GetComponent<Enemy>().TakeDamage(20); // Example damage to enemy
    //        //    }
    //        //    else if (collider.CompareTag("DestructibleObject"))
    //        //    {
    //        //        // Destroy any destructible objects in the way
    //        //        Destroy(collider.gameObject);
    //        //    }
    //        //}
    //        if (agent.pathStatus == NavMeshPathStatus.PathInvalid || agent.remainingDistance <= agent.stoppingDistance)
    //        {
    //            animator.SetBool("isSprint", false); // Stop sprint animation
    //            break; // Exit the loop as the agent can't reach the position
    //        }
    //        // Wait for a frame before checking again
    //    }

    //    // End the charge and stop the running animation
    //    animator.SetBool("isSprint", false);

    //    // Reset speed after charge

     
    //}




    IEnumerator ResetAfterAttack()
    {

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;

    }


    IEnumerator DeactivateShieldAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Destroy the shield GameObject
        Destroy(activeShield);

        // Explicitly set the reference to null
        activeShield = null;
    }

    IEnumerator ResetAfterBarbarianAttack()
    {
        yield return new WaitForSeconds(1.9f); // Wait for the animation duration
        animator.SetBool("Attack", false); // Reset the attack animation state
        barAttacking = false;
    }

    IEnumerator ResetAfterBarbarianSpecialAttack()
    {
        yield return new WaitForSeconds(1.9f); // Wait for the animation duration
        animator.SetBool("Special_Attack", false); // Reset the attack animation state
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

        if (abilityPointsText != null)
        {
            abilityPointsText.text = $"{abilityPoints:F0}";
        }


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
        if(tag == "Barbarian" && activeShield != null)
        {
            return;
        }
        playerCurrenttHealth -= damage;
        audioManager.PlaySFX(audioManager.Wanderer_Damaged);
        if(playerCurrenttHealth <= 0){
            playerCurrenttHealth = 0;
           animator.Play("dead");
           audioManager.PlaySFX(audioManager.Wanderer_Dies);
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
            audioManager.PlaySFX(audioManager.Item_Picked_Up);
            Destroy(collision.gameObject);
            OpenPortal();
        }

        if (collision.gameObject.tag == "portal")
        {
            SceneManager.LoadScene("Level2_scene");
        }
    }
    //if(collision.gameObject.tag == "Summoned_Minions" && animator.GetBool("isSprint") )
    // {
    //        Debug.Log("Collision");
    //        GameObject minion = collision.gameObject;
    //        minion.GetComponent<Minion_Logic>().Die();
    //  }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Summoned_Minions" && animator.GetBool("isSprint"))
        {
            Debug.Log("Collision");
            GameObject minion = other.gameObject;
            audioManager.PlaySFX(audioManager.Enemy_Dies);
            minion.GetComponent<Minion_Logic>().Die();
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
    public void AddExp(int expToAdd)
    {
        exp += expToAdd;
    }
}
