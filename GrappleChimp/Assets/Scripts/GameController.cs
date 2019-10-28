using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

    private BoxCollider winnaBox;
    public bool won = false;
    public float winnaCountDown = 30.0f;
    private PlayerController playerController;
    public GameObject player;
    //public Animator levelAnim;
    public Text winText;

    // Use this for initialization
    void Start()
    {
        winnaBox = GetComponent<BoxCollider>();
        playerController = player.GetComponent<PlayerController>();
        winText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (won == true)
        {
            //Debug.Log("You Won");
            winnaCountDown -= 0.1f;
            winText.enabled = true;
            if (winnaCountDown <= 0.0f)
            {
                Debug.Log("BackToMenu");
                SceneManager.LoadScene("Main_Menu");
            }
                
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if ((col.gameObject.tag == "Player" || col.gameObject.tag == "Gem") && playerController.pickedUp)
        {
            Debug.Log("Collided!");
            won = true;
        }
        winnaBox.enabled = false;

    }
}
