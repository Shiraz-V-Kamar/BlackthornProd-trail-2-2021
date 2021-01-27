using System.Collections;
using System.Timers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class PlayerMoveNav : MonoBehaviour
{
    public LayerMask isground;
    public NavMeshAgent player;
    Vector3 MovePoint;

    private readonly Timer _MouseSingleClickTimer = new Timer();

    // Start is called before the first frame update
    void Start()
    {
        _MouseSingleClickTimer.Interval = 400;
        _MouseSingleClickTimer.Elapsed += SingleClick;
        player = GetComponent<NavMeshAgent>();
        
    }

    private void SingleClick(object o,System.EventArgs e)
    {
        _MouseSingleClickTimer.Stop();

        Debug.Log("Single Click");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (_MouseSingleClickTimer.Enabled == false)
            {
                // timer for click checker starts
                _MouseSingleClickTimer.Start();
                // Now it waits for the second input

            }
            else
            {
                //Double clicked , cancel single click
                _MouseSingleClickTimer.Stop();

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                Debug.Log("Double click worked");

                if (Physics.Raycast(ray,out hitInfo,100, isground))
                {
                    player.SetDestination(hitInfo.point);
                }
            }
        }
        
    }
}
