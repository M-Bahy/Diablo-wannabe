using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    // Start is called before the first frame update
    void Start()
    {
        minX = this.transform.position.x - 20;
        minZ = this.transform.position.z - 20;
        maxX = this.transform.position.x + 20;
        maxZ = this.transform.position.z + 20;

        int demonCount = Random.Range(1, 3);
        int minionCount = Random.Range(8, 11);
        GameObject tmp;

        for (int i = 0; i < demonCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition(demonY);
            tmp = Instantiate(demon, randomPosition, Quaternion.identity);
            demonsArray.Add(tmp);
        }

        for (int i = 0; i < minionCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition(minionY);
            tmp = Instantiate(minion, randomPosition, Quaternion.identity);
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
                      demonsArray.All(d => d == null || d.GetComponent<Minion_Logic>().isDead);

            if (keyFlag)
            {
                Instantiate(key, new Vector3((minX + maxX) / 2, 1.0f, (minZ + maxZ) / 2), Quaternion.identity);
                doneFlag = true;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Aggro");
        if (other.CompareTag("Sorcerer") || other.CompareTag("Barbarian"))
        {
            // Randomly select one demon (if available)
            if (demonsArray.Count > 0)
            {
                GameObject selectedDemon = demonsArray
                    .Where(d => d != null && !d.GetComponent<Minion_Logic>().isDead)
                    .OrderBy(_ => Random.value)
                    .FirstOrDefault();

                if (selectedDemon != null)
                {
                    Minion_Logic demonLogic = selectedDemon.GetComponent<Minion_Logic>();
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
        else if (other.CompareTag("Fireball") && aggroedDemons.Count == 0 && aggroedMinions.Count == 0)
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Deaggro");
        if (other.CompareTag("Sorcerer") || other.CompareTag("Barbarian"))
        {
            foreach (GameObject demon in demonsArray)
            {
                if (demon == null) continue;
                Minion_Logic demonLogic = demon.GetComponent<Minion_Logic>();
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

    private IEnumerator CallReplaceDeadEntities()
    {
        while (true)
        {
            ReplaceDeadEntities(); // Call your method here
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
                GameObject availableDemon = demonsArray.FirstOrDefault(d => d != null && !aggroedDemons.Contains(d) && !d.GetComponent<Minion_Logic>().isDead);
                if (availableDemon != null)
                {
                    // Aggro the demon
                    Minion_Logic demonLogic = availableDemon.GetComponent<Minion_Logic>();
                    demonLogic.player = player;
                    demonLogic?.goAggresive(true);

                    // Replace null with the available demon
                    aggroedDemons[i] = availableDemon;
                    Debug.Log("Replaced a demon in aggroed list");
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
                    Debug.Log("Replaced a minion in aggroed list");
                }
            }
        }
    }




}