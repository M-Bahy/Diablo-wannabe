using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Boss_phase1_script : MonoBehaviour
{
    public static bool fightStarted = false;
    public static GameObject player;
    public GameObject Wizard;
    public GameObject Barbarian;
    public GameObject minionPrefab;
    Animator anim;
    bool initialSummon = true;
    bool initialCast = true;
    float summonDelay = 10f;
    float ogSummonDelay = 0;
    float castDelay = 10f;
    float ogCastDelay = 0;
    float diveBoomDelay = 5f;
    float ogDiveBoomDelay = 0;
    float spikesDelay = 5f;
    float ogSpikesBoomDelay = 0;
    public NavMeshSurface surface;
    GameObject [] minions = new GameObject[3];

    AudioManagerScript audioManager;

    private void Awake() {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManagerScript>();
        PlayerMechanics.isLevel1 = false;
        SetPlayer();
        DistributeThePlayer();
        fightStarted = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ogSummonDelay = summonDelay;
        ogDiveBoomDelay = diveBoomDelay;
       // PlayerMechanics playerMechanics = player.GetComponent<PlayerMechanics>();
        ogCastDelay = castDelay;
        ogSpikesBoomDelay = spikesDelay;
        audioManager.PlayBackground(audioManager.Level2);

    }

    // Update is called once per frame
    void Update()
    {
       BossMech bm = GetComponent<BossMech>();
        if (bm.gameOver )
        {
            return;
        }
        transform.LookAt(player.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        if (fightStarted){
            
            if(bm.phaseOne){
                if (isAllMinionsDead())
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
                diveBoomDelay -= Time.deltaTime;
                if (diveBoomDelay <= 0)
                {
                    anim.ResetTrigger("dive_boomb");
                    anim.SetTrigger("dive_boomb");
                    audioManager.PlaySFX(audioManager.Boss_Stomps_Down);
                    diveBoomDelay = ogDiveBoomDelay;
                }
            }
            else{
                // phase 2
                if (!bm.auraActivated){
                    if (false)
                    {
                        // initialCast = false;
                        // castAura();
                        // bm.activateAura();
                    }
                    else
                    {
                        castDelay -= Time.deltaTime;
                        if (castDelay <= 0)
                        {
                            castAura();
                            bm.activateAura();
                            castDelay = ogCastDelay;
                        }
                    }
                }
                if (!bm.auraActivated){
                    spikesDelay -= Time.deltaTime;
                    if (spikesDelay <= 0)
                    {
                        bloodSpikes();
                        spikesDelay = ogSpikesBoomDelay;
                    }
                }

            
        }

    }
    
    }

    public bool isAllMinionsDead()
    {
        foreach (GameObject minion in minions)
        {
            if (minion != null)
            {
                return false;
            }
        }
        return true;
    }

    public void summon()
    {
        audioManager.PlaySFX(audioManager.Boss_Summons_Minions);
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
            GameObject minion = Instantiate(minionPrefab, pos, Quaternion.identity);
            Minion_Logic m_l = minion.GetComponent<Minion_Logic>();
            m_l.player = player;
            m_l?.goAggresive(true);
            minions[i] = minion;
            //s_m.player = player;
            // surface.BuildNavMesh();
        }
    }

    public void castAura()
    {
        audioManager.PlaySFX(audioManager.Boss_Casts_Spell);
        // Cast_Spell
        anim.ResetTrigger("Cast_Spell");
        anim.SetTrigger("Cast_Spell");
    }

    public void bloodSpikes()
    {
        audioManager.PlaySFX(audioManager.Boss_Swings_Hands);
        // Swinging_Hands
        anim.ResetTrigger("Swinging_Hands");
        anim.SetTrigger("Swinging_Hands");
    }

    public void SetPlayer()
    {
        if(MainMenu_Script.isWizard){
            player = Wizard;
            Destroy(Barbarian);
        }
        else{
            player = Barbarian;
            Destroy(Wizard);
        }
    }
    public void DistributeThePlayer(){
        HUD_Script.player = player;
        Camera_Movement.player = player;   
    }
}
