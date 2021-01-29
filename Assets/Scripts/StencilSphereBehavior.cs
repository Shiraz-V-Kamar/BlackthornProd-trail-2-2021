using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StencilSphereBehavior : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            Debug.Log("enemy detected");
            if (other.gameObject.GetComponent<WolfManager>() != null)
            {
                other.gameObject.GetComponent<WolfManager>().SetInPlayerVision(true);
                other.gameObject.GetComponent<WolfManager>().PassPlayerScript(this.transform.parent.gameObject);
            }
            else
            {
                Debug.Log("enemy Object doesnt have the scripts attached");
            }
        }

        if (other.gameObject.tag == "Dontcheck " || other.gameObject.tag == "enemy")
        {
            return;
        }
        else
        {
            other.gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "enemy")
        {

            other.gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; 
        }
        if (other.tag == "enemy")
        {
            other.gameObject.GetComponent<WolfManager>().SetInPlayerVision(false);
        }
    }

    
}
