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

    float diveBoomDelay = 5f;
    float ogDiveBoomDelay = 0;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ogSummonDelay = summonDelay;
        ogDiveBoomDelay = diveBoomDelay;
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
           // else
          //  {
                diveBoomDelay -= Time.deltaTime;
                if (diveBoomDelay <= 0)
                {
                    anim.ResetTrigger("dive_boomb");
                    anim.SetTrigger("dive_boomb");
                    diveBoomDelay = ogDiveBoomDelay;
                }
          //  }

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
            // left :
            // -17.5,-83.5
            // right :
            // 10.4,80.83
            // front :
            // -59.22 , -13.94
            // back :
            // 25.99 , 62.93
            bool left = Random.Range(0, 2) == 0;
            bool front = Random.Range(0, 2) == 0;
            float x = 0.0f;
            float z = 0.0f;
            if (left)
            {
                x = Random.Range(-17.5f, -83.5f);
            }
            else
            {
                x = Random.Range(10.4f, 80.83f);
            }
            if (front)
            {
                z = Random.Range(-59.22f, -13.94f);
            }
            else
            {
                z = Random.Range(25.99f, 62.93f);
            }
            Vector3 pos = new Vector3(x, transform.position.y, z);
            Instantiate(minions, pos, Quaternion.identity);
        }
    }
}
