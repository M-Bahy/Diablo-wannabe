using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Minion_Logic : MonoBehaviour
{
    public bool isDead = false;
    int minionMaxHealth = 20;
    int minionCurrentHealth = 20;

    private bool isAggro = false;

    public GameObject player;
    NavMeshAgent agent;
    private Vector3 startingPos;

    private Animator animator;

    public Slider healthSlider;

    private bool isInInferno = false;
    public static GameObject wizardClone;
    AudioManagerScript audioManager;

    private bool isAttacking = false; // To track if the minion is attacking
    private float attackCooldown = 3.0f; // Cooldown between attacks

    private void Awake()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManagerScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startingPos = transform.position;
        animator = GetComponent<Animator>();

        healthSlider.maxValue = minionMaxHealth;
        healthSlider.value = minionCurrentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerMechanics.isLevel1)
        {
            BossMech boss = GameObject.Find("Tortoise_Boss_Anims").GetComponent<BossMech>();
            if (boss.gameOver)
            {
                agent.isStopped = true;
                return;
            }
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            
            if (isAggro)
            {
                if (wizardClone == null)
                    agent.SetDestination(player.transform.position);
                else
                    agent.SetDestination(wizardClone.transform.position);

                StartCoroutine(delay(0.2f));
               
                animator.SetBool("isWalking", false);
            }
            else
            {
                if (agent.remainingDistance > agent.stoppingDistance + 0.1f)
                {
                    agent.SetDestination(startingPos);
                }
                else
                    animator.SetBool("isWalking", false);
            }
        }
        else
        {
            if (isAggro)
            {
                if (wizardClone == null)
                    agent.SetDestination(player.transform.position);
                else
                    agent.SetDestination(wizardClone.transform.position);
            }
            else
            {
                if (agent.remainingDistance > 0.1f)
                {
                    agent.SetDestination(startingPos);
                }
            }
            animator.SetBool("isWalking", true);
        }

        


    }
    IEnumerator delay(float time)
    {
        yield return new WaitForSeconds(time);
        if (!isAttacking && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(AttackPlayer());
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        animator.SetBool("Hit", true);
        minionCurrentHealth -= damage;
        if (healthSlider.value != minionCurrentHealth)
        {
            healthSlider.value = minionCurrentHealth;

        }

        if (minionCurrentHealth <= 0)
        {
            minionCurrentHealth = 0;
            Die();
        }
    }

    public void Die()
    {
        audioManager.PlaySFX(audioManager.Enemy_Dies);
        animator.SetBool("Hit", false);
        animator.SetBool("Death", true);
        isDead = true;
        StartCoroutine(WaitAndDestroy(1.4f)); // Wait for 1.4 seconds
    }

    private IEnumerator WaitAndDestroy(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        player.GetComponent<PlayerMechanics>().AddExp(10);
        Destroy(gameObject); // Destroy the GameObject
    }

    public void goAggresive(bool yes)
    {
        // Move towards the player
        isAggro = yes;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Inferno") && !isInInferno)
        {
            TakeDamage(10); // Initial damage
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
            TakeDamage(damage); // Apply periodic damage
        }
    }
    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        ///////////////////// code for looking at player before damaging
        // Calculate the target direction, keeping rotation limited to the Y-axis
        Vector3 direction = player.transform.position - transform.position;
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


        yield return new WaitForSeconds(0.7f); // Delay to sync with attack animation

        if (!isDead && isAggro && agent.remainingDistance <= agent.stoppingDistance) // Ensure the minion is still alive and aggressive
        {
            player.GetComponent<PlayerMechanics>().takeDamage(5); // Damage the player (adjust value as needed)
        }

        animator.SetBool("isAttacking", false);

        yield return new WaitForSeconds(attackCooldown); // Wait for cooldown
        isAttacking = false; // Allow attacking again
    }
}