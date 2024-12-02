using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Minion_Logic : MonoBehaviour
{
    public bool isDead = false;
    int minionMaxHealth = 20;
    int minionCurrentHealth = 20;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        minionCurrentHealth -= damage;
        if (minionCurrentHealth <= 0)
        {
            minionCurrentHealth = 0;
            Die();
        }

        //// Update the health bar
        //if (healthBarSlider != null)
        //{
        //    healthBarSlider.value = (float)minionCurrentHealth / minionMaxHealth;
        //}
    }

    private void Die()
    {
        isDead = true;

        //// Destroy the health bar
        //if (healthBarSlider != null)
        //{
        //    Destroy(healthBarSlider.gameObject);
        //}

        // Destroy the minion
        Destroy(gameObject);
    }
}