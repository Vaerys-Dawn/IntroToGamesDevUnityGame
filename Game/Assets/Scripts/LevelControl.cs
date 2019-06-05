using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour {

    public int sceneID;

    private void OnTriggerEnter(Collider other) {
        switch (other.tag) {
            case "Player":
                SceneManager.LoadScene(sceneID);
                break;
        }

    }
}
