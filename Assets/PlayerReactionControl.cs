using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReactionControl : MonoBehaviour
{
    public AudioSource PlayerReactSoundSource;
    public AudioClip AlertSound;

    public bool PlayStartledSound;

    private void Start()
    {
        PlayStartledSound = true;
    }

    void PlayAlertSoundFunc()
    {
        if (PlayStartledSound)
        {
            PlayStartledSound = false;
            PlayerReactSoundSource.clip = AlertSound;
            PlayerReactSoundSource.Play();
        }
    }
}
