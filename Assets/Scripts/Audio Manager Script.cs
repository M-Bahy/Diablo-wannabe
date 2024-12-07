
using UnityEngine;

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



    public void PlaySFX (AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

     public void PlayBackground (AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
}
