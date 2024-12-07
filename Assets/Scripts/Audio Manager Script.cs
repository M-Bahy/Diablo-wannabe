
using UnityEngine;
using UnityEngine.UI;

public class AudioManagerScript : MonoBehaviour
{

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

    private void Start() {
        PlayBackground(background);
        musicSlider.value = 1.0f;
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
       // Debug.Log(musicSlider.value);
        // when the user presses + or - on the keyboard, the volume of the music will increase or decrease
        // if (Input.GetKeyDown(KeyCode.Plus))
        // {
        //     musicSource.volume = Mathf.Clamp(musicSource.volume + 0.1f, 0.0f, 1.0f);
        // }
        // if (Input.GetKeyDown(KeyCode.B))
        // {
        //     float volume = Mathf.Clamp(musicSource.volume - 0.1f, 0.0f, 1.0f);
        //     SetMusicVolume(volume);
        // }
        SetMusicVolume(musicSlider.value);
      //  Debug.Log(musicSource.volume);
    }

}
