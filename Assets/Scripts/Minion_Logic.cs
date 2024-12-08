using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

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
}