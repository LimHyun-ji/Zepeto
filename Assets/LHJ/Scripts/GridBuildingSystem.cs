using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridBuildingSystem : MonoBehaviour
{   
    public GameObject[] objects;
    private Vector3 pos;
    private RaycastHit hitInfo;
    private GameObject pendingObj;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float gridSize;
    private bool gridOn=true;
    [SerializeField] private Toggle gridToggle;
    [SerializeField] private Vector3 startPos;//int단위로 입력하기
    
    //각 오브젝트의 피벗설정을 해줘야 함(모서리로)
    //각 오브젝트에 박스콜라이더와 이에 따른 스크립트를 붙여줘야 함
    private void Awake() 
    {
        Grid3D grid=new Grid3D(20,20, gridSize, startPos);
    }
    private void Update() 
    {
        if(pendingObj != null)
        {
            if(gridOn)
            {
                pendingObj.transform.position=new Vector3(
                        SetToNearestGrid(pos.x),
                        SetToNearestGrid(pos.y),
                        SetToNearestGrid(pos.z));
            }
            else
            {
                pendingObj.transform.position=pos;
            }
            if(Input.GetMouseButtonDown(0))
            {
                PlaceObject();
            }
        }
    }
    private void FixedUpdate() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hitInfo, 1000, groundLayers))
        {
            pos = hitInfo.point;
        }
    }
    public void SelectObject(int index)//선택해서 데리고 다니기
    {
        pendingObj=Instantiate(objects[index], pos, transform.rotation);
    }
    private void PlaceObject()
    {
        pendingObj=null;
    }
    public void ToggleGrid()
    {
        gridOn = !gridOn;
    }

    private float SetToNearestGrid(float pos)
    {
        float xDiff = pos % gridSize;
        pos -=xDiff;
        if(xDiff>gridSize/2)
        {
            pos += gridSize;
        }
        return pos;
    }
}
