using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolfRepellentScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="enemy")
        {
            other.GetComponent<WolfScript>().wolfRepellent = true;
        }
    }


}
