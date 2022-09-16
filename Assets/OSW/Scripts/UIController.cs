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
    public Sprite iconWindow;
    Controls controls;

    GameObject toolUI;
    GameObject toolTextUI;
    GameObject controlsUI;

    void Start () 
    {
        controls = FindObjectOfType<Controls>();

        // Canvas의 tool 가져오기
        toolUI = transform.GetChild(0).gameObject;
        // Canvas의 toolText 가져오기
        toolTextUI = transform.GetChild(1).gameObject;
        // ControlsTutorial의 controls 가져오기
        controlsUI = controlsTutorial.transform.GetChild(1).gameObject;
	}
	
	void Update () 
    {
		
	}

    // drawMode 에 따른 아이콘 변경
    public void UpdateIcon() 
    {
        if (controls.drawMode == Controls.DrawMode.WALL) 
        {
            toolImage.sprite = iconWall;
        } 
        else if (controls.drawMode == Controls.DrawMode.FLOOR)
        {
            toolImage.sprite = iconFloor;
        } 
        else if (controls.drawMode == Controls.DrawMode.WINDOW)
        {
            toolImage.sprite = iconWindow;
        }
        else if (controls.drawMode == Controls.DrawMode.DELETE) 
        {
            toolImage.sprite = iconHammer;
        }
        else 
        {
            Debug.Log("no drawmode found in UIController");
        }
    }

    // 아이콘 숨기는 기능
    public void HideControls() 
    {
        Debug.Log("Hide");
        //controlsTutorial.SetActive(false);
        toolUI.SetActive(false);
        toolTextUI.SetActive(false);
        controlsUI.SetActive(false);
    }

    // 아이콘 다시 보이게 하는 기능
    public void DisplayControls()
    {
        Debug.Log("Display");
        //controlsTutorial.SetActive(true);
        toolUI.SetActive(true);
        toolTextUI.SetActive(true);
        controlsUI.SetActive(true);
    }
}
