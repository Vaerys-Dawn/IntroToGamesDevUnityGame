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
    public bool isSneaking = false;
    private float speed = 20;
    private Vector3 spawn;

    public int score = 0;
    public int health;
    public float resource;
    public float maxResource = 200;

    public Material visible;
    public Material hidden;
    private bool coolDown;

    private bool isSpotted = false;

    Rigidbody player;


    private void Start() {
        spawn = gameObject.transform.position;
        player = GetComponent<Rigidbody>();
        resource = maxResource;
    }

    private void DoMovement() {
        if (!IsGrounded()) {
            return;
        }

        moveLeft = Input.GetKey("a") || Input.GetKey("left");
        moveRight = Input.GetKey("d") || Input.GetKey("right");
        moveUp = Input.GetKey("w") || Input.GetKey("up");
        moveDown = Input.GetKey("s") || Input.GetKey("down");

        HandleStealth();

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

    private void HandleStealth() {
        bool shiftHeld = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
        bool released = (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift));

        isSneaking = (shiftHeld && !coolDown);

        if (!shiftHeld && resource < maxResource) coolDown = true;
        if (!coolDown && isSneaking && resource > 0) resource--;
        if (coolDown && resource < maxResource) resource++;
        if (resource == maxResource) coolDown = false;
        if (resource == 0) coolDown = true;

    }

    internal bool isVisible() {
        return !isSneaking;
    }

    private void OnCollisionEnter(Collision col) {
        if (isSneaking) return;
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
        CheckVisible();
    }

    private void CheckVisible() {
        if (isSneaking) {
            gameObject.GetComponent<Renderer>().material = hidden;
        }
        else {
            gameObject.GetComponent<Renderer>().material = visible;
        }
    }

    internal void Respawn() {
        gameObject.transform.position = spawn;
    }
}
