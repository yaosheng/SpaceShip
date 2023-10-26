using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioClip coinSound;
    //public AudioClip backgroundMusic;
    public AudioSource audioSource;
    public bool isSoundTurnOn;

    void Start( )
    {
        audioSource = GetComponent<AudioSource>( );
        audioSource.Play( );
        isSoundTurnOn = GM.GameSetUp.isSoundOn;
    }

    public void EatingCoin( )
    {
        audioSource.PlayOneShot(coinSound, 0.5f);
    }

    void Update( )
    {
        //Debug.Log("isSoundTurnOn : " + isSoundTurnOn);
        SetSound( );
    }

    void SetSound( )
    {
        if(isSoundTurnOn) {
            audioSource.UnPause( );
        }
        else {
            audioSource.Pause( );
        }
    }
}
;