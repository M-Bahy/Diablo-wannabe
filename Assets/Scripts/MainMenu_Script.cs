using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Button editCameraButton;
    public Button creditsButton;
    public Button playBackButton;
    public Button characterSelectBackButton;
    public Button levelSelectBackButton;
    public Button optionsBackButton;
    public Button editCameraBackButton;
    public Button creditsBackButton;
    public Button iconWizard;
    public Button iconBarbarian;
    [SerializeField] Button angle1Button;
    [SerializeField] Button angle2Button;
    [SerializeField] Button angle3Button;
    [Header("------------ Images ------------")]
    [SerializeField] Image cameraImage;
    [SerializeField] Sprite angle1Image;
    [SerializeField] Sprite angle2Image;
    [SerializeField] Sprite angle3Image;

    public static int goToLevel = 1 ;
    public static bool isWizard = false;
    AudioManagerScript audioManager;
    bool cameFromLevelSelect = false;

    public static int cameraChoice = 1;

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
        editCameraButton.onClick.AddListener(EditCameraButton);
        creditsButton.onClick.AddListener(CreditsButton);
        playBackButton.onClick.AddListener(PlayBackButton);
        characterSelectBackButton.onClick.AddListener(CharacterSelectBackButton);
        levelSelectBackButton.onClick.AddListener(LevelSelectBackButton);
        optionsBackButton.onClick.AddListener(OptionsBackButton);
        editCameraBackButton.onClick.AddListener(EditCameraBackButton);
        creditsBackButton.onClick.AddListener(CreditsBackButton);
        angle1Button.onClick.AddListener(Angle1Button);
        angle2Button.onClick.AddListener(Angle2Button);
        angle3Button.onClick.AddListener(Angle3Button);
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
        PlayerMechanics.isLevel1 = goToLevel == 1;
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
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    public void NewGameButton(){
        playPanel.SetActive(false);
        cameFromLevelSelect = false;
        goToLevel = 1;
        characterSelectPanel.SetActive(true);

    //         static int level = 1;
    // static int exp = 0;
    // static int requiredExp = 100;
    // public static int playerMaxHealth = 100;
    // public static int playerCurrenttHealth = 100;
    // static int numberOfHealingPortions = 0;
    // public static int abilityPoints = 0;
    PlayerMechanics.level = 1;
    PlayerMechanics.exp = 0;
    PlayerMechanics.requiredExp = 100;
    PlayerMechanics.playerMaxHealth = 100;
    PlayerMechanics.playerCurrenttHealth = 100;
    PlayerMechanics.numberOfHealingPortions = 0;
    PlayerMechanics.abilityPoints = 0;
    HUD_Script.abilitiesUnlocked = new bool[4];
    HUD_Script.abilitiesCoolDown = new bool[4];
    HUD_Script.coolDownTimer = new float[4];
    }
    public void LevelSelectButton(){
        playPanel.SetActive(false);
        cameFromLevelSelect = true;
        levelSelectPanel.SetActive(true);
        // /         If the level is restarted, or is loaded using cheats, the Wanderer starts the level with
// character level 4, i.e their HP will be 400, all their abilities unlocked, and healing
// potions equal to 0.

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
    public void EditCameraButton(){
        optionsPanel.SetActive(false);
        teamCreditsPanel.SetActive(true);
    }
    public void CreditsButton(){
        optionsPanel.SetActive(false);
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
        // revert the changes 
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
        
    }
    public void OptionsBackButton(){
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void EditCameraBackButton(){
        teamCreditsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void CreditsBackButton(){
        assetsCreditsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    private void Angle1Button()
    {
        cameraImage.sprite = angle1Image;
        Color hexColor;
        if (ColorUtility.TryParseHtmlString("#00DB1E", out hexColor))
        {
            angle1Button.GetComponent<TextMeshProUGUI>().color = hexColor;
        }
        Color hexColor2;
        if (ColorUtility.TryParseHtmlString("#FFFFFF", out hexColor2))
        {
            angle2Button.GetComponent<TextMeshProUGUI>().color = hexColor2;
            angle3Button.GetComponent<TextMeshProUGUI>().color = hexColor2;
        }
        cameraChoice = 1;
    }
    private void Angle2Button()
    {
        cameraImage.sprite = angle2Image;
        Color hexColor;
        if (ColorUtility.TryParseHtmlString("#00DB1E", out hexColor))
        {
            angle2Button.GetComponent<TextMeshProUGUI>().color = hexColor;
        }
        Color hexColor2;
        if (ColorUtility.TryParseHtmlString("#FFFFFF", out hexColor2))
        {
            angle1Button.GetComponent<TextMeshProUGUI>().color = hexColor2;
            angle3Button.GetComponent<TextMeshProUGUI>().color = hexColor2;
        }
        cameraChoice = 2;
    }
    private void Angle3Button()
    {
        cameraImage.sprite = angle3Image;
        Color hexColor;
        if (ColorUtility.TryParseHtmlString("#00DB1E", out hexColor))
        {
            angle3Button.GetComponent<TextMeshProUGUI>().color = hexColor;
        }
        Color hexColor2;
        if (ColorUtility.TryParseHtmlString("#FFFFFF", out hexColor2))
        {
            angle2Button.GetComponent<TextMeshProUGUI>().color = hexColor2;
            angle1Button.GetComponent<TextMeshProUGUI>().color = hexColor2;
        }
        cameraChoice = 3;
    }
}