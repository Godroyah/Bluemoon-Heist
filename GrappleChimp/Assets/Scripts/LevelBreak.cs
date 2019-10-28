using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelBreak : MonoBehaviour {

    public Animator animator;
    private PlayerController playerController;
    private GameObject player;
    private GameController gameController;
    //private Animator playerAnimator;
    public Slider healthSlider;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = GameObject.FindObjectOfType<PlayerController>();
        gameController = GameObject.FindObjectOfType<GameController>();
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((playerController.dead == true && playerController.fadeDelay <= 0.0f) || gameController.winnaCountDown <= 5.0f)
        {
            FadeOnDeath();
        }
    }

    public void FadeOnDeath()
    {
        //Debug.Log("FadeTime!");
        animator.SetBool("Fade", true);
    }

    public void FadeOver()
    {
        player.transform.position = GameObject.FindWithTag("StartSpawn").transform.position; ;
        player.transform.rotation = GameObject.FindWithTag("StartSpawn").transform.rotation; ;
        playerController.health = 5;
        playerController.currentHealth = playerController.health;
        healthSlider.value = playerController.health;
        playerController.dead = false;
        animator.SetBool("Fade", false);
        playerController.fadeDelay = 10.0f;
    }
}
