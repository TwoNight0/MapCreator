using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MngBuild : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ray();
    }


    private void ray()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log(hit.transform.name);
            Debug.Log("hit");
        }

    }

    /// <summary>
    /// 1. �����յ��� �̹����� ������ ���� ��ư�� �Ҵ�
    /// 2. ��ư�� ������ ���콺�����ǿ� ��ü�� ����ٴϰ���
    /// 3. ���ʹ�ư�� ������ �� ��ġ�� ��ü�� ����(������������ �ʷϻ�, �ƴϸ� ����)
    /// 4. �����ʹ�ư�� ������ ����� ������Ʈ�� ���ְ� �����չ�ư�� ������ �ٽ� ����
    /// </summary>
    private void objs()
    {

    }

    //���콺�� �ִ� ��ü ȸ��
    private void objRotate()
    {

    }

}
