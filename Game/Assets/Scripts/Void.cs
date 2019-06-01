using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : MonoBehaviour {

    public PlayerControl player;

    private void OnCollisionEnter(Collision col) {
        switch (col.collider.tag) {
            case "Player":
                player.Respawn();
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
