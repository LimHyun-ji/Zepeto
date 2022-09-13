using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance()
    {
        return _instance;
    }

    private void Awake() 
    {
        if(!_instance)
        {
            _instance=this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() 
    {
        DataManager.Instance().OnLoad();
    }

}
