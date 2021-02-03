using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using System;

public class WolfScript : MonoBehaviour
{
    public GameObject ObjwithwolfMaterial;
    public GameObject enemyModelObj;

    private Transform TargetToFollow;
    private Transform _FollowpointBack;
    private Transform _FollowpointFront;
    public Transform spawnPoint;

    public enum State { Idle, triggered, stay, chase, kill, Retreat }
    public State state = default;

    public NavMeshAgent enemyagent;


    [HideInInspector]
    public bool kill = false, isInRange, Retreat, slow;

    public bool chase;
    public bool retreat;
    private bool playWolfAlerSound = true;
    public  bool playWolfHowlSound = true;
    //[HideInInspector]
    public bool wolfRepellent= false;

    private Animator enemyAnim;



    [HideInInspector]
    public float StayTimeout = 8f;
    public float waitAfterTrigger;
    public float waitForChase;
    private float _distToPlayer;
    [SerializeField]
    private float slowSPeed = 2f;
    private float _blendValue;

    public AudioClip WolfAlert;
    public AudioClip wolfgrowlingsound;
    public AudioSource WolfAlertSource;
    public AudioSource WolfGrowlSource;
    


    private WolfVisuals wolfVisuals;
    private float _enemyAnimStart = 1f;
    [SerializeField]
    private float enemySpeed = 4f;
    private Vector3 direction;




    private void Awake()
    {
        enemyagent = GetComponent<NavMeshAgent>();
        wolfVisuals = GetComponent<WolfVisuals>();
        enemyAnim = enemyModelObj.GetComponent<Animator>();
    }


    private void Start()
    {
        playWolfHowlSound = true;
        spawnPoint.parent = null;
        WolfAlertSource = GetComponent<AudioSource>();
        WolfAlertSource.clip = WolfAlert;
        state = State.Idle;
        enemyagent.updateRotation = false;
    }
    private void Update()
    {
        CalculatingBlendVal();


        Statecheck();
        if (slow)
        {
            enemyagent.speed = slowSPeed;
        }
        else
        {
            enemyagent.speed = enemySpeed;
        }

        if (isInRange)
        {
            float distToFollowpointFront = Vector3.Distance(transform.position, _FollowpointFront.position);
            float distToFollowpointBack = Vector3.Distance(transform.position, _FollowpointBack.position);

            if (distToFollowpointBack > distToFollowpointFront)
            {
                TargetToFollow = _FollowpointFront;
            }
            else
            {
                TargetToFollow = _FollowpointBack;
            }

            direction = TargetToFollow.transform.position - transform.position;
            direction.y = 0f;
            direction.Normalize();
        }
        if (kill)
        {
            enemyagent.isStopped = true;
        }

       
    } 

    private void Statecheck()
    {
        if (state == State.Idle)
        {
            wolfRepellent = false;
            playWolfAlerSound = true;
            chase = false;
            StopCoroutine(MoveEnemyToPos((0.1f)));


        }
        else if (state == State.triggered)
        {
           chase = false;
            if (playWolfAlerSound)
            {
                PlayWolfAlertSound();
            }
            StayTimeout = 8f;
            slow = false;
            enemyAnim.SetBool("Triggered", true);
            if (isInRange)
            {

                playWolfAlerSound = false;

                if (TargetToFollow != null)
                {
                    DOTween.To(() => transform.forward, X => transform.forward = X, direction, 1.8f);
                    enemyAnim.SetFloat("MoveBlendWolf", _blendValue, _enemyAnimStart, Time.deltaTime);

                    StartCoroutine(MoveEnemyToPos(waitAfterTrigger));

                }
                else
                {
                    state = State.stay;
                }
            }
        }
        else if (state == State.stay)
        {
            chase = false;
            StayTimeout -= Time.deltaTime;
            StayTimeout = Mathf.Clamp(StayTimeout, 0, 8f);
            
            slow = false;
            enemyagent.SetDestination(transform.position);
            if (!isInRange)
            {
                enemyAnim.SetBool("Triggered", false);
                enemyAnim.SetBool("Sniff", true);
                enemyAnim.SetFloat("MoveBlendWolf", _blendValue, _enemyAnimStart, Time.deltaTime);
            }
            retreat = true;
            
        }
        else if (state == State.chase)
        {
           
            StayTimeout = 8f;
            enemyAnim.SetBool("Triggered", true);
            enemyAnim.SetBool("Sniff", false);
            
            DOTween.To(() => transform.forward, X => transform.forward = X, direction, 1.8f);
            enemyAnim.SetFloat("MoveBlendWolf", _blendValue, _enemyAnimStart, Time.deltaTime);
            StartCoroutine(MoveEnemyToPos(waitForChase));
            chase = true;
            slow = true;

        
        }
        else if (state == State.Retreat)
        {
            
            chase = false;
            slow = false;
            Vector3 Spawndirection = spawnPoint.position - transform.position;
            Spawndirection.y = 0f;
            Spawndirection.Normalize();
            if (transform.forward != Spawndirection)
            {
                DOTween.To(() => transform.forward, X => transform.forward = X, Spawndirection, 1.8f);
            }
                enemyAnim.SetFloat("MoveBlendWolf", _blendValue, _enemyAnimStart, Time.deltaTime);
           
            StartCoroutine(MoveEnemyToSpawn(1f));
            if (enemyagent.remainingDistance < 0.5f)
            {
               
                state = State.Idle;
                enemyAnim.SetBool("Triggered", false);
                enemyAnim.SetBool("Sniff", false);
            }
            else
            {
                enemyAnim.SetBool("Triggered", true);
                enemyAnim.SetBool("Sniff", false);
               
             
            }
           
        }
        else if (state == State.kill)
        {
            //play the camera out animation 
            
            chase = false;
            enemyAnim.SetBool("attack", true);
           
        }

        
    }

    private void CalculatingBlendVal()
    {
        _distToPlayer = Mathf.Clamp(enemyagent.remainingDistance, 0f, 10f);
        if (chase == false)
        {
            if (!Retreat)
            {
                if (_distToPlayer > 0 && _distToPlayer < 8)
                {

                    DOTween.To(() => _blendValue, X => _blendValue = X, 0.2f, .25f);
                }
                else if (_distToPlayer > 8)
                {
                    DOTween.To(() => _blendValue, X => _blendValue = X, 1f, .25f);

                }
                else
                {
                    DOTween.To(() => _blendValue, X => _blendValue = X, 0f, .25f);
                }
            } else
            {
                DOTween.To(() => _blendValue, X => _blendValue = X, 0f, .25f);
            }
        }
        else
        {

            DOTween.To(() => _blendValue, X => _blendValue = X, 0f, .25f);
        }

    }

    IEnumerator MoveEnemyToPos(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (isInRange)
        {
            enemyagent.SetDestination(TargetToFollow.transform.position);
        }
    }

    IEnumerator MoveEnemyToSpawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
       
            enemyagent.SetDestination(spawnPoint.transform.position);
    }
    void PlayWolfAlertSound()
    {

        WolfAlertSource.clip = WolfAlert;
        WolfAlertSource.Play();
        wolfVisuals.TriggerWolfDetectionParticl();
        playWolfAlerSound = false;
    }

    public void PlayWolfChaseSound()
    {
        if (playWolfHowlSound)
        {
            WolfGrowlSource.DOFade(0.04f, 0.5f);
            WolfGrowlSource.clip = wolfgrowlingsound;
            if (!WolfGrowlSource.isPlaying)
            {
                WolfGrowlSource.Play();
            }
        }
    }

    public void FadeOutChaseSound()
    {
        
            WolfGrowlSource.DOFade(0f, 0.5f);
        
    }

    public void GetWolfFollowPoint(Transform followPointFront, Transform followPointBack)
    {
        _FollowpointBack = followPointBack;
        _FollowpointFront = followPointFront;
    }
}
