using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

    private int gemCount;
    private PlayerControl player;
    public Canvas pauseMenu;
    public Canvas deathScreen;
    public Text textHealth;
    public Text textScore;
    public Text alert;

    bool paused;

    // Use this for initialization
    void Start () {
        gemCount = GameObject.FindGameObjectsWithTag("Gem").Length;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        pauseMenu.enabled = false;
        deathScreen.enabled = false;
        UnPause();
    }
	
	// Update is called once per frame
	void Update () {
        HandlePause();
        UpdateText();
        CheckGems();
        CheckHealth();
    }

    private void CheckHealth() {
        if (player.health == 0) {
            Pause();
            deathScreen.enabled = true;
        }
    }

    private void HandlePause() {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            if (paused) {
                UnPause();
                pauseMenu.enabled = false;
            }
            else {
                Pause();
                pauseMenu.enabled = true;
            }
        }
    }

    public void Pause() {
        Time.timeScale = 0;
        paused = true;
    }

    public void UnPause() {
        Time.timeScale = 1;
        paused = false;
    }

    private void CheckGems() {
        if (gemCount == 0) return;
        if (player.score >= gemCount) {
            if (SceneManager.sceneCountInBuildSettings-1 == SceneManager.GetActiveScene().buildIndex) {
                SceneManager.LoadScene(0);
            }
            else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private void UpdateText() {
        textHealth.text = ("Lives: " + player.health);
        textScore.text = ("Gems " + player.score + "/" + gemCount);
    }

    internal bool IsPaused() {
        return paused;
    }
}
