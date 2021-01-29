using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementOldInput : MonoBehaviour
{ 
    public float _movespeed;
    public Transform _movePoint;
    public LayerMask _hurdles;

    // Start is called before the first frame update
    void Start()
    {

        _movePoint.parent = null;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, _movespeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _movePoint.position) <= .05f)
        {
             if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1)
            {
               Collider[] hitcollider = Physics.OverlapSphere(_movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.2f, _hurdles);
                if (hitcollider.Length == 0)
                {
                    _movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    Debug.Log(hitcollider.Length);
                }
                else
                    _movePoint.position = transform.position;

        }
        else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1)
            {
                Collider[] hitcollider = Physics.OverlapSphere(_movePoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), 0.2f, _hurdles);
                if (hitcollider.Length == 0)
                {
                    _movePoint.position += new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));
                    Debug.Log(hitcollider.Length);
                }
                else
                    _movePoint.position = transform.position;
            }
        }
    }
}
