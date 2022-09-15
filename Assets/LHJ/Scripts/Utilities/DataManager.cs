using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class ObjectInfo
{
    public int objHashCode;
    public int objIndex;
    public Vector3 position;
    public Vector3 scale;
    public Vector3 angle;
}
[System.Serializable]
public class ArrayObjectInfo<T>
{
    public List<T> data;
}
public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager Instance()
    {
        return _instance;
    }
    public List<ObjectInfo> objInfoList=new List<ObjectInfo>();  

    private void Awake() 
    {
        if(_instance==null)
        {
            _instance=this;
        }
    }

    //Infolist에 정보 추가하기
    public void AddObjectInfo(int hashCode, int objIndex, Vector3 pos, Vector3 scale, Vector3 angle)
    {
        ObjectInfo objInfo= new ObjectInfo();

        objInfo.objHashCode=hashCode;
        objInfo.objIndex=objIndex;
        objInfo.position=pos;
        objInfo.scale=scale;
        objInfo.angle=angle;

        objInfoList.Add(objInfo);
    }

    //저장하기
    public void OnSave()
    {
        ArrayObjectInfo<ObjectInfo> arrayData= new ArrayObjectInfo<ObjectInfo>();
        arrayData.data = objInfoList;

        string jsonData= JsonUtility.ToJson(arrayData, true);
        print(jsonData);

        string path = Application.dataPath + "/LHJ/Data";
        if(Directory.Exists(path)==false)
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(path + "/data.txt", jsonData);
    }

    public void OnLoad()
    {
        string path = Application.dataPath + "/LHJ/Data/data.txt";
        string jsonData = File.ReadAllText(path);
        print(jsonData);

        ArrayObjectInfo<ObjectInfo> arrayData= JsonUtility.FromJson<ArrayObjectInfo<ObjectInfo>>(jsonData);

        //생성을 다시 해줘야 함
        for(int i = 0; i < arrayData.data.Count; i++)
        {
            ObjectInfo info = arrayData.data[i];
            objInfoList.Add(info);
            CreateObject(info);
        }
    }

    void CreateObject(ObjectInfo info)
    {
        GameObject obj = Instantiate(GridBuildingSystem.Instance().buildingObjects[info.objIndex]);
        obj.transform.position = info.position;
        obj.transform.localScale = info.scale;
        obj.transform.eulerAngles = info.angle;
        //Destroy(obj.GetComponent<MovableFurniture>());
        obj.GetComponent<MovableFurniture>().enabled=false;
        obj.GetComponent<MovableFurniture>().isPlaced=true;
        obj.GetComponent<MovableFurniture>().isInit=false;

    }
}
