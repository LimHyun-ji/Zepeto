using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridBuildingSystem : MonoBehaviour
{   
    private static GridBuildingSystem _instance;
    public static GridBuildingSystem Instance()
    {
        return _instance;
    }

    public GameObject[] buildingObjects;
    //미리보기용 오브젝트
    private GameObject[] preObjects;
    private Vector3 pos;
    private RaycastHit hitInfo;
    private GameObject pendingObj;
    private int currentIndex;
    public bool isGround;
    [SerializeField] public LayerMask groundLayers;
    [SerializeField] private float gridSize;
    [SerializeField] private float rotateAmount;
    private bool gridOn=true;
    [SerializeField] private Toggle gridToggle;
    [SerializeField] private Vector3 startPos;//int단위로 입력하기
    
    //각 오브젝트의 피벗설정을 해줘야 함
    //각 오브젝트에 박스콜라이더와 이에 따른 스크립트를 붙여줘야 함-->여기서 붙여주면 되지
    private void Awake() 
    {
        _instance=this;

        preObjects=new GameObject[buildingObjects.Length];
        for(int i=0; i < buildingObjects.Length; i++)
        {
            preObjects[i] = Instantiate(buildingObjects[i]);
            preObjects[i].SetActive(false);
        }
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
            if(Input.GetKeyDown(KeyCode.R))
            {
                RotateObject();
            }
        }
    }
    private void FixedUpdate() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hitInfo, 1000, groundLayers))
        {
            //object를 놓을 위치
            pos = hitInfo.point;
            isGround=true;
        }
        else   
            isGround=false;
    }
    //버튼 선택해서 데리고 다니기
    public void SelectObject(int index)
    {
        //pendingObj=Instantiate(objects[index], pos, transform.rotation);
        preObjects[index].SetActive(true);
        preObjects[index].GetComponent<CheckCollision>().ChangeMatColor(preObjects[index].GetComponent<CheckCollision>().materials, "_BaseColor", Color.red);

        currentIndex=index;
        pendingObj=preObjects[index];
        pendingObj.transform.position=pos;
        pendingObj.transform.rotation=buildingObjects[index].transform.rotation;//원본 Prefab의 rotation값으로 불러오기
    }
    private void PlaceObject()
    {
        //설치
        CheckCollision checkCol = pendingObj.GetComponent<CheckCollision>();
        if(!checkCol.canBuild) return;

        var obj = Instantiate(buildingObjects[currentIndex], pendingObj.transform.position, pendingObj.transform.rotation);
        checkCol = obj.GetComponent<CheckCollision>();
        checkCol.ChangeMatColor(checkCol.materials, "_BaseColor", Color.white);

        //스크립트 없애기
        Destroy(checkCol);
        
        //List에 추가하기
        DataManager.Instance().AddObjectInfo(currentIndex, obj.transform.localPosition, obj.transform.localScale, obj.transform.localEulerAngles);

        pendingObj=null;
        preObjects[currentIndex].SetActive(false);

    }
    public void ToggleGrid()
    {
        gridOn = !gridOn;
    }


    //이동(가장 가까운 그리드 자리를 찾아서 배치)
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
    //회전//90도인상태라서 이걸 맞춰줘야 함
    private void RotateObject()
    {
        //마우스 드래그  방향에 따라(추가예정)
        pendingObj.transform.eulerAngles += new Vector3(0, rotateAmount, 0);
    }
}
