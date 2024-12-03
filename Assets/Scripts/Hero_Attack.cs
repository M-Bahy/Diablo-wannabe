using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Attack : MonoBehaviour
{

    private float attackRange = 1.0f; 
    private string enemyTag = "Enemy"; 
    private Animator animator;
    private bool[] abilitiesUnlocked;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        abilitiesUnlocked = HUD_Script.abilitiesUnlocked;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isAttacking) 
        {
            BasicAttack();
        }
    }


    void BasicAttack()
    {


        isAttacking = true;

        animator.Play("attack_short_001", 0, 0f);


        StartCoroutine(ResetAfterAttack());



    }


    IEnumerator ResetAfterAttack()
    {
        // Wait until the attack animation is done
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Set isAttacking to false once the animation is finished
        isAttacking = false;

        // Optionally, you can explicitly set the state to "Idle" here if needed
        // animator.Play("Idle");
    }

}
