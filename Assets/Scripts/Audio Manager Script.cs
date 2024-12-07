
using UnityEngine;
using UnityEngine.UI;

public class AudioManagerScript : MonoBehaviour
{

    [Header("------------ Are we in the main menu ------------")]
    [SerializeField] bool isMainMenu;

    [Header("------------ Audio Source ------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("------------ Audio Clips ------------]")]
    public AudioClip background ;
    public AudioClip menu ;


    public AudioClip boostTile ;
    public AudioClip stickyTile ;
    public AudioClip suppliesTile ;
    public AudioClip burningTile ;
    public AudioClip obstacleTile;
    public AudioClip emptyTile;
    public AudioClip invalidAction;
   public Slider musicSlider;
    public Slider SFXSlider;

    public static float musicVolume = 0.0f;
    public static float SFXVolume = 0.0f;

    private void Start() {

        //PlayBackground(background);
        if ( isMainMenu)
        {
            musicSlider.value = 0.5f;
            SFXSlider.value = 0.5f;
        }
        else
        {
            SetMusicVolume(musicVolume);
            SetSFXVolume(SFXVolume);
        }
        
    }

    public void PlaySFX (AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

     public void PlayBackground (AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void SetMusicVolume (float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume (float volume)
    {
        SFXSource.volume = volume;
    }

    private void Update() {
        if (isMainMenu)
        {
           SetMusicVolume(musicSlider.value);
           SetSFXVolume(SFXSlider.value);
        }
        
    }

}
