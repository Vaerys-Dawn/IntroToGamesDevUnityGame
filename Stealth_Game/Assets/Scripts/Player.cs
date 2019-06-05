using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public event System.Action OnReachEndOfLevel;

	public float moveSpeed = 7;
	public float smoothMoveTime = .1f;
	public float turnSpeed = 8;

	float angle;
	float smoothInputPressed;
	float smoothMoveVelocity;
	Vector3 velocity;

	Rigidbody skeleton;
	bool CantMove;

	void Start() {
		skeleton = GetComponent<Rigidbody> ();
		Guard.OnGuardHasSpottedPlayer += Disable;
	}

	void Update () {
		Vector3 inputDirection = Vector3.zero;
		if (!CantMove) {
			inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")).normalized;
		}

		float inputPressed = inputDirection.magnitude;
		smoothInputPressed = Mathf.SmoothDamp(smoothInputPressed, inputPressed, ref smoothMoveVelocity, smoothMoveTime);
		float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
		angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputPressed);

		velocity = transform.forward * moveSpeed * smoothInputPressed;
	}
	void OnTriggerEnter(Collider hitCollider) {
		if (hitCollider.tag == "Finish") {
			Disable ();
			if (OnReachEndOfLevel != null){
				OnReachEndOfLevel ();
			}
		}
	}

	void Disable() {
		CantMove = true;
	}

	void FixedUpdate(){
		skeleton.MoveRotation(Quaternion.Euler(Vector3.up * angle));
		skeleton.MovePosition(skeleton.position + velocity * Time.deltaTime);
	}

	void OnDestroy () {
		Guard.OnGuardHasSpottedPlayer -= Disable;
	}
}
