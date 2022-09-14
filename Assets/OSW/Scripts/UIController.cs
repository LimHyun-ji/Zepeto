using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Building System Tool UI
public class UIController : MonoBehaviour {

    public Image toolImage;
    public GameObject controlsTutorial;
    public Sprite iconHammer;
    public Sprite iconWall;
    public Sprite iconFloor;
    Controls controls;

	void Start () 
    {
        controls = FindObjectOfType<Controls>();
	}
	
	void Update () 
    {
		
	}

    public void UpdateIcon() 
    {
        if (controls.drawMode == Controls.DrawMode.WALL) {
            toolImage.sprite = iconWall;
        } else if (controls.drawMode == Controls.DrawMode.FLOOR) {
            toolImage.sprite = iconFloor;
        } else if (controls.drawMode == Controls.DrawMode.DELETE) {
            toolImage.sprite = iconHammer;
        } else {
            Debug.Log("no drawmode found in UIController");
        }
    }

    public void HideControls() 
    {
        Debug.Log("use");
        controlsTutorial.SetActive(false);
    }
}
