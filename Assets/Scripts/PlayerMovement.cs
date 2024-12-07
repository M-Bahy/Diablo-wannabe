using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Vector3 pos ;
    [SerializeField] Camera _maincamera;

    private NavMeshAgent agent;
    float agentSpeed = 10f;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = agentSpeed;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerMechanics.isLevel1)
        {
            BossMech boss = GameObject.Find("Tortoise_Boss_Anims").GetComponent<BossMech>();
            if(boss.gameOver){
                agent.isStopped = true;
                return;
            }
        }
        // record the position where the player clicked to move the player
        // if(Input.GetMouseButton(0)){
        //     RaycastHit hit;
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     if(Physics.Raycast(ray, out hit )){
        //         pos = new Vector3(hit.point.x , hit.point.y , hit.point.z);
        //     }
        //     //calculate the angle to make rotation
        //     Quaternion lookAtNewPos = Quaternion.LookRotation(pos - transform.position );
        //     lookAtNewPos.x = 0f;
        //     lookAtNewPos.z = 0f ; 
        //     transform.rotation = Quaternion.Slerp(transform.rotation , lookAtNewPos , Time.deltaTime * 10);
        //     movePlayer.SimpleMove(transform.forward);

        // }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }

        }
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(agent.destination.x, 0, agent.destination.z)) <= 1.5f)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }


    }
}
