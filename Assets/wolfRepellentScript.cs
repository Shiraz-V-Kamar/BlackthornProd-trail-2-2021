using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolfRepellentScript : MonoBehaviour
{
    public LayerMask enenmyLayer;
    [SerializeField]
    private float sphereRadius;
    private void Update()
    {
        Collider[] hitcolliders = Physics.OverlapSphere(transform.position, sphereRadius, enenmyLayer);
        if (hitcolliders.Length > 0)
        {
            for (int i = 0; i < hitcolliders.Length; i++)
            {
                if (hitcolliders[i].gameObject.tag == "enemy")
                {
                    hitcolliders[i].gameObject.GetComponent<WolfScript>().wolfRepellent = true;
                }
            }
        }
    }




    /*private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="enemy")
        {
            other.GetComponent<WolfScript>().wolfRepellent = true;
        }
    }*/

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}
