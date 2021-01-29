using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScriptToTestAgent : MonoBehaviour
{
    public LayerMask isground;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            Debug.Log("Double click worked");

            if (Physics.Raycast(ray, out hitInfo, 100, isground))
            {
                agent.SetDestination(hitInfo.point);
            }
    }
    }
}
