using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using System;

public class WolfScript : MonoBehaviour
{
    private HoldEventInfo holdEventInfo;
    public GameObject enemyModelObj;
    private GameObject playerObj;
    public enum State { Idle, triggered, stay, chase, kill, Retreat}

    public NavMeshAgent enemyagent ;
    public State state = default;

    public bool kill;
    public bool isInRange;
    public bool Retreat;
    public bool slow;

    private Animator enemyAnim;

    private float OutofRangeTimeout;
    private float InRangeTimeout;
    private float _distToPlayer;
    private float slowSPeed =0.5f;

    public float waitAfterTrigger;
    public float waitForChase;
    public Transform spawnPoint;
    private float _blendValue;

    public AudioClip WolfAlert;
    public AudioSource WolfAlertSource;
    internal void PassingPlayerObj(GameObject player)
    {
        playerObj = player;
    }

    //private float OutofRangeTimeout;

    /*public WolfEvent OnStartTrigger;
    public WolfEvent OnStay;
    public WolfEvent OnKill;
    public WolfEvent OnChase;*/

    private WolfVisuals wolfVisuals;
    private float _enemyAnimStart= 1f;
    [SerializeField]
    private  float enemySpeed;
    private Vector3 direction;
    

    private void Awake()
    {
        enemyagent = GetComponent<NavMeshAgent>();
        wolfVisuals = GetComponent<WolfVisuals>();
        enemyAnim = enemyModelObj.GetComponent<Animator>();
    }


    private void Start()
    {
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
            direction = playerObj.transform.position - transform.position;
            direction.y = 0f;
            direction.Normalize();
        }
    }

    private void Statecheck()
    {
        if (state == State.Idle)
        {

            //Play the idle animation 

        }
        else if (state == State.triggered)
        {
            slow = false;
            enemyAnim.SetBool("Triggered", true);
            if (isInRange)
            {
                WolfAlertSource.Play();
                if (playerObj != null)
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
            //player ground thingy to work now

            //starts running if the player uses big vision for long time
            Debug.Log("triggered state");
        }
        else if (state == State.stay)
        {
            slow = false;
            enemyagent.SetDestination(transform.position);
            if (!isInRange)
            {
                enemyAnim.SetBool("Triggered", false);
                enemyAnim.SetBool("Sniff", false);
            }
            //starts stand and snif 
            //lays low 
            //or
            //change state to retreat state
            Debug.Log("stay state");
        }
        else if (state == State.chase)
        {
            enemyAnim.SetBool("Triggered", true);
            enemyAnim.SetBool("Sniff", false);
            DOTween.To(() => transform.forward, X => transform.forward = X, direction, 1.8f);
            enemyagent.SetDestination(playerObj.transform.position);
            enemyAnim.SetFloat("MoveBlendWolf", _blendValue, _enemyAnimStart, Time.deltaTime);
            StartCoroutine(MoveEnemyToPos(waitForChase));

            //walks slowly towards the player
            slow = true;
            //enemyagent.speed = slowWalk;

            Debug.Log("chase state");
        }
        else if (state == State.Retreat)
        {
            slow = false;
            //walks back to the spawn point or nearby spawn point
            enemyagent.SetDestination(spawnPoint.position);
            Debug.Log("Retreat state");
        }
        else if (state == State.kill)
        {
            //Trigger player fall animation 
            //Trigger the lose condition for the player
            //Stop the the game 
            //play the camera out animation 
            enemyAnim.SetBool("attack", true);
            Debug.Log("Kill state");
        }
    }

    private void CalculatingBlendVal()
    {
        _distToPlayer = Mathf.Clamp(enemyagent.remainingDistance, 0f, 10f);

        if (_distToPlayer > 0 && _distToPlayer< 8)
        {

            DOTween.To(() => _blendValue, X => _blendValue = X, 0.2f, .25f);
        }
        else if (_distToPlayer > 8)
        {
            DOTween.To(() => _blendValue, X => _blendValue = X, 0.5f, .25f);

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
            enemyagent.SetDestination(playerObj.transform.position);
        }
    }
     
   
}
