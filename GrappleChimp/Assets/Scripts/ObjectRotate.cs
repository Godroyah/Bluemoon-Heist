using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour {

    private GameObject player;
    private PlayerController playerController;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
	}

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.tag == "Gem" && !playerController.pickedUp)
        {
            this.transform.Rotate(0, 1, 0, Space.World);
        }
        else if(this.gameObject.tag == "Bandage")
        {
            this.transform.Rotate(0, 1, 0, Space.World);
        }
    }
}
