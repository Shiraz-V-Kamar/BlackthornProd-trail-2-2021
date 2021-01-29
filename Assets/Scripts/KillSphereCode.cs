using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSphereCode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       if(other.tag == "enemy")
        {
            other.gameObject.GetComponent<WolfManager>()
                            .SetKillState(true);
        }
    }
}
