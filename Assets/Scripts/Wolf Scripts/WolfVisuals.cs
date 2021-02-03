using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfVisuals : MonoBehaviour
{
    public GameObject WolfTriggerParticleObj;
  
    public void TriggerWolfDetectionParticl()
    {
        
        GameObject z =  Instantiate(WolfTriggerParticleObj, transform );
        z.GetComponent<ParticleSystem>().Play();

    }
}
