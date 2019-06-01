using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseControl : MonoBehaviour {

    public Button hub;
    public Button restart;
    
	// Use this for initialization
	void Start () {
        hub.onClick.AddListener(() => GoToHub());
        restart.onClick.AddListener(() => Restart());
    }

    private void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GoToHub() {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
