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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame() {
        int level = PlayerMechanics.isLevel1 ? 1 : 2;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level" + level+"_scene");
    }

    public void MainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu_Scene");
    }
}
