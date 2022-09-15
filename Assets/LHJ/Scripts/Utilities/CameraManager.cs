using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    enum CameraMode
    {
        BuildMode,
        CharacterSelectMode
    }
    private CameraMode currentCamMode;
    //카메라가 바라볼 타겟
    public Transform target;
    private Transform camPos;
    private float mouseX;
    private float mouseY;
    


    //Input 변수들
    private float h, v, y, mx, my;
    public float mouseSpeed=200;
    public float moveSpeed=10;
    public float minDistance=0.5f;
    public float maxDistance =3f;
    public float maxRot;
    public float minRot;


    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name=="CharacterScene")
        {
            currentCamMode = CameraMode.CharacterSelectMode;
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void Start()
    {
        camPos=transform.parent;//CamPos가져오기
    }

    // Update is called once per frame
    void Update()
    {
        h= Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        y = Input.GetAxisRaw("Height");

        mx = Input.GetAxis("Mouse X");
        my = Input.GetAxis("Mouse Y");

        switch(currentCamMode)
        {
            case CameraMode.CharacterSelectMode:
                if(Input.GetMouseButton(1))
                {
                    RotateCamera(target);
                    MoveCamera();
                }
                break;
            case CameraMode.BuildMode:
                if(Input.GetMouseButton(1))//우클릭
                {
                    RotateCamera(transform);
                    MoveCamera();
                }
                break;
        }        
    }
    

    public void MoveCamera()
    {
        //float distance=Vector3.Distance(transform.position, camPos.position);
        float angleX=transform.eulerAngles.x;

        Vector3 dir;
        //타겟이 빌드 부지이면
        dir =(transform.right * h + transform.up * y + transform.forward* v).normalized;
        //타겟이 캐릭터이면
        if(target.gameObject.tag == "Player")
        {
            dir = Vector3.forward * v;
            //가까이 가면 x값이 0으로
            if(v>0)
            {
                angleX = Mathf.Lerp(angleX, 0, Time.deltaTime);
            }
            //멀리 가면 x값이 15으로
            else if(v<0)
            {
                angleX = Mathf.Lerp(angleX, 14, Time.deltaTime);
            }
            transform.eulerAngles = new Vector3(angleX, 0, 0);
            transform.position +=  dir * moveSpeed/10*Time.deltaTime;
        }
        else
        {
            transform.position +=  dir * moveSpeed*Time.deltaTime;
        }

        if(camPos)
        {
            float distance=Vector3.Distance(transform.position, camPos.position);
            if(distance< minDistance || distance > maxDistance) return;

            if(distance > maxDistance ) 
            {
                transform.localPosition = new Vector3(0, 0, -maxDistance);
            }
            if(distance < minDistance ) 
            {
                transform.localPosition = new Vector3(0, 0, -minDistance);
            }
        }
    }

    public void RotateCamera(Transform rotateTarget)
    {
        mouseX=rotateTarget.eulerAngles.y;
        mouseY=-rotateTarget.eulerAngles.x;
        
        mouseY += my * mouseSpeed * Time.deltaTime;
        
        if(rotateTarget == this.transform)//카메라
        {
            mouseX += mx *mouseSpeed *Time.deltaTime;
            rotateTarget.eulerAngles=new Vector3(-mouseY,mouseX, 0);
        }
        else if(rotateTarget == target)
        {
            mouseX -= mx *mouseSpeed *Time.deltaTime;
            rotateTarget.eulerAngles=new Vector3(0,mouseX, 0);
        }
    }

    //얼굴부근을 클릭하면 얼굴을 확대한다?
    public void RotateTarget()
    {

    }

}
