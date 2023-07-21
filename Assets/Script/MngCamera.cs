using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MngCamera : MonoBehaviour
{
    [SerializeField]private Camera maincam;
    private float camPosY;
    private float camPosX;
    private float camPosZ;



    // Start is called before the first frame update
    void Start()
    {
        camPosY = 50;
        
    }

    // Update is called once per frame
    void Update()
    {
        InputCamerMove();
    }

    private void InputCamerMove(){
   
        //���� ������
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)//���콺 ���� �������ϸ�����
        {
            camPosY -= 10;
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            camPosY += 10;
        }
         
        //�¿� ��
        camPosX += Input.GetAxisRaw("Horizontal") * Time.deltaTime * 10;
        camPosZ += Input.GetAxisRaw("Vertical") * Time.deltaTime * 10;

        //ī�޶� ��ġ�Ҵ�
        maincam.transform.position = new Vector3 (camPosX, camPosY, camPosZ);

    }

}
