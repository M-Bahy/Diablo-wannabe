using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Camp_Logic : MonoBehaviour
{
    float minX;
    float maxX;
    float minZ;
    float maxZ;
    float minionY = 0.176f;
    float demonY = 0.21f;

    [SerializeField] GameObject minion;
    [SerializeField] GameObject demon;
    [SerializeField] GameObject key;

    [SerializeField] GameObject player; // THIS IS NOT FINAL, WE NEED A WAY TO PASS THE PLAYER

    List<GameObject> minionsArray = new List<GameObject>();
    List<GameObject> demonsArray = new List<GameObject>();

    List<GameObject> aggroedMinions = new List<GameObject>();
    List<GameObject> aggroedDemons = new List<GameObject>();

    bool keyFlag = false;
    bool doneFlag = false;

    public Transform[] patrollPoints ; 
    int targetPoint ;

    float patrollSpeed ;

    AudioManagerScript audioManager;

    private void Awake() {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManagerScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        audioManager.PlayBackground(audioManager.Level1);
        minion.GetComponent<Minion_Logic>().player = player;
        demon.GetComponent<DemonLogic>().player = player;
        minX = this.transform.position.x - 20;
        minZ = this.transform.position.z - 20;
        maxX = this.transform.position.x + 20;
        maxZ = this.transform.position.z + 20;
        Healing_Script hs = player.GetComponent<Healing_Script>();
        hs.spawnHealingPotionsInCamps(minX, maxX, minZ, maxZ);

        int demonCount = Random.Range(1, 3);
        int minionCount = Random.Range(8, 11);
        GameObject tmp;

        targetPoint = 0;
        patrollSpeed = 4.0f ; 

        for (int i = 0; i < demonCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition(demonY);
            tmp = Instantiate(demon, randomPosition, Quaternion.identity);
            //tmp.GetComponent<DemonLogic>().player = player;
            demonsArray.Add(tmp);
        }

        for (int i = 0; i < minionCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition(minionY);
            tmp = Instantiate(minion, randomPosition, Quaternion.identity);
            //tmp.GetComponent<Minion_Logic>().player = player;
            minionsArray.Add(tmp);
        }

        StartCoroutine(CallReplaceDeadEntities());
    }

    Vector3 GetRandomPosition(float y)
    {
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        return new Vector3(randomX, y, randomZ);
    }

    // Update is called once per frame
    void Update()
    {
        if (!doneFlag)
        {
            // Optimized: Check if all minions and demons are dead
            keyFlag = minionsArray.All(m => m == null || m.GetComponent<Minion_Logic>().isDead) &&
                      demonsArray.All(d => d == null || d.GetComponent<DemonLogic>().isDead);

            if (keyFlag)
            {
                Instantiate(key, new Vector3((minX + maxX) / 2, 2.0f, (minZ + maxZ) / 2), Quaternion.identity);
                doneFlag = true;
            }
        }

        patroll();

    }

    private void patroll()
    {
         if(patrollPoints[targetPoint].position.x-0.05 <=demonsArray[0].transform.position.x  &&  demonsArray[0].transform.position.x <= patrollPoints[targetPoint].position.x+0.05 
         && patrollPoints[targetPoint].position.z-0.05 <=demonsArray[0].transform.position.z &&  demonsArray[0].transform.position.z <= patrollPoints[targetPoint].position.z+0.05 ){
        //if(patrollPoints[targetPoint].position ==demonsArray[0].transform.position  && patrollPoints[targetPoint].position == demonsArray[0].transform.position ){
             increaseTargetInt();
        }
        demonsArray[0].transform.position = Vector3.MoveTowards(demonsArray[0].transform.position , patrollPoints[targetPoint].position , patrollSpeed*Time.deltaTime);
    }

    void increaseTargetInt(){
        
        targetPoint++;
        // Debug.Log("targetPoint " + targetPoint);
        // Debug.Log("partorll points length  " + patrollPoints.Length);

        if(targetPoint >= patrollPoints.Length ){
            targetPoint = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sorcerer") || other.CompareTag("Barbarian"))
        {
            aggroEnemies();
        }
        else if (other.CompareTag("Clone"))
        {
            // check if the player is within the camp, if yes then aggro the enemies in the aggroed to the clone, if no then deaggro them
            if (player.transform.position.x >= minX && player.transform.position.x <= maxX && player.transform.position.z >= minZ && player.transform.position.z <= maxZ)
            {
               

            }
            else
            {
                aggroEnemies();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sorcerer") || other.CompareTag("Barbarian"))
        {
            deAggroEnemies();
        }
        else if (other.CompareTag("Clone"))
        {
            // check if the player is within the camp coordinates, if yes then aggro the enemies in the aggroed list to him, if no then deaggro them
            if (player.transform.position.x >= minX && player.transform.position.x <= maxX && player.transform.position.z >= minZ && player.transform.position.z <= maxZ)
            {
                foreach (GameObject demon in aggroedDemons)
                {
                    if (demon == null) continue;
                    DemonLogic demonLogic = demon.GetComponent<DemonLogic>();
                    demonLogic.player = player;
                    demonLogic?.goAggresive(true);
                }

                foreach (GameObject minion in aggroedMinions)
                {
                    if (minion == null) continue;
                    Minion_Logic minionLogic = minion.GetComponent<Minion_Logic>();
                    minionLogic.player = player;
                    minionLogic?.goAggresive(true);
                }
            }
            else
            {
                deAggroEnemies();
            }
        }
    }

    private IEnumerator CallReplaceDeadEntities()
    {
        while (true)
        {
            ReplaceDeadEntities();
            yield return new WaitForSeconds(3f); // Wait for 3 seconds before calling again
        }
    }

    private void ReplaceDeadEntities()
    {
        // Maintain the desired size of aggroedDemons (1)
        for (int i = 0; i < aggroedDemons.Count; i++)
        {
            if (aggroedDemons[i] == null)
            {
                // Find a non-aggroed demon
                GameObject availableDemon = demonsArray.FirstOrDefault(d => d != null && !aggroedDemons.Contains(d) && !d.GetComponent<DemonLogic>().isDead);
                if (availableDemon != null)
                {
                    // Aggro the demon
                    DemonLogic demonLogic = availableDemon.GetComponent<DemonLogic>();
                    demonLogic.player = player;
                    demonLogic?.goAggresive(true);

                    // Replace null with the available demon
                    aggroedDemons[i] = availableDemon;
                }
            }
        }

        // Maintain the desired size of aggroedMinions (5)
        for (int i = 0; i < aggroedMinions.Count; i++)
        {
            if (aggroedMinions[i] == null)
            {
                // Find a non-aggroed minion
                GameObject availableMinion = minionsArray.FirstOrDefault(m => m != null && !aggroedMinions.Contains(m) && !m.GetComponent<Minion_Logic>().isDead);
                if (availableMinion != null)
                {
                    // Aggro the minion
                    Minion_Logic minionLogic = availableMinion.GetComponent<Minion_Logic>();
                    minionLogic.player = player;
                    minionLogic?.goAggresive(true);

                    // Replace null with the available minion
                    aggroedMinions[i] = availableMinion;
                }
            }
        }
    }

    private void aggroEnemies()
    {
        // Randomly select one demon (if available)
        if (demonsArray.Count > 0)
        {
            GameObject selectedDemon = demonsArray
                .Where(d => d != null && !d.GetComponent<DemonLogic>().isDead)
                .OrderBy(_ => Random.value)
                .FirstOrDefault();

            if (selectedDemon != null)
            {
                DemonLogic demonLogic = selectedDemon.GetComponent<DemonLogic>();
                demonLogic.player = player;
                demonLogic?.goAggresive(true);
                aggroedDemons.Add(selectedDemon);
            }
        }

        // Randomly select up to five minions (if available)
        List<GameObject> availableMinions = minionsArray
            .Where(m => m != null && !m.GetComponent<Minion_Logic>().isDead)
            .OrderBy(_ => Random.value)
            .Take(5)
            .ToList();

        foreach (GameObject minion in availableMinions)
        {
            Minion_Logic minionLogic = minion.GetComponent<Minion_Logic>();
            minionLogic.player = player;
            minionLogic?.goAggresive(true);
            aggroedMinions.Add(minion);
        }
    }

    private void deAggroEnemies()
    {
        foreach (GameObject demon in demonsArray)
        {
            if (demon == null) continue;
            DemonLogic demonLogic = demon.GetComponent<DemonLogic>();
            demonLogic?.goAggresive(false);
            aggroedDemons.Remove(demon);
        }

        foreach (GameObject minion in minionsArray)
        {
            if (minion == null) continue;
            Minion_Logic minionLogic = minion.GetComponent<Minion_Logic>();
            minionLogic?.goAggresive(false);
            aggroedMinions.Remove(minion);
        }
    }


}