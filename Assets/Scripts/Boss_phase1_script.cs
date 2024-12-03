using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_phase1_script : MonoBehaviour
{
    public static bool fightStarted = true;
    public GameObject player;
    public GameObject minions;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
       
    }

    // Update is called once per frame
    void Update()
    {
        // look at player
        transform.LookAt(player.transform);

        if (fightStarted){

            if (canISummon())
            {
                summon();
            }
            else
            {
                anim.Play("Dive bommb");
            }

        }

    }

    bool canISummon()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Summoned_Minions");
        return enemies.Length == 0;
    }

    public void summon()
    {
        anim.Play("Summoning");

        int numberOfMinions = Random.Range(1, 4);
        for (int i = 0; i < numberOfMinions; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y, transform.position.z + Random.Range(-5, 5));
            Instantiate(minions, pos, Quaternion.identity);
        }
    }
}
