using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Over_Script : MonoBehaviour
{
    AudioManagerScript audioManager;
    public Button restartButton;
    public Button mainMenuButton;

    private void Awake() {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManagerScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        audioManager.PlayBackground(audioManager.Menus); 
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(MainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame() {
        Time.timeScale = 1;
        int level = PlayerMechanics.isLevel1 ? 1 : 2;
        Minion_Logic.isGameOver = false;
        BossMech.gameOver = false;
        if (level == 2)
        {
            PlayerMechanics.level = 4;
            PlayerMechanics.exp = 0;
            PlayerMechanics.playerMaxHealth = 400;
            PlayerMechanics.playerCurrenttHealth = 400;
            PlayerMechanics.numberOfHealingPortions = 0;
            PlayerMechanics.abilityPoints = 0;
            HUD_Script.abilitiesUnlocked = new bool[4];
            HUD_Script.abilitiesUnlocked[0] = true;
            HUD_Script.abilitiesUnlocked[1] = true;
            HUD_Script.abilitiesUnlocked[2] = true;
            HUD_Script.abilitiesUnlocked[3] = true;
            HUD_Script.ResetCoolDowns();
            Healing_Script.collectedHealingPotions.Clear();
        }
        else
        {
            PlayerMechanics.level = 1;
            PlayerMechanics.exp = 0;
            PlayerMechanics.playerMaxHealth = 100;
            PlayerMechanics.playerCurrenttHealth = 100;
            PlayerMechanics.numberOfHealingPortions = 0;
            PlayerMechanics.abilityPoints = 0;
            HUD_Script.abilitiesUnlocked = new bool[4];
            HUD_Script.abilitiesUnlocked[0] = true;
            HUD_Script.abilitiesUnlocked[1] = false;
            HUD_Script.abilitiesUnlocked[2] = false;
            HUD_Script.abilitiesUnlocked[3] = false;
            HUD_Script.ResetCoolDowns();
            Healing_Script.collectedHealingPotions.Clear();
            PlayerMechanics.numberOfFragments = 0;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level" + level + "_scene");
    }

    public void MainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu_Scene");
    }
}
