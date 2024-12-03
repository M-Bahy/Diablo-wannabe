using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hero_Attack : MonoBehaviour
{

    private Animator animator;
    private bool isAttacking = false;
    private bool buttonCliked = false;
    private bool defenseButtonClicked = false;
    private string tag;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
   
     
        animator = GetComponent<Animator>();
        // get tag of the gameobject
        tag = gameObject.tag;
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
       

        if (!buttonCliked && Input.GetMouseButtonDown(1) && !isAttacking)
        {
            BasicAttack();
        }

        if (Input.GetKeyDown(KeyCode.W) && HUD_Script.abilitiesUnlocked[1])
        {
            buttonCliked = true;
            defenseButtonClicked = true;
        }
        if(defenseButtonClicked && Input.GetMouseButtonDown(1))
        {

            DefensiveAttack();

        }

    }


    void BasicAttack()
    {

        if (tag == "Sorcerer")
        {
            isAttacking = true;
            animator.Play("attack_short_001", 0, 0f);
            StartCoroutine(ResetAfterAttack());
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
                agent.SetDestination(hit.point);
                transform.position = hit.point;

            }
        
        }

        defenseButtonClicked = false;
        buttonCliked = false;
    }


    IEnumerator ResetAfterAttack()
    {
       
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;

    }
    IEnumerator TeleportAfterDelay(Vector3 targetPosition)
    {
        // Wait for 2 seconds before teleporting
        yield return new WaitForSeconds(2f);

        // Teleport to the target position
        transform.position = targetPosition;
    }













}


