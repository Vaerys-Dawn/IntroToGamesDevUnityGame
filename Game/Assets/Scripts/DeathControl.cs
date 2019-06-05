using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathControl : MonoBehaviour {

    public Button restart;

	// Use this for initialization
	void Start () {
        restart.onClick.AddListener(() => Restart());
    }

    private void Restart() {
        SceneManager.LoadScene(0);
    }
}
