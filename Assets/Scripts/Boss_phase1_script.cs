using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_phase1_script : MonoBehaviour
{
    public static bool fightStarted = true;
    public GameObject player;
    public GameObject minions;
    Animator anim;
    bool initialSummon = true;
    float summonDelay = 10f;
    float ogSummonDelay = 0;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ogSummonDelay = summonDelay;
       
    }

    // Update is called once per frame
    void Update()
    {
        // look at player
        transform.LookAt(player.transform);

        if (fightStarted){

            if (canISummon())
            {
                if (initialSummon)
                {
                    initialSummon = false;
                    summon();
                }
                else
                {
                    summonDelay -= Time.deltaTime;
                    if (summonDelay <= 0)
                    {
                        summon();
                        summonDelay = ogSummonDelay;
                    }
                }
                
            }
            else
            {
                //anim.Play("Dive bommb");
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
        // anim.Play("Summoning");
        anim.ResetTrigger("summon");
        anim.SetTrigger("summon");
        // int numberOfMinions = Random.Range(1, 4);
        int numberOfMinions = 3;
        for (int i = 0; i < numberOfMinions; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y, transform.position.z + Random.Range(-5, 5));
            Instantiate(minions, pos, Quaternion.identity);
        }
    }
}
