using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemControl : MonoBehaviour {

    private float matrix;

	// Use this for initialization
	void Start () {
        matrix = Random.Range(0.1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward, 20 * matrix * Time.deltaTime);
	}
}
