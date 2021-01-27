using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class movement : MonoBehaviour
{
    public InputActionAsset InputControlActions;
    InputActionMap PlayerMovementMap;
    InputAction HorizontalMovemenAction;
    InputAction VerticalMovementAction;

    public float _movespeed;
    public Transform _movePoint;
    public LayerMask _hurdles;

    private float VerticalVal;
    private float HorizontalVal;

    private void Awake()
    {
        PlayerMovementMap = InputControlActions.FindActionMap("PlayerMovement");

        HorizontalMovemenAction = PlayerMovementMap.FindAction("HorizontalMovement");
        VerticalMovementAction = PlayerMovementMap.FindAction("VerticalMovement");

        HorizontalMovemenAction.performed += GetHoriMoveInput;
        HorizontalMovemenAction.canceled += GetHoriMoveInput;

        VerticalMovementAction.performed += GetVertiMoveInput;
        VerticalMovementAction.canceled += GetVertiMoveInput;
    }
    // Start is called before the first frame update
    void Start()
    {

        _movePoint.parent = null;
    }

    private void GetHoriMoveInput(InputAction.CallbackContext obj)
    {
        HorizontalVal = obj.ReadValue<float>();

    }

    private void GetVertiMoveInput(InputAction.CallbackContext obj)
    {
        VerticalVal = obj.ReadValue<float>();

    }

    private void OnDisable()
    {
        HorizontalMovemenAction.Disable();
        VerticalMovementAction.Disable();
    }

    private void OnEnable()
    {
        HorizontalMovemenAction.Enable();
        VerticalMovementAction.Enable();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, _movespeed * Time.deltaTime);

        MovePlayer();
    }

    private void MovePlayer()
    {
        if (Vector3.Distance(transform.position, _movePoint.position) <= .05f)
        {
            if (Mathf.Abs(HorizontalVal) == 1)
            {
                Collider[] hitcollider = Physics.OverlapSphere(_movePoint.position + new Vector3(HorizontalVal, 0f, 0f), 0.2f, _hurdles);
                if (hitcollider.Length == 0)
                {
                    _movePoint.position += new Vector3(HorizontalVal, 0f, 0f);
                    Debug.Log(hitcollider.Length);
                }
                else
                    _movePoint.position = transform.position;
            }

            if (Mathf.Abs(VerticalVal) == 1)
            {
                Collider[] hitcollider = Physics.OverlapSphere(_movePoint.position + new Vector3(0f, 0f, VerticalVal), 0.2f, _hurdles);
                if (hitcollider.Length == 0)
                {
                    _movePoint.position += new Vector3(0f, 0f, VerticalVal);
                    Debug.Log(hitcollider.Length);
                }
                else
                    _movePoint.position = transform.position;
            }

        }
    }
}

