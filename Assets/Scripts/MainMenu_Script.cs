using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_Script : MonoBehaviour
{
    public Button goToLevel2;
    public Slider musicSlider;
    public Slider SFXSlider;
    // Start is called before the first frame update
    void Start()
    {
        goToLevel2.onClick.AddListener(GoToLevel2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void GoToLevel2()
    {
        AudioManagerScript.musicVolume = musicSlider.value;
        AudioManagerScript.SFXVolume = SFXSlider.value;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level2_scene");
    }
}
