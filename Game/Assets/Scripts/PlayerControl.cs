using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

    public bool isSneaking = false;
    public int score = 0;
    public int health;
    public float resource;
    public float maxResource = 200;
    public Material visible;
    public Material hidden;

    public Canvas checkpointCanvas;
    public Canvas respawnCanvas;
    public Text respawnText;

    private bool coolDown;
    private float speed = 20;
    private Vector3 spawn;
    private bool isSpotted = false;
    private Rigidbody player;
    private int checkCountDown = 0;
    private GameControl control;


    private void Start() {
        spawn = gameObject.transform.position;
        player = GetComponent<Rigidbody>();
        resource = maxResource;
        checkpointCanvas.enabled = false;
        respawnCanvas.enabled = false;
        control = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameControl>();
    }

    private void DoMovement() {
        if (!IsGrounded()) {
            return;
        }

        bool moveLeft = false;
        bool moveRight = false;
        bool moveUp = false;
        bool moveDown = false;

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

    internal bool IsVisible() {
        return !isSneaking;
    }

    private void OnCollisionEnter(Collision col) {
        if (isSneaking) return;
        switch (col.collider.tag) {
            case "Gem":
                Destroy(col.gameObject);
                score++;
                break;
            case "Enemy":
                Detected();
                health--;
                break;
        }
    }

    private void OnTriggerEnter(Collider other) {
        switch (other.tag) {
            case "CheckPoint":
                Vector3 newSpawn = new Vector3(other.transform.position.x, player.position.y, other.transform.position.z);
                if (Vector3.Distance(spawn, newSpawn) > 1) {
                    spawn = newSpawn;
                    ShowCheckPoint();
                }
                break;
        }
    }

    private void ShowCheckPoint() {
        if(checkCountDown == 0) checkCountDown = 100;
    }

    bool IsGrounded() {
        float width = gameObject.transform.localScale.x / 2;
        float height = gameObject.transform.localScale.y / 2 + 0.1f;
        bool corner1 = Physics.Raycast(new Vector3(player.position.x - width, player.position.y, player.position.z - width), Vector3.down, height);
        bool corner2 = Physics.Raycast(new Vector3(player.position.x + width, player.position.y, player.position.z - width), Vector3.down, height);
        bool corner3 = Physics.Raycast(new Vector3(player.position.x + width, player.position.y, player.position.z + width), Vector3.down, height);
        bool corner4 = Physics.Raycast(new Vector3(player.position.x - width, player.position.y, player.position.z + width), Vector3.down, height);
        return (corner1 || corner2 || corner3 || corner4);
    }

    private void FixedUpdate() {
        DoMovement();
        CheckVisible();

        if(checkCountDown > 0) {
            checkpointCanvas.enabled = true;
            checkCountDown--;
        }else {
            checkpointCanvas.enabled = false;
        }
    }

    private void CheckVisible() {
        if (isSneaking) {
            gameObject.GetComponent<Renderer>().material = hidden;
        }
        else {
            gameObject.GetComponent<Renderer>().material = visible;
        }
    }

    internal void Detected() {
        control.Pause();
        respawnCanvas.enabled = true;
    }

    internal void Void() {
        control.Pause();
        respawnText.text = "You Died...";
        respawnCanvas.enabled = true;
    }

    internal void Respawn() {
        gameObject.transform.position = spawn;
        respawnCanvas.enabled = false;
        control.UnPause();
        respawnText.text = "Detected!";
    }
}
