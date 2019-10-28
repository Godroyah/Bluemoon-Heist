using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Grappler : MonoBehaviour {

    public RawImage bullsEye;
    public Camera mainCam;
    public float maxGrappleDist;
    public float pullSpeed;
    [SerializeField]
    private float aimLayerWeight;
    private Animator playerAnim;
    private PlayerController playerController;
    private int layerMask = 1 << 9;

    private Vector3 grappleTarget;
    [SerializeField]
    private bool aiming = false;
    public bool grappled = false;


	// Use this for initialization
	void Start ()
    {
        playerAnim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        layerMask = ~layerMask;
        if (Input.GetMouseButton(1) && !grappled && !playerController.noInput)
        {
            aiming = true;
        }
        else
        {
            aiming = false;
        }

        if(aiming)
        {
            if(aimLayerWeight < 1.0f)
            {
                aimLayerWeight += 0.1f;
            }
            playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("Aiming"), aimLayerWeight);
        }
        else
        {
            if(aimLayerWeight > 0.0f)
            {
                aimLayerWeight -= 0.1f;
            }
            playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("Aiming"), aimLayerWeight);
        }

        GrappleRay();

        if(grappled && !playerController.pickedUp)
        {
            PlayerGrappled();
        }
        playerAnim.SetBool("Aiming", aiming);
        playerAnim.SetBool("Grappled", grappled);
    }

    private void PlayerGrappled()
    {
        transform.position = Vector3.Lerp(transform.position, grappleTarget, pullSpeed * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, grappleTarget);

        if(dist <= 2.5f)
        {
            grappled = false;
        }
    }

    private void GrappleRay()
    {
        Ray grappleRay = mainCam.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(mainCam.transform.position, Input.mousePosition, Color.red);
        RaycastHit hit;

        if(Physics.Raycast(grappleRay, out hit, maxGrappleDist, layerMask))
        {
            Debug.Log("Fire");
            if (hit.collider.CompareTag("GrappleTarget"))
            {
                bullsEye.color = Color.green;
                if (Input.GetMouseButtonUp(0) && aiming)
                {
                    grappleTarget = hit.point;
                    grappled = true;
                    playerController.inAir = true;
                    aiming = false;
                }
            }
            else
            {
                bullsEye.color = Color.red;
            }
        }
    }
}
