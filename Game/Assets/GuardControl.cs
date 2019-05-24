using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardControl : MonoBehaviour {

    float waitTime;
    float timeLeft;
    Rigidbody guard;

	// Use this for initialization
	void Start () {
        guard = GetComponent<Rigidbody>();
        waitTime = Random.Range(20, 40);
        timeLeft = waitTime;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (timeLeft <= 0) {
            timeLeft = waitTime;
            guard.transform.RotateAround(gameObject.transform.position, Vector3.up, 90f);
        }
        timeLeft -= 0.1f;
	}
}
