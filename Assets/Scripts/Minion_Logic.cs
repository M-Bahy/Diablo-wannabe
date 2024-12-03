using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private bool firstTimeRun = true;
    private bool firstTimeIdle = true;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startingPos = transform.position;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAggro)
        {
            agent.SetDestination(player.transform.position);
            if (firstTimeRun)
            {
                animator.Play("Run");
                firstTimeRun = false;
            }
        }
        else
        {
            if (agent.remainingDistance > 0.1f)
            {
                agent.SetDestination(startingPos);
            }
        }


        if (agent.remainingDistance <= 2.0f && firstTimeIdle)
        {
            animator.Play("Idle");
            firstTimeRun = true;
            firstTimeIdle = false;
        }


    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        minionCurrentHealth -= damage;
        if (minionCurrentHealth <= 0)
        {
            minionCurrentHealth = 0;
            Die();
        }

        //// Update the health bar
        //if (healthBarSlider != null)
        //{
        //    healthBarSlider.value = (float)minionCurrentHealth / minionMaxHealth;
        //}
    }

    private void Die()
    {
        isDead = true;

        //// Destroy the health bar
        //if (healthBarSlider != null)
        //{
        //    Destroy(healthBarSlider.gameObject);
        //}

        // Destroy the minion
        Destroy(gameObject);
    }
    public void goAggresive(bool yes)
    {
        // Move towards the player
        isAggro = yes;
    }
}