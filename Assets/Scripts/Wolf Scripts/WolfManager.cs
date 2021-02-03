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

    [SerializeField]
    private float MinimumDist = 7f;

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
        
        passingValue();
        
    }

    private void Update()
    {

        if (visionControl != null)
        {
            if (wolfScript.wolfRepellent)
            {
                wolfScript.state = WolfScript.State.Retreat;
                wolfScript.FadeOutChaseSound();
            }
            else
            {


                if (visionControl._VignetteIntensityValue > 0.7)
                {
                    if (!wolfScript.kill)
                    {
                        if (wolfScript.isInRange)
                        {
                            wolfScript.PlayWolfChaseSound();
                        }
                        else
                        {
                            wolfScript.FadeOutChaseSound();
                        }
                    }
                    else
                    {
                        wolfScript.FadeOutChaseSound();
                    }
                }
                else if (visionControl._VignetteIntensityValue < 0.7)
                {
                    if (!wolfScript.kill)
                    {
                        if (wolfScript.isInRange)
                        {
                            wolfScript.PlayWolfChaseSound();
                        }
                        else
                        {
                            wolfScript.FadeOutChaseSound();
                        }
                    }
                    else
                    {
                        wolfScript.FadeOutChaseSound();
                    }
                }
                if (!wolfScript.kill)
                {

                    if (visionControl._VignetteIntensityValue < 0.7)
                    {
                        if (wolfScript.isInRange)
                        {
                            wolfScript.state = WolfScript.State.triggered;
                            float DistToPlayer = Vector3.Distance(transform.position, player.transform.position);
                            if (DistToPlayer < MinimumDist)
                            {
                                wolfScript.state = WolfScript.State.chase;
                            }
                        }
                        else
                        {
                            wolfScript.state = WolfScript.State.Idle;
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
                            if (wolfScript.StayTimeout > 0)
                            {

                                wolfScript.state = WolfScript.State.stay;
                            }
                            else
                            {
                                wolfScript.state = WolfScript.State.Retreat;
                                if(wolfScript.enemyagent.remainingDistance<1)
                                {
                                    wolfScript.state = WolfScript.State.Idle;
                                }
                            }
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
    }
  
    public void passingValue()
    {
        playerMoveNav = player.GetComponent<PlayerMoveNav>();
        visionControl = player.GetComponent<VisionControl>();
        wolfScript.GetWolfFollowPoint(playerMoveNav.wolfFollowPointFront.transform, playerMoveNav.wolfFollowPointBack.transform);
    }
}


