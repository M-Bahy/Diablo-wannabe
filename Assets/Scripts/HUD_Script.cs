using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HUD_Script : MonoBehaviour
{
    private Animator playerAnimator;
    private string Tag;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        // get the tag of the componenet
        Tag = gameObject.tag;

    }

    public void basicAbility()
    {
        if (Tag == "Sorcerer")

        {
            playerAnimator.Play("dead");
        }
        else if (Tag == "Barbarian")
        {
            playerAnimator.SetTrigger("BasicAttack");
        }


    }
}