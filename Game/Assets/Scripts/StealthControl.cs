using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StealthControl : MonoBehaviour {

    public Image tint;
    public Image stealthBar;
    private Canvas stealthUI; 
    private PlayerControl player;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        stealthUI = gameObject.GetComponent<Canvas>();
        stealthUI.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        HandleStealth();
	}

    private void HandleStealth() {
        if (player.resource < player.maxResource) {
            stealthUI.enabled = true;
            RectTransform rectTransform = stealthBar.GetComponent<RectTransform>();
            float size = (player.resource / player.maxResource) * 200;
            rectTransform.sizeDelta = new Vector2(size, rectTransform.sizeDelta.y);
        }
        else {
            stealthUI.enabled = false;
        }

        if (player.IsVisible()) {
            tint.enabled = false;
        }
        else {
            tint.enabled = true;
        }
    }
}
