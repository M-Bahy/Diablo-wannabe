using UnityEngine;
using UnityEngine.UI;

public class AudioManagerScript : MonoBehaviour
{
    [Header("------------ Are we in the main menu ------------")]
    [SerializeField] bool isMainMenu;

    [Header("------------ Volume Sliders ------------]")]
    public Slider musicSlider;
    public Slider SFXSlider;

    [Header("------------ Audio Source ------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("------------ Music Clips ------------]")]
    public AudioClip Menus;
    public AudioClip Level1;
    public AudioClip Level2;

    [Header("------------ SFX Clips ------------]")]
    public AudioClip Shield_Activated;
    public AudioClip Inferno_Activated;
    public AudioClip Clone_Activated;
    public AudioClip Charging;
    // public AudioClip Dashing;
    // public AudioClip Arrow_Fired;
    public AudioClip Explosive_Detonates;
    public AudioClip Fireball_Shot;
    public AudioClip Item_Picked_Up;
    public AudioClip Wanderer_Damaged;
    public AudioClip Wanderer_Healing_Potion;
    public AudioClip Wanderer_Dies;
    public AudioClip Enemy_Dies;
    public AudioClip Boss_Summons_Minions;
    public AudioClip Boss_Stomps_Down;
    public AudioClip Boss_Casts_Spell;
    public AudioClip Boss_Swings_Hands;
    public AudioClip Boss_Getting_Damaged;
    public AudioClip Boss_Dies;

    public static float musicVolume = 0.2f;
    public static float SFXVolume = 0.5f;

    private void Start()
    {
        if (isMainMenu)
        {
            musicSlider.value = musicVolume;
            SFXSlider.value = SFXVolume;
        }
        else
        {
            SetMusicVolume(musicVolume);
            SetSFXVolume(SFXVolume);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayBackground(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = volume;
        SFXSource.volume = volume;
    }

    private void Update()
    {
        if (isMainMenu)
        {
            SetMusicVolume(musicSlider.value);
            SetSFXVolume(SFXSlider.value);
        }
    }
}