using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarControl : MonoBehaviour {

    Rigidbody pillar;
    private Vector3 spawn;
    private PlayerControl player;

    public bool isBox = true;

	// Use this for initialization
	void Start () {
        spawn = gameObject.transform.position;
        pillar = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }
	
	// Update is called once per frame
	void Update () {
		if (!IsGrounded()) {
            pillar.velocity = new Vector3(0, -10, 0);
        }
	}

    private void OnCollisionEnter(Collision col) {
        switch (col.collider.tag) {
            case "Enemy":
                Respawn();
                break;
        }
        if (isBox && Vector3.Distance(player.transform.position, transform.position) < 0.6) {
            player.health--;
            player.Void();
        }
    }

    private void Respawn() {
        gameObject.transform.position = spawn;
    }

    bool IsGrounded() {
        float width = gameObject.transform.localScale.x/2;
        float height = gameObject.transform.localScale.y/2 + 0.1f;
        bool corner1 = Physics.Raycast(new Vector3(pillar.position.x - width, pillar.position.y, pillar.position.z - width), Vector3.down, height);
        bool corner2 = Physics.Raycast(new Vector3(pillar.position.x + width, pillar.position.y, pillar.position.z - width), Vector3.down, height);
        bool corner3 = Physics.Raycast(new Vector3(pillar.position.x + width, pillar.position.y, pillar.position.z + width), Vector3.down, height);
        bool corner4 = Physics.Raycast(new Vector3(pillar.position.x - width, pillar.position.y, pillar.position.z + width), Vector3.down, height);
        return (corner1 || corner2 || corner3 || corner4);
    }
}
