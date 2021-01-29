using System.Collections;
using System.Timers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using DG.Tweening;
using System;

public class PlayerMoveNav : MonoBehaviour
{
    public GameObject MoveMarker;
    private VisionControl _visionControl;
    private Animator _playeranimator;
    public GameManager gameManager;
    public GameObject model;

    public LayerMask isground;
    public NavMeshAgent player;

    public Slider DistToHome;
    private bool isCrouching;

    private readonly Timer _MouseSingleClickTimer = new Timer();


    private float _velocity;
    private float _walkAnimStart = 0.2f;
    private float _crouchAnimeStart = 0.2f;
    private float blendVariable;

    public GameObject home;
    private Vector3 direction;

    public AudioSource ZoomingInSoundSource;
    public AudioSource ZoomingOutSoundSource;
    public AudioClip ZoomingInSOundClip;
    public AudioClip ZoomingOutSOundClip;

    // Start is called before the first frame update
    void Start()
    {
        MoveMarker.SetActive(false);
        MoveMarker.transform.parent = null;
        _MouseSingleClickTimer.Interval = 400;
        _MouseSingleClickTimer.Elapsed += SingleClick;
        _visionControl = GetComponent<VisionControl>();
        player = GetComponent<NavMeshAgent>();
        _playeranimator = model.GetComponent<Animator>();
    }

    private void SingleClick(object o,System.EventArgs e)
    {
        _MouseSingleClickTimer.Stop();

        Debug.Log("Single Click");
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistToFinish();
        player.updateRotation = false;
        if(_visionControl._VignetteIntensityValue > 0.7)
        {
            //play the sound here 
            isCrouching = true;
        }else if (_visionControl._VignetteIntensityValue< 0.7)
        {
            //Play the sound here
            isCrouching = false;
        }

        blendVariable = Mathf.Clamp(player.remainingDistance,0f,10f);
        
        if (blendVariable > 0 && blendVariable < 8)
        { 
            DOTween.To(() => _velocity, X => _velocity= X,0.2f, .25f);
        }
        else if(player.remainingDistance > 8)
        {
            DOTween.To(() => _velocity, X => _velocity = X, 0.5f, .25f);
           
        }else
        {
            
            DOTween.To(() => _velocity, X => _velocity = X, 0f, .25f);
        }

        if(blendVariable <4)
        {
            MoveMarker.SetActive(false);
        }

       
        //Debug.Log(player.remainingDistance);
        if (isCrouching)
        {
            _playeranimator.SetBool("isCrouching", true);
            _playeranimator.SetFloat("BlendCrouch", _velocity, _walkAnimStart, Time.deltaTime);

        }
        else
        {
            _playeranimator.SetBool("isCrouching", false);
            _playeranimator.SetFloat("BlendWalk", _velocity, _crouchAnimeStart, Time.deltaTime);
        }
     
     

        GettingMouseInputtoMove();
        
        
    }

    private void CheckDistToFinish()
    {
        float distleft = Vector3.Distance(transform.position, home.transform.position);
        Debug.Log(distleft);
        distleft = (distleft *10f)/100f;
        distleft = Mathf.Clamp(distleft, 0, 10);

        DOTween.To(() => DistToHome.value, X => DistToHome.value = X, distleft, 1.25f);
        if (distleft == 0)
        {
            
            gameManager.GameFinished();
        }
    }

    private void GettingMouseInputtoMove()
    {
        if (Input.GetMouseButtonDown(0))
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

                if (Physics.Raycast(ray, out hitInfo, 100, isground))
                {
                    Vector3 MovemarkerOffset = new Vector3(0,0.3f,0);
                    MoveMarker.SetActive(true);
                    MoveMarker.transform.position = hitInfo.point + MovemarkerOffset ;
                    direction = hitInfo.point - transform.position;
                    direction.y = 0f;
                    direction.Normalize();
                    //Debug.Log(direction);
                    DOTween.To(() => transform.forward, X => transform.forward= X,  direction, 1.8f);

                    player.isStopped = false;
                    player.SetDestination(hitInfo.point);
                    
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            MoveMarker.SetActive(false);
            //player.velocity;
            DOTween.To(() => _velocity, X => _velocity = X, 0f, .05f);
            DOTween.To(() => player.velocity, X => player.velocity = X, Vector3.zero, 1.25f);
            player.SetDestination(transform.position);
        }
    }
}
