using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DemonLogic : MonoBehaviour
{
    int demonMaxHealth = 40;
    int demonCurrentHealth = 40;  
    
    public bool isDead = false;
    public bool isAggro = false;

    public Slider healthSlider;

        
    private bool isInInferno = false;

    NavMeshAgent agent;
    private Vector3 startingPos;

    public static GameObject wizardClone;

    public GameObject player;

    private Animator animator;

    AudioManagerScript audioManager;

    public Rigidbody rb;

    bool isAttack = false;

    private void Awake() {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManagerScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startingPos = transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();


        healthSlider.maxValue = demonMaxHealth;
        healthSlider.value = demonMaxHealth;
    }

    public void damageDemon(int dmg){
        animator.SetBool("Hit", true);

        demonCurrentHealth -= dmg;


        if (healthSlider.value != demonCurrentHealth)
        {
            healthSlider.value = demonCurrentHealth;

        }
        if(demonCurrentHealth <= 0){
            demonCurrentHealth = 0;
            isDead = true;
            Die();
        }
    }

    private void Die()
    {
        audioManager.PlaySFX(audioManager.Enemy_Dies);
        animator.SetBool("Hit", false);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);;
        animator.SetBool("Death", true);
        StartCoroutine(WaitAndDestroy(4f));
    }

    public void goAggresive(bool yes)
    {
        // Move towards the player
        isAggro = yes;
    }
    private IEnumerator WaitAndDestroy(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        player.GetComponent<PlayerMechanics>().AddExp(30);
        Destroy(gameObject); // Destroy the GameObject
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Inferno") && !isInInferno)
        {
            damageDemon(10); // Initial damage
            StartCoroutine(InfernoDamageOverTime(2, 1f, other.gameObject)); // Periodic damage
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Inferno"))
        {
            isInInferno = false; // Stop periodic damage when exiting the inferno
        }
    }


        IEnumerator InfernoDamageOverTime(int damage, float interval, GameObject inferno)
    {
        isInInferno = true;
        while (isInInferno)
        {
            // Check if the Inferno GameObject is still valid
            if (inferno == null)
            {
                isInInferno = false; // Stop damage if the inferno is destroyed
                yield break;
            }

            yield return new WaitForSeconds(interval); // Wait for the specified interval
            damageDemon(damage); // Apply periodic damage
        }
    }

    private void demonAttackPlayer(){

        animator.SetBool("Idle" , true);
        StartCoroutine(AttackSwordOne());
    }
    IEnumerator AttackSwordOne()
    {
        yield return new WaitForSeconds(5f);
        animator.SetBool("swordOne" , true);
        yield return new WaitForSeconds(1);
        if (agent.remainingDistance <= agent.stoppingDistance){
            player.GetComponent<PlayerMechanics>().takeDamage(10);
        }
        StartCoroutine(AttackSwordTwo());

    }

    IEnumerator AttackSwordTwo()
    {
        yield return new WaitForSeconds(2);
        animator.SetBool("swordOne" , true);
        yield return new WaitForSeconds(1);
        if (agent.remainingDistance <= agent.stoppingDistance){
            player.GetComponent<PlayerMechanics>().takeDamage(10);
        }
        StartCoroutine(attackWithBomb());

    }
        IEnumerator attackWithBomb()
    {
        yield return new WaitForSeconds(2);
        animator.SetBool("attackBomb" , true);
        yield return new WaitForSeconds(1);
        if (agent.remainingDistance <= agent.stoppingDistance){
            player.GetComponent<PlayerMechanics>().takeDamage(15);
        }


        isAttack = false ; 

    }

  



    // Update is called once per frame
    void Update()
    {

    

            if (isAggro)
            {
                agent.enabled = true;

                if (wizardClone == null) 
                    agent.SetDestination(player.transform.position);
                else
                    agent.SetDestination(wizardClone.transform.position);

                

                if (agent.remainingDistance <= agent.stoppingDistance && agent.remainingDistance > 0 && !isAttack)
                {
                    //animator.SetBool("isWalking", false);
                    //animator.SetBool("Idle", true);
                    isAttack = true;
                    demonAttackPlayer();
                                

                }
                if (agent.remainingDistance > agent.stoppingDistance )
                {
                    animator.SetBool("Idle", false);
                }

                

               
            }
            else
            {
            // if (agent.remainingDistance > 0.1f)
            // {
            //     agent.SetDestination(startingPos);
            // }
            agent.enabled = false;
        }
        
    }
}
