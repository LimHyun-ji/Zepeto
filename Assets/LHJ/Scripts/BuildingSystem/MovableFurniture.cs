using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Prefab에 붙여야 하는 스크립트
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class MovableFurniture : MonoBehaviour
{
    public bool canBuild=true;
    [HideInInspector] public bool isPlaced;
    [HideInInspector] public Material[] materials;
    [HideInInspector] public int myIndex;
    private void Awake() 
    {
        GetComponent<BoxCollider>().isTrigger=true;
        GetComponent<Rigidbody>().useGravity=false;
        
        MeshRenderer[] meshRenderer = GetComponentsInChildren<MeshRenderer>();
        materials = new Material[meshRenderer.Length];
        for(int i=0; i<meshRenderer.Length; i++)
        {
            materials[i] = meshRenderer[i].material;
        }
    }
    void Start()
    {

    }

    
    private void OnTriggerExit(Collider other) 
    {
        if(1 << other.gameObject.layer != GridBuildingSystem.Instance().groundLayers)
        {
            //설치 가능
            canBuild=true;
            //파란색으로
            ChangeMatColor(materials, "_BaseColor", Color.blue);
        }
    }
    private void OnTriggerStay(Collider other) 
    {
        if(1 << other.gameObject.layer != GridBuildingSystem.Instance().groundLayers)
        {
            //설치 불가
            canBuild=false;
            //빨간색으로 
            //mat.SetColor("_BaseColor", Color.red);
            ChangeMatColor(materials, "_BaseColor", Color.red);
        }
    }

    public void ChangeMatColor(Material[] mats,string colorName, Color color)
    {
        for(int i=0 ; i< mats.Length; i++)
        {
            mats[i].SetColor(colorName, color);
        }
    }
}
