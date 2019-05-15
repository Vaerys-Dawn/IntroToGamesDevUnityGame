using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {


    bool moveLeft = false;
    bool moveRight = false;
    bool moveUp = false;
    bool moveDown = false;
    public int speed = 20;

    public Rigidbody player;

    void FixedUpdate() {
        moveLeft = Input.GetKey("a") || Input.GetKey("left");
        moveRight = Input.GetKey("d") || Input.GetKey("right");
        moveUp = Input.GetKey("w") || Input.GetKey("up");
        moveDown = Input.GetKey("s") || Input.GetKey("down");

        float moveX = 0;
        float moveZ = 0;

        if (moveLeft) moveX -= 1;
        if (moveRight) moveX += 1;
        if (moveUp) moveZ += 1;
        if (moveDown) moveZ -= 1;

        Vector3 movement = new Vector3(moveX, 0, moveZ);
        float divisor = Mathf.Sqrt(Mathf.Abs(moveX) + Mathf.Abs(moveZ));
        player.AddForce(movement * speed / divisor);
    }
}
