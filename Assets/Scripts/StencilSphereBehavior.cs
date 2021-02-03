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

    public LayerMask enemyLayerMask;

    private Vector3 FirePos;

    private bool _NearFire;
    
    private float distToFire;
    private float SphereOverlapRadius;
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
        if(playerMoveNav.visionItensityValueHolder > 0.7)
        {
            DOTween.To(() => SphereOverlapRadius, x => SphereOverlapRadius = x, 8.5f, 1.5f);
        }else if (playerMoveNav.visionItensityValueHolder  < 0.7)
        {
            DOTween.To(() => SphereOverlapRadius, x => SphereOverlapRadius = x, 17.5f, 1.5f);
        }

        Collider[] hitcolliders = Physics.OverlapSphere(transform.position,SphereOverlapRadius,enemyLayerMask);
        if (hitcolliders.Length>0)
        {
            for (int i = 0; i < hitcolliders.Length; i++)
            {
                if (hitcolliders[i].gameObject.tag == "enemy")
                {
                    Vector3 direction = hitcolliders[i].transform.position - transform.position;
                    direction.y = 0f;
                    direction.Normalize();
                    ray = new Ray(transform.position + Vector3.up, direction);

                    float dist = Vector3.Distance(transform.position, hitcolliders[i].gameObject.transform.position);
                    if (Physics.Raycast(ray, dist, hurdles))
                    {
                        return;
                    }
                    else
                    {
                        hitcolliders[i].gameObject.GetComponent<WolfManager>().SetInPlayerVision(true);
                        hitcolliders[i].gameObject.GetComponent<WolfManager>().PassPlayerScript(this.transform.parent.gameObject);
                        hitcolliders[i].gameObject.GetComponent<WolfScript>().ObjwithwolfMaterial.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

                        heartbeatsource.DOPitch(1.5f, 1f);
                    }
                    if(playerMoveNav.ReachedHome)
                    {
                        hitcolliders[i].gameObject.GetComponent<WolfScript>().playWolfHowlSound = false;
                        hitcolliders[i].gameObject.GetComponent<WolfScript>().WolfGrowlSource.Stop() ;
                    }
                }
              
            }

        }
        else
        {
            if (_canPlayReliefSound)
            {
                PlayerReaction = StartCoroutine(PlayReliefSoundFuction());
            }
            heartbeatsource.DOPitch(1f, 1f);
        }      
       
        if(playerMoveNav.ReachedHome)
        {
            FadeHeartbeat();
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

    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, SphereOverlapRadius);
    }
}
