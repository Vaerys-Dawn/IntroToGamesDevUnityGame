using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

    bool moveLeft = false;
    bool moveRight = false;
    bool moveUp = false;
    bool moveDown = false;
    bool colliding = false;
    bool isSneaking = false;
    public int speed = 20;
    public int score = 0;
    public int health = 20;
    public Text textHealth;
    public Text textScore;
    public Text alert;

    private bool isSpotted = false;

    Rigidbody player;

    private void Start() {
        //SceneManager.LoadScene("Scene");
        player = GetComponent<Rigidbody>();
    }

    private void doMovement() {
        moveLeft = Input.GetKey("a") || Input.GetKey("left");
        moveRight = Input.GetKey("d") || Input.GetKey("right");
        moveUp = Input.GetKey("w") || Input.GetKey("up");
        moveDown = Input.GetKey("s") || Input.GetKey("down");
        isSneaking = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        float moveX = 0;
        float moveZ = 0;

        if (moveLeft) moveX -= 1;
        if (moveRight) moveX += 1;
        if (moveUp) moveZ += 1;
        if (moveDown) moveZ -= 1;

        if (isSneaking) speed = 9;
        else speed = 10;

        Vector3 movement = new Vector3(moveX, 0.6f, moveZ);
        float divisor = Mathf.Sqrt(Mathf.Abs(moveX) + Mathf.Abs(moveZ));
        if (moveX != 0 || moveZ != 0) player.AddForce(movement * (speed / divisor));
    }

    private static void animationThread(Collision col, GameObject game) {

    }

    private void OnCollisionEnter(Collision col) {
        print(col.collider.name);
        switch (col.collider.tag) {
            case "Gem":
                if (!isSpotted) {
                    Destroy(col.gameObject);
                    score++;
                }
                else {
                    health--;
                }
                break;
            case "Enemy":
                if (isSneaking) break;
                else health--;
                break;
        }
    }

    private void OnTriggerEnter(Collider col) {
        switch (col.tag) {
            case "Cone":
                alert.text = "You have been spotted.";
                isSpotted = true;
                break;
        }
    }

    private void OnTriggerExit(Collider other) {
        alert.text = "";
        isSpotted = false;
    }

    private void FixedUpdate() {
        doMovement();
        checkHealth();
        updateText();
    }

    private void updateText() {
        textHealth.text = ("Health: " + health);
        textScore.text = ("Score: " + score);
    }

    private void checkHealth() {
        if (health <= 0) {
            Destroy(gameObject);
            SceneManager.LoadScene("Game_Over");
        }
    }
}
