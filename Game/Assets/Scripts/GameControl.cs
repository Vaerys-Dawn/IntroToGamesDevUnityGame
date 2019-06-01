using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

    private int gemCount;
    private PlayerControl player;
    private Canvas pauseMenu;
    public Text textHealth;
    public Text textScore;
    public Text alert;
    bool paused;

    // Use this for initialization
    void Start () {
        gemCount = GameObject.FindGameObjectsWithTag("Gem").Length;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        pauseMenu = GameObject.FindGameObjectWithTag("Pause").GetComponent<Canvas>();
        pauseMenu.enabled = false;
        UnPause();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            if (paused) UnPause();
            else Pause();
        }
        if (!paused) {
            UpdateText();
            CheckGems();
        }
	}

    private void Pause() {
        Time.timeScale = 0;
        pauseMenu.enabled = true;
        paused = true;
    }

    private void UnPause() {
        Time.timeScale = 1;
        pauseMenu.enabled = false;
        paused = false;
    }

    private void CheckGems() {
        if (player.score >= gemCount) {
            if (SceneManager.sceneCountInBuildSettings == SceneManager.GetActiveScene().buildIndex) {
                SceneManager.LoadScene(0);
            }
            else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private void UpdateText() {
        textHealth.text = ("Health: " + player.health);
        textScore.text = ("Gems " + player.score + "/" + gemCount);
    }
}
