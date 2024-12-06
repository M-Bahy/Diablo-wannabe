using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class s_m : MonoBehaviour
{
    NavMeshAgent agent;
    public static GameObject player;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        BossMech boss = GameObject.Find("Tortoise_Boss_Anims").GetComponent<BossMech>();
        if(boss.gameOver){
            agent.isStopped = true;
            return;
        }
        agent.SetDestination(player.transform.position);
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    
}
