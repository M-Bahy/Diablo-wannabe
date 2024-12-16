using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;


public class PlayerMechanics : MonoBehaviour
{
    /////////////   
    public static bool isAttacking = false;
    public static bool barAttacking = false;
    public static bool circleAttacking = false;
    private bool buttonCliked = false;
    private bool defenseButtonClicked = false;
    private string tag;
    private NavMeshAgent agent;
    private bool ultimateButtonClicked = false;
    public GameObject infernoPrefab;
    private bool wildButtonClicked = false;
    private GameObject activeShield;
    public GameObject shieldPrefab;
    public GameObject axePrefab;
    public static GameObject minion;
    public static bool canHitSpecial = false;
    public  NavMeshSurface NavMeshSurface;
    public GameObject bossEmptyObj;
    public GameObject Boss;
    private bool noDamageCheat = false;
    private bool isSlowMotion = false; // Tracks the state of slow motion
    private bool noShieldNow = false;
    //////////////

    private bool isDead = false;

    public static int level = 1;
    public static int exp = 0;
    public static int requiredExp = 100;
    public static int playerMaxHealth = 100;
    public static int playerCurrenttHealth = 100;
    public static int numberOfHealingPortions = 0;
    public static int abilityPoints = 0;



    public static int numberOfFragments = 0;
    private Animator animator;
    private Collider collider;
    public static bool isLevel1 = true;



    public TMP_Text healthText ;
    public Slider healthSlider ; 

    public TMP_Text expText ;
    public Slider expSlider ; 

    public TMP_Text levelText ;

    [SerializeField] TMP_Text abilityPointsText;
    [SerializeField] TMP_Text fragmentText;

    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject portal;
    public GameObject telepoertCircle;
    Rigidbody rb;


   [SerializeField] GameObject wizardClone;
    [SerializeField] Camera _maincamera;
    [SerializeField] GameObject Fireball;

    AudioManagerScript audioManager;
    public GameObject pausePanel;
    public Button restartButton;
    public Button mainMenuButton;
    public Button resumeButton;

    public bool inCover = false;

    bool isBossAttacked = false;
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
        rb = GetComponent<Rigidbody>();
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(MainMenu);
        resumeButton.onClick.AddListener(ResumeGame);
        pausePanel.SetActive(false);
        numberOfFragments = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(tag == "Sorcerer" && bossEmptyObj != null)
        {
            Destroy(bossEmptyObj);
            bossEmptyObj = null;
        }
        //Debug.Log(agent.remainingDistance);

        //if (tag == "Barbarian" && animator.GetBool("isSprint"))
        //{
        //    collider.isTrigger = true;
        //}
        //if (tag == "Barbarian" && Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(agent.destination.x, 0, agent.destination.z)) < 4f && animator.GetBool("isSprint"))

        //{
        //    //collider.isTrigger = false;
        //    //Vector3 newPosition = transform.position;
        //    // agent.SetDestination(newPosition);
        //    animator.SetBool("isSprint", false);
        //    //animator.SetBool("isWalking", true);
        //}
        //if(tag == "Barbarian" && transform.position.x >= 27 && transform.position.x <= 50 && transform.position.z >= 22 && transform.position.z <= 27)
        // {
        //     agent.SetDestination(agent.transform.position);
        //}
   
        if (!isLevel1)
        {
            BossMech boss = GameObject.Find("Tortoise_Boss_Anims").GetComponent<BossMech>();
            if(BossMech.gameOver){
                agent.isStopped = true;
                healthSlider.value = 0;
                healthText.text = "0";
            
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            //// Trigger the animation
            //if(numberOfHealingPortions>0 && playerCurrenttHealth != playerMaxHealth ){
            //    animator.Play("heal");
            //    numberOfHealingPortions -- ;


            //    playerCurrenttHealth += playerMaxHealth/2; 
            //    if(playerCurrenttHealth > playerMaxHealth){
            //        playerCurrenttHealth = playerMaxHealth;
            //    }
            //}
            if (isSlowMotion)
            {
                // Restore normal speed
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f; // Default value for Fixed Timestep
                isSlowMotion = false;
            }
            else
            {
                // Enable slow motion
                Time.timeScale = 0.5f; // Half-speed
                Time.fixedDeltaTime = 0.02f * Time.timeScale; // Adjust Fixed Timestep
                isSlowMotion = true;
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
            if(playerCurrenttHealth > playerMaxHealth){
                playerCurrenttHealth = playerMaxHealth;
            }
        }
        if (Input.GetKeyDown(KeyCode.D)){
            //playerCurrenttHealth -=20;
            takeDamage(20);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
           noDamageCheat = !noDamageCheat;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            
            for (int i = 0; i < HUD_Script.abilitiesCoolDown.Length; i++)
            {
                HUD_Script.abilitiesCoolDown[i] = false;
                HUD_Script.coolDownTimer[i] = 0;

            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            HUD_Script.abilitiesUnlocked[0] = true;
            HUD_Script.abilitiesUnlocked[1] = true;
            HUD_Script.abilitiesUnlocked[2] = true;
            HUD_Script.abilitiesUnlocked[3] = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape)){
            if(pausePanel.activeSelf){
                pausePanel.SetActive(false);
                if(isLevel1){
                    audioManager.PlayBackground(audioManager.Level1);
                }
                else{
                    audioManager.PlayBackground(audioManager.Level2);
                }
                Time.timeScale = 1;
            }else{
                pausePanel.SetActive(true);
                audioManager.PlayBackground(audioManager.Menus);
                Time.timeScale = 0;
            }
        }



        if (!buttonCliked && Input.GetMouseButtonDown(1) && !isAttacking && !HUD_Script.abilitiesCoolDown[0] )
        {
            //Debug.Log("Hit Achieved");
            if (tag == "Barbarian" &&  animator.GetBool("Attack"))
                return;
            if(tag == "Barbarian" && (animator.GetBool("Special_Attack") || animator.GetBool("isSprint")))
                return;
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
            isBossAttacked = false;
            noShieldNow = true;
        }


        if (tag == "Sorcerer" && Input.GetKeyDown(KeyCode.Q) && HUD_Script.abilitiesUnlocked[3] && !HUD_Script.abilitiesCoolDown[3] && !buttonCliked)
        {
            buttonCliked = true;
            wildButtonClicked = true;
        }
        if (tag == "Barbarian" && Input.GetKeyDown(KeyCode.W) && HUD_Script.abilitiesUnlocked[1] && !HUD_Script.abilitiesCoolDown[1] && activeShield == null && !noShieldNow)
        {
            DefensiveAttack();
        }
        if (tag == "Barbarian" && Input.GetKeyDown(KeyCode.Q) && HUD_Script.abilitiesUnlocked[3] && !HUD_Script.abilitiesCoolDown[3] && barAttacking == false && !buttonCliked)
        {
            WildAttack( new Vector3());
        }
        if (ultimateButtonClicked && Input.GetMouseButtonDown(1))
        {
            Ray ray = _maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                if (!isLevel1)
                {
                    Collider[] colliders = Physics.OverlapBox(hit.point, new Vector3(8, 8, 8)); // Adjust the sphere's position and radius as needed

                    bool bossDetected = false;

                    foreach (var collider in colliders)
                    {
                        if (collider.CompareTag("Boss")) // Replace "Boss" with the actual tag of your boss
                        {
                            bossDetected = true;
                            break;
                        }
                    }

                    if (bossDetected)
                        UltimateAttack(new Vector3(-2.236006f, 11.04607f, 9.111984f));

                    else

                        UltimateAttack(hit.point);
                }
                else
                {
                    UltimateAttack(hit.point);
                }

            }
            noShieldNow = false;

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

        if (tag == "Barbarian" && animator.GetBool("Special_Attack"))
            agent.SetDestination(agent.transform.position);
    }



    public void RestartGame() {
        Time.timeScale = 1;
        int level = PlayerMechanics.isLevel1 ? 1 : 2;
        Minion_Logic.isGameOver = false;
        BossMech.gameOver = false;
        if (level == 2)
        {
            PlayerMechanics.level = 4;
            PlayerMechanics.exp = 0;
            PlayerMechanics.playerMaxHealth = 400;
            PlayerMechanics.playerCurrenttHealth = 400;
            PlayerMechanics.numberOfHealingPortions = 0;
            PlayerMechanics.abilityPoints = 0;
            HUD_Script.abilitiesUnlocked = new bool[4];
            HUD_Script.abilitiesUnlocked[0] = true;
            HUD_Script.abilitiesUnlocked[1] = true;
            HUD_Script.abilitiesUnlocked[2] = true;
            HUD_Script.abilitiesUnlocked[3] = true;
            HUD_Script.ResetCoolDowns();
            Healing_Script.collectedHealingPotions.Clear();
        }
        else
        {
            PlayerMechanics.level = 1;
            PlayerMechanics.exp = 0;
            PlayerMechanics.playerMaxHealth = 100;
            PlayerMechanics.playerCurrenttHealth = 100;
            PlayerMechanics.numberOfHealingPortions = 0;
            PlayerMechanics.abilityPoints = 0;
            HUD_Script.abilitiesUnlocked = new bool[4];
            HUD_Script.abilitiesUnlocked[0] = true;
            HUD_Script.abilitiesUnlocked[1] = false;
            HUD_Script.abilitiesUnlocked[2] = false;
            HUD_Script.abilitiesUnlocked[3] = false;
            HUD_Script.ResetCoolDowns();
            Healing_Script.collectedHealingPotions.Clear();
            numberOfFragments = 0;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level" + level+"_scene");
    }

    public void MainMenu() {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu_Scene");
    }
     public void ResumeGame() {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }


    void BasicAttack(Vector3 pos)
    {
        //animator.ResetTrigger("Attack");
        if (tag == "Sorcerer")
        {
            StartCoroutine(SpawnFireballWithDelay(0.5f, pos));
        }
        else if(tag == "Barbarian")
        {
            Collider[] hitColliders;
            if (isLevel1)
            {
                hitColliders = Physics.OverlapSphere(pos, 2.0f);
            }
            else
            {
                hitColliders = Physics.OverlapSphere(pos, 5.0f); // Adjust radius as needed
            }
             // Adjust radius as needed
            bool enemyFound = false;
            Transform nearestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Summoned_Minions") || collider.CompareTag("Boss") || collider.CompareTag("Turtle_Stop") || collider.CompareTag("Minion") || collider.CompareTag("Demon"))
                {
                    float distance = Vector3.Distance(pos, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        nearestEnemy = collider.transform;
                        minion = collider.gameObject;
                       
                    }
                   
                }
            }
            //Debug.Log("Player Mechanics: " + minion);
            if (nearestEnemy!= null)
            {

                if (minion.CompareTag("Boss") || minion.CompareTag("Turtle_Stop"))
                {
                    damage_the_boss_script.barAttackedBoss = false;
                    StartCoroutine(MoveAndAttack(nearestEnemy, pos,"Boss"));
                }
                else
                {
                    StartCoroutine(MoveAndAttack(nearestEnemy, pos, "minion"));
                }
            }






        }
    }




    private IEnumerator MoveAndAttack(Transform nearestEnemy, Vector3 pos,String type)
    {
        float attackRange = 5.0f; // Define the attack range
        agent.updateRotation = true; // Allow rotation
        if(type == "Boss")
        {
            attackRange = 16.5f;
        }
        if (isLevel1)
        {
            attackRange = 1.2f;
        }
        Vector3 targetPosition = new Vector3(nearestEnemy.position.x, transform.position.y, nearestEnemy.position.z);
        float distanceToEnemy = Vector3.Distance(transform.position, targetPosition);
        if (distanceToEnemy < 30f)
        {

            while (true)
            {




                if (Input.GetMouseButtonDown(0))
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        yield return null;
                        continue;

                    }
                    Ray ray = _maincamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (PlayerMechanics.isLevel1)
                            agent.SetDestination(new Vector3(hit.point.x, 0, hit.point.z));
                        else
                            agent.SetDestination(new Vector3(hit.point.x, hit.point.y, hit.point.z));
                    }
                    yield break;
                }







                // Update the target position based on the enemy's current position
                targetPosition = new Vector3(nearestEnemy.position.x, transform.position.y, nearestEnemy.position.z);
                distanceToEnemy = Vector3.Distance(transform.position, targetPosition);

                // Set destination to the enemy's updated position
                agent.SetDestination(targetPosition);
                //  Debug.Log("Distance to enemy: " + distanceToEnemy);
                // If within attack range, stop moving and attack
                if (distanceToEnemy <= attackRange)
                {
                    if(type == "Boss")
                    {
                        agent.SetDestination(transform.position);
                    }

                    break;
                }

                yield return null; // Wait for the next frame
            }

            // Once within range, stop moving and perform the attack
            agent.updateRotation = false; // Stop NavMeshAgent rotation

            // Rotate the Barbarian to face the enemy
            Vector3 attackDirection = (nearestEnemy.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(attackDirection.x, 0, attackDirection.z));
            transform.rotation = targetRotation;

            // Trigger the attack animation
            animator.SetBool("Attack", true);
            barAttacking = true;
            agent.SetDestination(transform.position);
            // Start the attack cooldown and reset
            StartCoroutine(ResetAfterBarbarianAttack());
        }
        
    }
    private IEnumerator SpawnFireballWithDelay(float delay, Vector3 pos)
    {
        // Check if there's an enemy at the specified position
        Collider[] hitColliders = Physics.OverlapBox(pos, new Vector3(5.0f, 5.0f, 5.0f)); // Adjust radius as needed
        bool enemyFound = false;
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Minion") || collider.CompareTag("Demon") || collider.CompareTag("Summoned_Minions") || collider.CompareTag("Boss"))
            {
                enemyFound = true;
                break;
            }
        }

        if (enemyFound)
        {
            HUD_Script.abilitiesCoolDown[0] = true;
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
            audioManager.PlaySFX(audioManager.Fireball_Shot);
            yield return new WaitForSeconds(delay);

            //transform.LookAt(pos); LINE BEING REPLACED
            //rb.isKinematic = true;
            GameObject fireballInstance = new GameObject();
            Vector3 offset = transform.forward * 0.5f; // Offset 0.5 units in the forward direction
            Vector3 spawnPosition = transform.position + offset;

            if (isLevel1)
            {
                fireballInstance = Instantiate(Fireball, spawnPosition, transform.rotation); // Use the object's rotation for the fireball
            }
            else
            {
                fireballInstance = Instantiate(Fireball, spawnPosition + Vector3.up * 7, transform.rotation); // Add height offset for level 2
                fireballInstance.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); // Adjust the scale values as needed
            }

            FbScript fireballLogic = fireballInstance.GetComponent<FbScript>();
            if (fireballLogic != null)
            {
                fireballLogic.SetTarget(pos);
            }
            yield return new WaitForSeconds(delay-0.1f);
            //rb.isKinematic = false;
            animator.SetBool("isAttacking", false);
            StartCoroutine(AbilityCooldown(0, 1f)); // Cooldown for 1 second
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
           StartCoroutine(DeactivateShieldAfterTime(3f));

            
        }


    }
    void UltimateAttack(Vector3 pos)
    {


        if (tag == "Sorcerer")
        {


          
                Vector3 spawnPosition = new Vector3 (pos.x, 0, pos.z);
                if (!isLevel1)
                    spawnPosition.y = 5.0f;
                audioManager.PlaySFX(audioManager.Inferno_Activated);
                GameObject infernoInstance =  Instantiate(infernoPrefab, spawnPosition, Quaternion.identity);
                StartCoroutine(AbilityCooldown(2, 15f));
                Destroy(infernoInstance, 5f);
            ultimateButtonClicked = false;
            buttonCliked = false;

        }
        else if(tag == "Barbarian")
        {
            ultimateButtonClicked = false;

            animator.SetBool("isSprint", true);
            //agent.enabled = false;
            StartCoroutine(MoveInStraightLine(pos));
            //agent.enabled = true;
            //





        }
     
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
            damage_the_boss_script.specialAttackedBoss = false;
            Axe_Script.affectedMinions = new List<GameObject>();

            circleAttacking = true;
            if(rb != null)
            {
                Destroy(rb);
            }
            StartCoroutine(ResetAfterBarbarianSpecialAttack());


        }

     
    }


  



    private IEnumerator MoveInStraightLine(Vector3 targetPosition)
    {
        audioManager.PlaySFX(audioManager.Charging);
        //targetPosition.y = transform.position.y;
        //Debug.Log("position: "+ transform.position.y);
        float speed = 15f; // Adjust movement speed
        Vector3 startPosition = transform.position;
        Vector3 direction = (targetPosition - startPosition).normalized; // Get the straight-line direction
        float timeElapsed = 0f; // Track the elapsed time
        float timeLimit = 3.0f;
        //direction.y = transform.position.y;
        while (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                          new Vector3(transform.position.x+15f, 0, transform.position.z+15f)) > 0.5f) // Check distance ignoring y
        {
            // Move toward the target position
            transform.position += direction * speed * Time.deltaTime;
            agent.SetDestination(agent.transform.position);
            // Keep facing the target position
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            timeElapsed += Time.deltaTime;

            // If the time exceeds the limit, stop the movement
            if (timeElapsed >= timeLimit)
            {
                animator.SetBool("isSprint", false); // Stop the sprinting animation
                agent.SetDestination(transform.position);
                //animator.SetBool("isSprint", false);
                StartCoroutine(AbilityCooldown(2, 10f));
                buttonCliked = false;
                yield break; // Exit the coroutine
            }


            yield return null;
        }
        agent.SetDestination(transform.position);
        animator.SetBool("isSprint", false);
        StartCoroutine(AbilityCooldown(2, 10f));
    }





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
        StartCoroutine(AbilityCooldown(1, 10f));

        // Explicitly set the reference to null
        activeShield = null;
    }

    IEnumerator ResetAfterBarbarianAttack()
    {
        yield return new WaitForSeconds(1.3f); // Wait for the animation duration
        animator.SetBool("Attack", false); // Reset the attack animation state
        barAttacking = false;
        agent.updateRotation = true;
        StartCoroutine(AbilityCooldown(0, 1f));
    }

    IEnumerator ResetAfterBarbarianSpecialAttack()
    {
        yield return new WaitForSeconds(3.167f); // Wait for the animation duration
        animator.SetBool("Special_Attack", false); // Reset the attack animation state
        circleAttacking = false;
        StartCoroutine(AbilityCooldown(3, 5f));
        // agent.SetDestination(agent.transform.position);
        rb = gameObject.AddComponent<Rigidbody>();
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
        if (MainMenu_Script.isWizard)
        {
            if (abilityIndex == 2 || abilityIndex == 3)
            {
                HUD_Script.abilitiesCoolDown[abilityIndex] = true;
                yield return new WaitForSeconds(5f);
                HUD_Script.coolDownTimer[abilityIndex] = cooldownTime;

                // Wait for the cooldown duration
                yield return new WaitForSeconds(cooldownTime);

                // Set the ability cooldown back to false
                HUD_Script.abilitiesCoolDown[abilityIndex] = false;
            }
            else
            {
                // Set the ability cooldown to true
                HUD_Script.abilitiesCoolDown[abilityIndex] = true;
                HUD_Script.coolDownTimer[abilityIndex] = cooldownTime;

                // Wait for the cooldown duration
                yield return new WaitForSeconds(cooldownTime);

                // Set the ability cooldown back to false
                HUD_Script.abilitiesCoolDown[abilityIndex] = false;
            }
            
        }
        else
        {
            // Set the ability cooldown to true
            HUD_Script.abilitiesCoolDown[abilityIndex] = true;
            HUD_Script.coolDownTimer[abilityIndex] = cooldownTime;

            // Wait for the cooldown duration
            yield return new WaitForSeconds(cooldownTime);

            // Set the ability cooldown back to false
            HUD_Script.abilitiesCoolDown[abilityIndex] = false;
        }
        
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

        if (fragmentText != null)
        {
            fragmentText.text = "X " + $"{numberOfFragments:F0}";
        }


    }
    private void levelUp(){

        level += 1;
        abilityPoints++;
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
        if (noDamageCheat)
            return;
        if(tag == "Barbarian" && activeShield != null)
        {
            return;
        }
    
        if(playerCurrenttHealth >0)
            playerCurrenttHealth -= damage;
        audioManager.PlaySFX(audioManager.Wanderer_Damaged);
        if(playerCurrenttHealth <= 0 && !isDead){
            playerCurrenttHealth = 0;
            updateHUDUI();
            isDead = true;
            Minion_Logic.isGameOver = true;
            playerCurrenttHealth = 0;
           animator.Play("dead");
           audioManager.PlaySFX(audioManager.Wanderer_Dies);
           // here we should display the end game screen
           if(!isLevel1){
           BossMech boss = GameObject.Find("Tortoise_Boss_Anims").GetComponent<BossMech>();
           BossMech.gameOver = true;
           }
            // UnityEngine.SceneManagement.SceneManager.LoadScene("Game_Over_Scene");
            StartCoroutine(GameOverWithDelay(5f));
        }else{
            if(tag == "Sorcerer")
                 animator.Play("damage");
            else
            {
                if (tag == "Barbarian" && (animator.GetBool("Attack") || animator.GetBool("Special_Attack") || animator.GetBool("isSprint")))
                    return;
                else
                {
                    animator.SetBool("isHit", true);
                    // delay
                    StartCoroutine(ResetAfterHit());
                }
            }
            //
        }
    }
    private IEnumerator ResetAfterHit()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("isHit", false);
    }

    private IEnumerator GameOverWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        SceneManager.LoadScene("Game_Over_Scene"); // Load the Game Over scene
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
            HUD_Script.ResetCoolDowns();
            SceneManager.LoadScene("Level2_scene");
        }


        //if (collision.gameObject.tag == "Summoned_Minions" && animator.GetBool("isSprint"))
        //{
        //    //Debug.Log("Collision");
        //    GameObject minion = collision.gameObject;
        //    audioManager.PlaySFX(audioManager.Enemy_Dies);
        //    minion.GetComponent<Minion_Logic>().Die();
        //}
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
        if (tag == "Barbarian" &&(other.gameObject.tag == "Summoned_Minions" || other.gameObject.tag =="Minion") && animator.GetBool("isSprint"))
        {
           // Debug.Log("Collision");
            GameObject minion = other.gameObject;
            audioManager.PlaySFX(audioManager.Enemy_Dies);
            minion.GetComponent<Minion_Logic>().Die();
        }
        if (tag == "Barbarian" && other.gameObject.tag == "Turtle_Stop" && animator.GetBool("isSprint") && !isBossAttacked)
        {
            agent.SetDestination(agent.transform.position);
            Boss.GetComponent<BossMech>().damageBoss(20);
            isBossAttacked = true;

        }
        if (tag =="Barbarian" && other.gameObject.tag == "Demon" && animator.GetBool("isSprint"))
        {
            GameObject Demon = other.gameObject;
            audioManager.PlaySFX(audioManager.Enemy_Dies);
            Demon.GetComponent<DemonLogic>().Die();
        }
        if (other.gameObject.tag == "Fragment")
        {
            numberOfFragments++;
            audioManager.PlaySFX(audioManager.Item_Picked_Up);
            Destroy(other.gameObject);
            OpenPortal();
        }

        if (other.gameObject.tag == "portal")
        {
            HUD_Script.ResetCoolDowns();
            SceneManager.LoadScene("Level2_scene");
        }
        if(tag=="Barbarian"&& other.gameObject.tag == "Wall" && animator.GetBool("isSprint"))
        {
           // NavMeshObstacle obstacle = other.gameObject.GetComponent<NavMeshObstacle>();
            //if (obstacle != null)
            //{
            //    Debug.Log("Wall has NavMeshObstacle. Preparing to destroy...");
            //}
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
           // agent.SetDestination(agent.destination);
            // rebake the navmesh surface
            NavMeshSurface.BuildNavMesh();

        }
        if(other.gameObject.tag == "Cover")
        {
            inCover = true;
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

    private void OnTriggerStay(Collider other) {
        if (tag == "Barbarian" && other.gameObject.tag == "Turtle_Stop" && animator.GetBool("isSprint") && !isBossAttacked)
        {
            agent.SetDestination(agent.transform.position);
            Boss.GetComponent<BossMech>().damageBoss(20);
            isBossAttacked = true;

        }
        if(other.gameObject.tag == "Cover")
        {
            inCover = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Cover")
        {
            inCover = false;
        }
    }
 
}
