using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfManager : MonoBehaviour
{
    private GameObject player;
    PlayerMoveNav playerMoveNav;
    VisionControl visionControl;
    WolfScript wolfScript;

    private void Awake()
    {
        
        wolfScript = GetComponent<WolfScript>();
    }

    internal void SetKillState(bool v)
    {
        wolfScript.kill = v;
    }

    internal void SetInPlayerVision(bool v)
    {
        wolfScript.isInRange = v;
    }

    internal void PassPlayerScript(GameObject gameObject)
    {
        player = gameObject;
        wolfScript. PassingPlayerObj(player);
        passingValue();
    }

    private void Update()
    {
       
        if (visionControl != null)
        {
            if (!wolfScript.kill)
            {
                if (visionControl._VignetteIntensityValue < 0.7)
                {
                    if (wolfScript.isInRange)
                    {
                        
                        wolfScript.state = WolfScript.State.triggered;
                    }
                  
                }
                else if (visionControl._VignetteIntensityValue > 0.7)
                {
                    if (wolfScript.isInRange)
                    {
                        wolfScript.state = WolfScript.State.chase;
                    }
                    else
                    {
                        wolfScript.state = WolfScript.State.stay;
                    }
                }
                else
                {
                    wolfScript.state = WolfScript.State.Idle;
                }
            }
            else
            {
                wolfScript.state = WolfScript.State.kill;
            }
        }
    }

    public void passingValue()
    {
        playerMoveNav = player.GetComponent<PlayerMoveNav>();
        visionControl = player.GetComponent<VisionControl>();
    }
}


