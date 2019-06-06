using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardControl : MonoBehaviour {

    public static event System.Action OnGuardHasSpottedPlayer;

    public float speed = 5;
    public float turnSpeed = 90;
    public float timeToSpotPlayer = .5f;

    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;
    public bool doFollowPath = true;

    float viewAngle;
    float playerVisibleTimer;

    public Transform pathHolder;
    Transform player;
    PlayerControl playerControl;
    Color originalSpotlightColour;
    Material unalerted;
    private GameControl control;

    private class WayObject {
        public Vector3 location;
        public float waitTime;

        public WayObject(Vector3 location, float waitTime) {
            this.location = location;
            this.waitTime = waitTime;
        }
    }

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").transform;
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        control = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameControl>();
        viewAngle = spotlight.spotAngle;
		originalSpotlightColour = spotlight.color;
        List<WayObject> waypoints = new List<WayObject>();
		for (int i = 0; i < pathHolder.childCount; i++) {
            Vector3 start = pathHolder.GetChild(i).position;
            start.y = transform.position.y;
			waypoints.Add(new WayObject(start, GetWait(pathHolder.GetChild(i))));
		}

        if (waypoints.Count > 1) {
            if (doFollowPath) { 
                StartCoroutine(FollowPath(waypoints));
            }else {
                StartCoroutine(LookAt(waypoints));
            }
        }
	}

    private float GetWait(Transform child) {
        switch (child.tag) {
            case "Wait 5":
                return 5f;
            case "Wait 0.3":
                return 0.3f;
            case "Wait 2":
                return 2f;
            default:
                return 1f;
        }
    }

	void Update() {
        if (control.IsPaused()) return;
        if (CanSeePlayer() && playerControl.IsVisible()) {
            playerControl.health--;
            playerControl.Detected();
        }

	//	if (CanSeePlayer()) {
	//		playerVisibleTimer += Time.deltaTime;
	//	} else {
	//		playerVisibleTimer -= Time.deltaTime;
	//	}
	//	playerVisibleTimer = Mathf.Clamp (playerVisibleTimer, 0, timeToSpotPlayer);
	//	spotlight.color = Color.Lerp (originalSpotlightColour, Color.red, playerVisibleTimer / timeToSpotPlayer);
    //
    //
	//	if (playerVisibleTimer >= timeToSpotPlayer) {
    //       cone.GetComponent<Renderer>().material = alerted;
	//		if (OnGuardHasSpottedPlayer != null) {
	//			OnGuardHasSpottedPlayer();
	//		}
    //   }
    //   else {
    //       cone.GetComponent<Renderer>().material = unalerted;
    //   }
	}

	bool CanSeePlayer() {
		if (Vector3.Distance(transform.position,player.position) < viewDistance) {
			Vector3 dirToPlayer = (player.position - transform.position).normalized;
			float angleBetweenGuardAndPlayer = Vector3.Angle (transform.forward, dirToPlayer);
			if (angleBetweenGuardAndPlayer < viewAngle / 2f) {
				if (!Physics.Linecast (transform.position, player.position, viewMask)) {
					return true;
				}
			}
		}
		return false;
	}

	IEnumerator FollowPath(List<WayObject> waypoints) {
		transform.position = waypoints[0].location;

		int next = 1;
		Vector3 targetWaypoint = waypoints[next].location;
		transform.LookAt (targetWaypoint);

		while (true) {
			transform.position = Vector3.MoveTowards (transform.position, targetWaypoint, speed * Time.deltaTime);
			if (transform.position == targetWaypoint) {
				next = (next + 1) % waypoints.Count;
				targetWaypoint = waypoints[next].location;
				yield return new WaitForSeconds (waypoints[next].waitTime);
				yield return StartCoroutine (TurnToFace (targetWaypoint));
			}
			yield return null;
		}
	}


    private IEnumerator LookAt(List<WayObject> waypoints) {
        int next = 1;
        Vector3 targetWaypoint = waypoints[next].location;
        transform.LookAt(targetWaypoint);

        while (true) {
            next = (next + 1) % waypoints.Count;
            targetWaypoint = waypoints[next].location;
            yield return new WaitForSeconds(waypoints[next].waitTime);
            yield return StartCoroutine(TurnToFace(targetWaypoint));
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget) {
		Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2 (dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

		while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f) {
			float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}
	}

	void OnDrawGizmos() {
        if (pathHolder.childCount > 0) {
            Vector3 startPosition = pathHolder.GetChild(0).position;
            Vector3 previousPosition = startPosition;

            foreach (Transform waypoint in pathHolder) {
                Gizmos.DrawSphere(waypoint.position, .3f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine(previousPosition, startPosition);
        }
		Gizmos.color = Color.red;
		Gizmos.DrawRay (transform.position, transform.forward * viewDistance);
	}

}
