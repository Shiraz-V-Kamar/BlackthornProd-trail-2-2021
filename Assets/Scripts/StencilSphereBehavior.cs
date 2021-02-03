using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StencilSphereBehavior : MonoBehaviour
{
    public PlayerMoveNav playerMoveNav;
    public AudioSource heartbeatsource;
    public AudioSource PlayerReactionSoundSource;
    public AudioClip heartbeatsound;
    public AudioClip ReliefSound;
    private Ray ray;
    public LayerMask hurdles;
    private bool _canPlayReliefSound;

    public Vector3 FirePos;

    private bool _NearFire;
    
    private float distToFire;
    [SerializeField]
    private float FireSoundlimit = 5f;

    private void Start()
    { 
        _canPlayReliefSound = true;
        heartbeatsource.clip = heartbeatsound;
        PlayerReactionSoundSource.clip = ReliefSound;
    }

    private void Update()
    {
        if(playerMoveNav.ReachedHome)
        { 
            heartbeatsource.Pause();
            heartbeatsource.volume = 1f;
        }
        distToFire = Vector3.Distance(transform.position, FirePos);
        if (_NearFire)
        {
           
           
            if (distToFire > FireSoundlimit)
            {
                heartbeatsource.DOFade(1f, 1.5f);
                
        
            }
            else if (distToFire < FireSoundlimit)
            {
                heartbeatsource.DOFade(0f, 1.5f);
               
            }
        }else
        {
           

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            if (PlayerReaction != null)
            {
                StopCoroutine(PlayerReaction);
            }
            _canPlayReliefSound = true;
            Debug.Log("enemy detected");
            if (other.gameObject.GetComponent<WolfManager>() != null)
            {
                ray = new Ray(transform.position + Vector3.up, other.gameObject.transform.position);
                float dist = Vector3.Distance(transform.position, other.gameObject.transform.position);
                if (Physics.Raycast(ray, dist, hurdles))
                {
                    return;
                }
                else
                {
                    heartbeatsource.DOPitch(1.5f, 1f);
                    other.gameObject.GetComponent<WolfManager>().SetInPlayerVision(true);
                    other.gameObject.GetComponent<WolfManager>().PassPlayerScript(this.transform.parent.gameObject);
                    other.gameObject.GetComponent<WolfScript>().ObjwithwolfMaterial.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
            }
            else
            {
                Debug.Log("enemy Object doesnt have the scripts attached");
            }
        }
        if(other.tag =="Fire")
        {
            FirePos = other.transform.position;
            _NearFire = true;
        }

        if (other.gameObject.tag == "Dontcheck "|| other.tag == "enemy" || other.tag == "Player" )
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
        if (other.tag != "enemy" )
        {

            other.gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; 
        }
        if (other.tag == "Fire")
        {
            _NearFire = false;
        }
        if (other.tag == "enemy")
        {
           if(_canPlayReliefSound)
            {
                
                PlayerReaction = StartCoroutine(PlayReliefSoundFuction());
            }
            heartbeatsource.DOPitch(1f, 1f);
            other.gameObject.GetComponent<WolfScript>().ObjwithwolfMaterial.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            other.gameObject.GetComponent<WolfManager>().SetInPlayerVision(false);
        }
    }
    Coroutine PlayerReaction;
    

    IEnumerator PlayReliefSoundFuction()
    {
        _canPlayReliefSound = false;
        yield return new WaitForSeconds(2f);
        PlayerReactionSoundSource.Play();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(ray);
    }

    public void FadeHeartbeat()
    {
        heartbeatsource.DOFade(0f, 2f);
    }
}
