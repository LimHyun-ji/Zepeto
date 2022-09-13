using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private float mouseX;
    private float mouseY;
    public float mouseSpeed=200;
    public float moveSpeed=10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1))//우클릭
        {
            LookCamera();
            MoveCamera();
        }
    }

    public void MoveCamera()
    {
        float h= Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float y = Input.GetAxisRaw("Height");

        Vector3 dir =(transform.right * h + transform.up * y + transform.forward* v).normalized;
        transform.position +=  dir * moveSpeed*Time.deltaTime;
    }
    public void LookCamera()
    {
        mouseX=transform.eulerAngles.y;
        mouseY=-transform.eulerAngles.x;
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        mouseX += mx *mouseSpeed *Time.deltaTime;
        mouseY += my * mouseSpeed * Time.deltaTime;

        transform.eulerAngles=new Vector3(-mouseY,mouseX, 0);
    }
}
