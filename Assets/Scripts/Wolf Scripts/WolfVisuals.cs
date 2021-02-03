using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfVisuals : MonoBehaviour
{
    public ParticleSystem wolfTriggerParticle;
    
    public void TriggerWolfDetectionParticl()
    {
        wolfTriggerParticle.Play();
    }
}
