using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_Script : MonoBehaviour
{
    [Header("------------ Audio Sliders ------------")]
    public Slider musicSlider;
    public Slider SFXSlider;

    [Header("------------ UI Panels ------------")]
    public GameObject mainMenuPanel;
    public GameObject playPanel;
    public GameObject characterSelectPanel;
    public GameObject levelSelectPanel;
    public GameObject optionsPanel;
    public GameObject teamCreditsPanel;
    public GameObject assetsCreditsPanel;
    [Header("------------ Buttons ------------")]
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;
    public Button newGameButton;
    public Button levelSelectButton;
    public Button level1Button;
    public Button level2Button;
    // public Button wizardButton;
    // public Button barbarianButton;
    public Button teamCreditsButton;
    public Button assetsCreditsButton;
    public Button playBackButton;
    public Button characterSelectBackButton;
    public Button levelSelectBackButton;
    public Button optionsBackButton;
    public Button teamCreditsBackButton;
    public Button assetsCreditsBackButton;
    public Button iconWizard;
    public Button iconBarbarian;
    public static int goToLevel = 1 ;
    public static bool isWizard = true;
    AudioManagerScript audioManager;
    bool cameFromLevelSelect = false;

    private void Awake() {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManagerScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        audioManager.PlayBackground(audioManager.Menus);
        // disable all panels except main menu
        playPanel.SetActive(false);
        characterSelectPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        optionsPanel.SetActive(false);
        teamCreditsPanel.SetActive(false);
        assetsCreditsPanel.SetActive(false);
        // add listeners to buttons
        playButton.onClick.AddListener(PlayButton);
        optionsButton.onClick.AddListener(OptionsButton);
        quitButton.onClick.AddListener(QuitButton);
        newGameButton.onClick.AddListener(NewGameButton);
        levelSelectButton.onClick.AddListener(LevelSelectButton);
        level1Button.onClick.AddListener(Level1Button);
        level2Button.onClick.AddListener(Level2Button);
        iconWizard.onClick.AddListener(WizardButton);
        iconBarbarian.onClick.AddListener(BarbarianButton);
        teamCreditsButton.onClick.AddListener(TeamCreditsButton);
        assetsCreditsButton.onClick.AddListener(AssetsCreditsButton);
        playBackButton.onClick.AddListener(PlayBackButton);
        characterSelectBackButton.onClick.AddListener(CharacterSelectBackButton);
        levelSelectBackButton.onClick.AddListener(LevelSelectBackButton);
        optionsBackButton.onClick.AddListener(OptionsBackButton);
        teamCreditsBackButton.onClick.AddListener(TeamCreditsBackButton);
        assetsCreditsBackButton.onClick.AddListener(AssetsCreditsBackButton);
        // set slider values
        musicSlider.value = AudioManagerScript.musicVolume;
        SFXSlider.value = AudioManagerScript.SFXVolume;
        
    }

    // Update is called once per frame
    void Update()
    {
        AudioManagerScript.musicVolume = musicSlider.value;
        AudioManagerScript.SFXVolume = SFXSlider.value;
    }

     void GoToLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level" + goToLevel+"_scene");
    }
    void PlayButton()
    {
        mainMenuPanel.SetActive(false);
        playPanel.SetActive(true);
    }
    void OptionsButton()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    void QuitButton()
    {
        Application.Quit();
    }
    public void NewGameButton(){
        playPanel.SetActive(false);
        cameFromLevelSelect = false;
        goToLevel = 1;
        characterSelectPanel.SetActive(true);
    }
    public void LevelSelectButton(){
        playPanel.SetActive(false);
        cameFromLevelSelect = true;
        levelSelectPanel.SetActive(true);
    }
    public void Level1Button(){
        goToLevel = 1;
        levelSelectPanel.SetActive(false);
        characterSelectPanel.SetActive(true);
    }
    public void Level2Button(){
        goToLevel = 2;
        levelSelectPanel.SetActive(false);
        characterSelectPanel.SetActive(true);
    }
    public void WizardButton(){
        // some code to select wizard
        isWizard = true;
        GoToLevel();
    }
    public void BarbarianButton(){
        // some code to select barbarian
        isWizard = false;
        GoToLevel();
    }
    public void TeamCreditsButton(){
        mainMenuPanel.SetActive(false);
        teamCreditsPanel.SetActive(true);
    }
    public void AssetsCreditsButton(){
        mainMenuPanel.SetActive(false);
        assetsCreditsPanel.SetActive(true);
    }
    public void PlayBackButton(){
        playPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void CharacterSelectBackButton(){
        characterSelectPanel.SetActive(false);
        Hoverable_Button hb = iconWizard.GetComponent<Hoverable_Button>();
        hb.ResetButton();
        hb = iconBarbarian.GetComponent<Hoverable_Button>();
        hb.ResetButton();
        if(cameFromLevelSelect){
            levelSelectPanel.SetActive(true);
        }else{
            playPanel.SetActive(true);
        }
    }
    public void LevelSelectBackButton(){
        levelSelectPanel.SetActive(false);
        playPanel.SetActive(true);
    }
    public void OptionsBackButton(){
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void TeamCreditsBackButton(){
        teamCreditsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void AssetsCreditsBackButton(){
        assetsCreditsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
}