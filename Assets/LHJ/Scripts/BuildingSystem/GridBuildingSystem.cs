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
    [SerializeField] public LayerMask objectLayers;
    [SerializeField] private float gridSize;
    [SerializeField] private float rotateAmount;
    [SerializeField] private float scaleAmount=0.2f;

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
            buildingObjects[i].GetComponent<MovableFurniture>().myIndex=i;
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
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                ScaleObject(scaleAmount);
            }
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                ScaleObject(-scaleAmount);
            }
            if(Input.GetKeyDown(KeyCode.Delete))
            {
                RemoveObject();
            }
        
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                ReselectObject();
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
    }
    //버튼 선택해서 데리고 다니기
    public void SelectObject(int index)
    {
        //pendingObj=Instantiate(objects[index], pos, transform.rotation);
        preObjects[index].SetActive(true);
        preObjects[index].GetComponent<MovableFurniture>().ChangeMatColor(preObjects[index].GetComponent<MovableFurniture>().materials, "_BaseColor", Color.blue);

        currentIndex=index;
        pendingObj=preObjects[index];
        pendingObj.transform.position=pos;
        pendingObj.transform.rotation=buildingObjects[index].transform.rotation;//원본 Prefab의 rotation값으로 불러오기
    }
    public void ReselectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log("ResSelect");

        if(Physics.Raycast(ray, out hitInfo, 1000, objectLayers))
        {
            Debug.Log("Ray");
            //int _index = hitInfo.transform.gameObject.GetComponent<MovableFurniture>().myIndex;
            //SelectObject(_index);
            pendingObj = hitInfo.transform.gameObject;
            //pendingObj.GetComponent<MovableFurniture>().enabled=true;
            MovableFurniture moveComp = pendingObj.GetComponent<MovableFurniture>();
            moveComp.enabled=true;
            moveComp.isPlaced=false;
            moveComp.ChangeMatColor(moveComp.materials, "_BaseColor", Color.blue);
        }
    }
    private void PlaceObject()
    {
        //설치
        MovableFurniture checkCol = pendingObj.GetComponent<MovableFurniture>();
        if(!checkCol.canBuild) return;

        GameObject obj;
        if(checkCol.isInit)
        {

            obj = Instantiate(buildingObjects[currentIndex], pendingObj.transform.position, pendingObj.transform.rotation);
            obj.name += Random.Range(0, 99).ToString();
        }
        else
        {
            //재선택이면
            obj=pendingObj;
        }

        checkCol = obj.GetComponent<MovableFurniture>();
        checkCol.ChangeMatColor(checkCol.materials, "_BaseColor", Color.white);

        //스크립트 없애기
        //Destroy(checkCol);
        checkCol.isPlaced=true;
        checkCol.isInit=false;
        checkCol.enabled=false;
        //obj.GetComponent<MovableFurniture>().enabled=false;

        //List에 추가하기
        //DataManager.Instance().AddObjectInfo(obj.GetHashCode(),currentIndex, obj.transform.localPosition, obj.transform.localScale, obj.transform.localEulerAngles);

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
     private void ScaleObject(float scaleAmount)
    {
        pendingObj.transform.transform.localScale += Vector3.one * scaleAmount;
    }
    private void RemoveObject()
    {
        MovableFurniture checkCol = pendingObj.GetComponent<MovableFurniture>();
        if(checkCol.isPlaced)//재선택이면
            Destroy(pendingObj);
        else
            pendingObj.SetActive(false);

        pendingObj=null;
    }
    public void GetMovableObj()
    {
        //저장할 때마다 아에 지우고 새로 담아주기
        DataManager.Instance().objInfoList.Clear();
        GameObject[] movableObjs = GameObject.FindGameObjectsWithTag("MovableObject");
        foreach(GameObject obj in movableObjs)
        {
            int _index = obj.GetComponent<MovableFurniture>().myIndex;
            DataManager.Instance().AddObjectInfo(obj.GetHashCode(),_index, obj.transform.localPosition, obj.transform.localScale, obj.transform.localEulerAngles);
        }
        DataManager.Instance().OnSave();
    }
}
