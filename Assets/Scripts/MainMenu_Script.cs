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
    public static int goToLevel = 1 ;

    AudioManagerScript audioManager;

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
}
