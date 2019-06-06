using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidControl : MonoBehaviour {

    private PlayerControl player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    private void OnCollisionEnter(Collision col) {
        switch (col.collider.tag) {
            case "Player":
                player.Void();
                player.health--;
                break;
            case "Pillar":
                break;
            default:
                Destroy(col.collider.gameObject);
                break;
        }
        
    }
}
