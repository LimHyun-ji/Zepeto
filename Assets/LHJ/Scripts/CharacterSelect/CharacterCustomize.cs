using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomize : MonoBehaviour
{
    public FlexibleColorPicker fcp;
    //Character
    public SkinnedMeshRenderer backHair;
    public SkinnedMeshRenderer frontHair;
    public SkinnedMeshRenderer clothes;
    public SkinnedMeshRenderer head;
    private Material matEyes;
    private Material matClothes;
    private Material[] matHeads;

    public List<Material> eyeLists;
    public List<Material> clothesCasualLists;

    void Start()
    {
        fcp.color = backHair.material.color;
        fcp.onColorChange.AddListener(OnChangeColor);
    }
    //눈 색깔 고르기
    //옷 색깔 고르기

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Eye change: "+ head.materials[4]);
        }
    }
    public void ChangeEyeMaterial(int index)
    {
        matHeads= new Material[head.materials.Length];
        for(int i = 0; i< head.materials.Length; i++)
        {
            matHeads[i] = head.materials[i];
        }
        matHeads[4] = eyeLists[index];
        head.materials=matHeads;
    }

    public void ChangeClothesMaterial(int index)
    {
        clothes.material = clothesCasualLists[index];
    }

    public void ChangeHairColor()
    {
        
    }
    //머리색 바꾸기
    private void OnChangeColor(Color co) 
    {
        backHair.material.color = co;
        frontHair.material.color=co;
    }


}
