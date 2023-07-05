using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MngBuild : MonoBehaviour
{

    private enum BtnSelected
    {
        None,
        Floor,
        Wall,
        Deco
    }

    

    //GameObject
    private GameObject Right; //UI가 있는 영역
    private GameObject BtnLayer;
    
    //Button
    private Button FloorTab;
    private Button WallTab;
    private Button DecoTab;

    //int
    private int StateBtnNow;


    //bool
    private bool isFloorTab = false;
    private bool isWallTab = false;
    private bool isDecoTab = false;


    private Color myBlue = new Color(0.5f, 0.5f, 1, 1);

    private void Awake()
    {
        initMng();
    }

    // Start is called before the first frame update
    void Start()
    {
        //초기 FloorTab만 키고 나머지는 어둡게 만들기
        isFloorTab = true;
        isWallTab = false;
        isDecoTab = false;
        BtnColorChange(FloorTab, isFloorTab);
        BtnColorChange(WallTab, isWallTab);
        BtnColorChange(DecoTab, isDecoTab);
    }

    private void initMng()
    {
        StateBtnNow = (int)BtnSelected.None;
        //Debug.Log(StateBtnNow);
        GameObject cansvas = GameObject.Find("Canvas");
        Right = cansvas.transform.Find("Right").gameObject;

        //캔버스 -> right(1) -> btnlayer
        // 버튼레이어 받아오기
        BtnLayer = Right.transform.Find("BtnLayer").gameObject;
        //Debug.Log(BtnLayer.name);

        //버튼 레이어에어 버튼 꺼내기
        GameObject btn = BtnLayer.transform.GetChild(0).gameObject;

        FloorTab = btn.GetComponent<Button>();
        FloorTab.onClick.AddListener(() => 
        {
            BtnSwitch(ref isFloorTab);
            BtnColorChange(FloorTab, isFloorTab);
        });

        btn = BtnLayer.transform.GetChild(1).gameObject;
        WallTab = btn.GetComponent<Button>();
        WallTab.onClick.AddListener(() => { 
            BtnSwitch(ref isWallTab);
            BtnColorChange(WallTab, isWallTab);
        });

        btn = BtnLayer.transform.GetChild(2).gameObject;
        DecoTab = btn.GetComponent<Button>();
        DecoTab.onClick.AddListener(() => { 
            BtnSwitch(ref isDecoTab);
            BtnColorChange(DecoTab, isDecoTab);
        });
        btn = null;
        Destroy(btn);

        //초기 버튼 색 변경
        isWallTab = false;
        isDecoTab = false;



    }

    // Update is called once per frame
    void Update()
    {
        //UpdataBtnAction();
    }

    //나중에 게임시스템으로 변경
    public void UpdataBtnAction()
    {
        
    }

  
    /// <summary>
    /// 버튼 스위치
    /// </summary>
    /// <param name="_btn"> 바꿀 bool value</param>
    public void BtnSwitch(ref bool _btnBool)
    {
        _btnBool = !_btnBool;
        Debug.Log(_btnBool);
       
    }

    /// <summary>
    /// 버튼 색변경 함수
    /// </summary>
    /// <param name="_btn">적용될 버튼</param>
    private void BtnColorChange(Button _btn, bool _btnbool)
    {
        if (_btnbool)//선택
        {
            _btn.image.color = myBlue;
        }
        else //비선택
        {
            _btn.image.color = Color.gray;
        }
    }

    /// <summary>
    /// 누르면 오브젝트의 정보를 가져오는 함수
    /// </summary>
    private void rayFind()
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
    private void objsMove()
    {

    }

    //마우스에 있는 물체 회전
    private void objRotate()
    {

    }

}
