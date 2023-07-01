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
    /// 1. 프리팹들의 이미지를 가져온 다음 버튼에 할당
    /// 2. 버튼을 누르면 마우스포지션에 물체가 따라다니게함
    /// 3. 왼쪽버튼을 누르면 그 위치에 물체를 놓음(놓을수있으면 초록색, 아니면 빨강)
    /// 4. 오른쪽버튼을 누르면 저장된 오브젝트를 없애고 프리팹버튼을 눌러야 다시 가능
    /// </summary>
    private void objs()
    {

    }

    //마우스에 있는 물체 회전
    private void objRotate()
    {

    }

}
