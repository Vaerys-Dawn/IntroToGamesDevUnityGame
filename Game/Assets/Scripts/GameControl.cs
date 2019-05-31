using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

    private int gemCount;
    private PlayerControl player;
    public Text textHealth;
    public Text textScore;
    public Text alert;

    // Use this for initialization
    void Start () {
        gemCount = GameObject.FindGameObjectsWithTag("Gem").Length;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateText();
        CheckGems();
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
