using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardControl : MonoBehaviour {

    float waitTime;
    float timeLeft;
    Rigidbody guard;
    float facing;
    float newAngel;

	// Use this for initialization
	void Start () {
        guard = GetComponent<Rigidbody>();
        waitTime = Random.Range(20, 40);
        timeLeft = waitTime;
        facing = gameObject.transform.eulerAngles.y;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        while (facing != newAngel) {
            facing += 3;
            if (facing < newAngel) {
                facing = newAngel;
            }
            Vector3 angle = guard.transform.eulerAngles;
            guard.transform.rotation.SetEulerAngles(angle.x, facing, angle.y);
        }
        if (timeLeft <= 0) {
            timeLeft = waitTime;
            guard.transform.RotateAround(gameObject.transform.position, Vector3.up, 90f);
        }
        timeLeft -= 0.1f;
	}
}
