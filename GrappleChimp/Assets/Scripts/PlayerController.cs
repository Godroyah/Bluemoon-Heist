using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float moveSpeed;
    [SerializeField]
    private float currentSpeed;
    public float sprintSpeed;
    public float fullSpeed;
    public float sneakSpeed;
    [SerializeField]
    private float pickupTimer;
    private float hitLayerWeight;
    public bool noInput;
    public bool hit;
    public bool sneaking;
    public bool sprint;
    public int health;
    public bool pickedUp;
    [HideInInspector]
    public int currentHealth;
    public float fadeDelay = 10.0f;
    public bool dead;
    public bool inAir;
    private float horizontal;
    private float vertical;
    [SerializeField]
    private float jumpTimer;
    public float jumpForce;
    private Rigidbody rb;
    public GameObject attachGemHere;
    public GameObject grappleGun;
    [HideInInspector]
    public Animator playerAnim;
    private Collider meshBoy;
    private Collider thisCollider;
    private LevelBreak levelBreak;
    private GameObject gemObject;
    public Slider healthSlider;
    private Scene menuScene;

    // Use this for initialization
    void Start ()
    {
        menuScene = SceneManager.GetActiveScene();
        levelBreak = GameObject.FindObjectOfType<LevelBreak>();
        gemObject = GameObject.FindGameObjectWithTag("Gem");
        currentHealth = health;
        playerAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        moveSpeed = fullSpeed;
        if(menuScene.name != "Main_Menu")
        {
            this.transform.position = GameObject.FindWithTag("StartSpawn").transform.position;
            this.transform.rotation = GameObject.FindWithTag("StartSpawn").transform.rotation;

        }
        dead = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        pickupTimer -= 0.1f;
        jumpTimer -= 0.1f;
        if (menuScene.name != "Main_Menu")
            healthSlider.value = health;

        if (fadeDelay < -20.0f)
        {
            //Debug.Log("ComingBack!");
            levelBreak.FadeOver();
        }

        if (currentHealth > health)
            {
                hit = true;
                if (hitLayerWeight < 1.0f)
                {
                    hitLayerWeight += 0.1f;
                }
                playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("Hurt"), hitLayerWeight);
                if (hitLayerWeight >= 1.0f)
                {
                    currentHealth = health;
                }
            }
            else if (currentHealth == health)
            {
                if (hitLayerWeight > 0.0f)
                {
                    hitLayerWeight -= 0.01f;
                }
                playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("Hurt"), hitLayerWeight);
            }
            if (currentHealth == health)
            {
                hit = false;
            }

            if (sneaking)
            {
                moveSpeed = sneakSpeed;
            }
            else
            {
                moveSpeed = fullSpeed;
            }

            if (sprint)
            {
                moveSpeed = sprintSpeed;
            }
            else
            {
                moveSpeed = fullSpeed;
            }

            if (health <= 0)
            {
                dead = true;
                hit = false;
                fadeDelay -= 0.1f;
            }

        //if (dead)
        //{
        //    this.transform.position = GameObject.FindWithTag("StartSpawn").transform.position;
        //    this.transform.rotation = GameObject.FindWithTag("StartSpawn").transform.rotation;
        //    dead = false;
        //    health = 5;
        //}

        if(!noInput)
        {
            if (Input.GetButton("Sprint") && !pickedUp)
            {
                sprint = true;
            }
            else
            {
                sprint = false;
            }

            if ((Input.GetButton("Interact") && currentSpeed < 0.1f && pickedUp && pickupTimer < 0) || dead && pickedUp)
            {
                attachGemHere.transform.DetachChildren();
                grappleGun.SetActive(true);
                SphereCollider gemSphere = gemObject.GetComponent<SphereCollider>();
                MeshCollider gemMesh = gemObject.GetComponent<MeshCollider>();
                // Rigidbody gemBody = gemObject.GetComponent<Rigidbody>();
                // gemBody.isKinematic = false;
                gemSphere.enabled = true;
                gemMesh.enabled = true;
                pickupTimer = 3.0f;
                gemObject.transform.position = transform.position + this.transform.forward + new Vector3(0, transform.position.y + 0.25f, 0);
                //gemObject.transform.position = new Vector3(gemObject.transform.position.x, gemObject.transform.position.y - 0.9f, gemObject.transform.position.x);
                gemObject.transform.rotation = new Quaternion(0, 0, 0, 1);
                pickedUp = false;
            }

            if (Input.GetButton("Sneak"))
            {
                sneaking = true;
            }
            else
            {
                sneaking = false;
            }

            if (Input.GetButton("Jump") && !inAir && jumpTimer <= 0)
            {
                jumpTimer = 3.0f;
                rb.AddForce(Vector3.up * jumpForce);
                inAir = true;
            }

            PlayerMovement();
        }
       

            playerAnim.SetBool("Sneaking", sneaking);
            playerAnim.SetBool("Sprinting", sprint);
            playerAnim.SetBool("Dead", dead);
            playerAnim.SetBool("InAir", inAir);
            playerAnim.SetBool("PickedUp", pickedUp);
            playerAnim.SetBool("Hit", hit);
        
    }

    void PlayerMovement()
    {
        currentSpeed = new Vector2(horizontal, vertical).sqrMagnitude;
        
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 playerInput = new Vector3(horizontal, 0f, vertical);
        Vector3 playerMove = Vector3.ClampMagnitude(playerInput, 1.0f) * moveSpeed * Time.deltaTime;
        transform.Translate(playerMove, Space.Self);
        playerAnim.SetFloat("Speed", currentSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            inAir = false;
        }
        if (collision.gameObject.CompareTag("Bandage"))
        {
            if(health < 5)
            {
                Debug.Log("TharSheBlows");
                health += 1;
                Destroy(collision.gameObject);
            }
            else
            {
                Physics.IgnoreCollision(collision.gameObject.GetComponent<SphereCollider>(), GetComponent<CapsuleCollider>());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Idle_box")
        {
            noInput = true;
        }
        else
        {
            noInput = false; 
        }
        thisCollider = other.GetComponent<SphereCollider>();
        meshBoy = other.GetComponent<MeshCollider>();
        if (other.tag == "Gem" && Input.GetButton("Interact") && currentSpeed < 0.1f && !pickedUp && pickupTimer < 0)
        {
            other.transform.parent = attachGemHere.transform;
            pickedUp = true;
            grappleGun.SetActive(false);
            thisCollider.enabled = !thisCollider.enabled;
            meshBoy.enabled = !meshBoy.enabled;
            other.transform.localPosition = new Vector3(0.0044f, 0.0067f, 0.0063f);
            other.transform.localRotation = new Quaternion(-40.76f, -108.4f, -75.29f, 1);
            pickupTimer = 3.0f;
        }
    }
}
