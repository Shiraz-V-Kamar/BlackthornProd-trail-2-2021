using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerAnimControl : MonoBehaviour
{
    float speed = 0.5f;
    float rotspeed = 5;
    float gravity = 10;
    float rot = 0; //rot is y value for player to turn around towards
    Vector3 moveDir = new Vector3 (0, 0, 0);
    Vector3 worldPos;
    Vector3 displacement; //player pos to mousepoint distance
    Quaternion playerDirection;
    //float castdist;
    public Camera cam;
    public Transform camMoveDirection;


    CharacterController controller;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController> ();
        anim = GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
        //float playerZ = camMoveDirection.z;
        /* Vector3 mousePos = Input.mousePosition;
         cam = gameObject.GetComponent<Camera>(); //new method of accessing camera
         mousePos.z = cam.nearClipPlane; //depth of the raycast
         worldPos = cam.ScreenToWorldPoint(mousePos);

         Plane plane = new Plane(Vector3.up, 0);

         Ray ray = cam.ScreenPointToRay(mousePos);
         float castdist;
         if (plane.Raycast(ray, out castdist))
         {
             worldPos = ray.GetPoint(castdist);
         }*/

        //rot += Input.GetAxis("Mouse X") * rotspeed * Time.deltaTime;
        //transform.eulerAngles = new Vector3(0, rot + camMoveDirection.eulerAngles.y, 0);

        if (controller.isGrounded)
        {
            
            if (Input.GetKeyDown (KeyCode.Mouse0))
            {
                anim.SetInteger ("isWalking", 1);
                moveDir = new Vector3(0, 0, 1);
                moveDir *= speed;
                moveDir = transform.TransformDirection (moveDir);
                
            }
            else if (Input.GetKeyDown (KeyCode.Mouse1))
            {
                moveDir = new Vector3(0, 0, -1);
                moveDir *= speed;
            }

            if (Input.GetKeyUp (KeyCode.Mouse0))
            {
                anim.SetInteger ("isWalking", 0);
                moveDir = new Vector3(0, 0, 0);
            }
            else if (Input.GetKeyUp (KeyCode.Mouse1))
            {
                moveDir = new Vector3(0, 0, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            anim.SetInteger("isCrouching", 1);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            anim.SetInteger("isCrouching", 0);
        }
        /*displacement = (worldPos - moveDir).normalized;
        playerDirection = Quaternion.LookRotation(displacement);
        transform.rotation = Quaternion.Slerp(transform.rotation, playerDirection, Time.deltaTime * rotspeed);*/

        /* displacement = (worldPos - moveDir);
         transform.rotation = Quaternion.FromToRotation(Vector3.up, displacement);*/

        rot += Input.GetAxis("Mouse X") * rotspeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rot + camMoveDirection.eulerAngles.y, 0);

        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);

    }
}
