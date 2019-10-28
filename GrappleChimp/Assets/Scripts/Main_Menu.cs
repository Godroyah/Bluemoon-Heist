using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class Main_Menu : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject playMenu;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(Cursor.lockState);	
	}

    public void Play()
    {
        SceneManager.LoadScene("testinggreybox");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        mainMenu.SetActive(false);
        playMenu.SetActive(true);
    }

    public void Back()
    {
        playMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

}
