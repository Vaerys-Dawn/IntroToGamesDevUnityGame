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
    bool isSneaking = false;
    private float speed = 20;
    public int score = 0;
    public int health = 20;
    public Vector3 spawn;
   

    private bool isSpotted = false;

    Rigidbody player;

    private void Start() {
        spawn = gameObject.transform.position;
        player = GetComponent<Rigidbody>();
    }

    private void DoMovement() {
        if (!IsGrounded()) {
            return;
        }

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

        if (isSneaking) speed = 150;
        else speed = 300;

        Vector3 currentVelocity = player.velocity;
        //get vector for the direction
        Vector3 movement = new Vector3(moveX, 0, moveZ);
        //get the divisor
        float divisor = Mathf.Sqrt(Mathf.Abs(moveX) + Mathf.Abs(moveZ));
        //math...
        if (moveX != 0 || moveZ != 0) {
            player.velocity = movement * (speed / divisor) * Time.deltaTime;
        }
        else {
            player.velocity = new Vector3(player.velocity.x * 0.8f, 0, player.velocity.z * 0.8f);
        }
        player.velocity = new Vector3(player.velocity.x, currentVelocity.y, player.velocity.z);

        
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
                    Respawn();
                    health--;
                }
                break;
            case "Enemy":
                if (isSneaking) break;
                else health--;
                break;
        }
    }

    bool IsGrounded() {
        bool corner1 = Physics.Raycast(new Vector3(player.position.x - 0.5f, player.position.y, player.position.z - 0.5f), Vector3.down, 0.6f);
        bool corner2 = Physics.Raycast(new Vector3(player.position.x + 0.5f, player.position.y, player.position.z - 0.5f), Vector3.down, 0.6f);
        bool corner3 = Physics.Raycast(new Vector3(player.position.x + 0.5f, player.position.y, player.position.z + 0.5f), Vector3.down, 0.6f);
        bool corner4 = Physics.Raycast(new Vector3(player.position.x - 0.5f, player.position.y, player.position.z + 0.5f), Vector3.down, 0.6f);
        return (corner1 || corner2 || corner3 || corner4);
    }

    private void FixedUpdate() {
        DoMovement();
        CheckHealth();
    }

    private void CheckHealth() {
        if (health <= 0) {
            Destroy(gameObject);
            SceneManager.LoadScene("Game_Over");
        }
    }

    internal void Respawn() {
        gameObject.transform.position = spawn;
    }
}
