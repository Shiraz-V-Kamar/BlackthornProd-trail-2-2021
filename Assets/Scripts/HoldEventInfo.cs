using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldEventInfo : MonoBehaviour
{
    public WolfScript wolfScript;
    public PlayerMoveNav playerMoveNav;

    public AudioSource WolfhowsoundSource;
    public AudioSource WolfAlertSource;
    public AudioClip WolfHowlSound;
    public AudioClip WolfAlertSound;

    private bool PlayhowlSound;
    private void Start()
    {
        playerMoveNav.wolfattacked = false;
        WolfAlertSource.clip = WolfAlertSound;
        WolfhowsoundSource.clip = WolfHowlSound;

    }
    private void Update()
    {
        if(PlayhowlSound)
        {
            WolfhowsoundSource.Play();
            PlayhowlSound = false;
        }
    }
    void SetaValue()
    {
        WolfAlertSource.Play();
        Debug.Log("ScripWorked");
        playerMoveNav.wolfattacked = true;
    }
    
    void setHowlOn()
    {
        PlayhowlSound = true;
    }
}  
