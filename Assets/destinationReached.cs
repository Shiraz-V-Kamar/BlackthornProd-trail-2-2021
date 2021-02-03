using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destinationReached : MonoBehaviour
{
    public bool Reached = false;
  

    

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") 
        {
            Reached = true;
        }
    }

}
