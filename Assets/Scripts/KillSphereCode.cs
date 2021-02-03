using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSphereCode : MonoBehaviour
{
    public PlayerMoveNav playerMoveNav;

 

   private void OnTriggerEnter(Collider other)
    {
       if(other.tag == "enemy")
        {
            
            other.gameObject.GetComponent<WolfManager>()
                            .SetKillState(true);
            playerMoveNav.wolfattacked = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "enemy")
        {
           
            playerMoveNav.wolfattacked = false;
            playerMoveNav.Getwolfposition(other.transform.position);
        }
    }

}
