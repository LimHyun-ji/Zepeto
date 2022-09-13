using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Prefab에 붙여야 하는 스크립트
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class CheckCollision : MonoBehaviour
{
    public bool canBuild=true;
    public Material mat;
    private void Awake() 
    {
        GetComponent<BoxCollider>().isTrigger=true;
        GetComponent<Rigidbody>().useGravity=false;
        mat = GetComponentInChildren<MeshRenderer>().material;
    }

    
    private void OnTriggerExit(Collider other) 
    {
        if(1 << other.gameObject.layer != GridBuildingSystem.Instance().groundLayers)
        {
            //설치 가능
            canBuild=true;
            //파란색으로
            mat.SetColor("_Color", Color.blue);
        }
    }
    private void OnTriggerStay(Collider other) 
    {
        if(1 << other.gameObject.layer != GridBuildingSystem.Instance().groundLayers)
        {
            //설치 불가
            canBuild=false;
            //빨간색으로 
            mat.SetColor("_Color", Color.red);
        }
    }
}
