using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseControl : MonoBehaviour {

    public Button hub;
    public Button restart;
    public Button exit;

	// Use this for initialization
	void Start () {
        hub.onClick.AddListener(() => GoToHub());
        restart.onClick.AddListener(() => Restart());
        exit.onClick.AddListener(() => Exit());
    }

    private void Exit() {
        Application.Quit();
    }

    private void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GoToHub() {
        SceneManager.LoadScene(0);
    }


}
