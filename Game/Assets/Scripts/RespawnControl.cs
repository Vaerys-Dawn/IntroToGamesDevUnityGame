using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnControl : MonoBehaviour {

    private PlayerControl player;


	// Use this for initialization
	void Start () {
        print(enabled);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Canvas>().enabled == false) return;
        bool respawn = Input.GetKey(KeyCode.Space);
        if (respawn) {
            player.Respawn();
        }
	}
}
