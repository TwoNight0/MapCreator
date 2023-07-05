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
    private GameObject Right; //UI�� �ִ� ����
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
        //�ʱ� FloorTab�� Ű�� �������� ��Ӱ� �����
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

        //ĵ���� -> right(1) -> btnlayer
        // ��ư���̾� �޾ƿ���
        BtnLayer = Right.transform.Find("BtnLayer").gameObject;
        //Debug.Log(BtnLayer.name);

        //��ư ���̾�� ��ư ������
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

        //�ʱ� ��ư �� ����
        isWallTab = false;
        isDecoTab = false;



    }

    // Update is called once per frame
    void Update()
    {
        //UpdataBtnAction();
    }

    //���߿� ���ӽý������� ����
    public void UpdataBtnAction()
    {
        
    }

  
    /// <summary>
    /// ��ư ����ġ
    /// </summary>
    /// <param name="_btn"> �ٲ� bool value</param>
    public void BtnSwitch(ref bool _btnBool)
    {
        _btnBool = !_btnBool;
        Debug.Log(_btnBool);
       
    }

    /// <summary>
    /// ��ư ������ �Լ�
    /// </summary>
    /// <param name="_btn">����� ��ư</param>
    private void BtnColorChange(Button _btn, bool _btnbool)
    {
        if (_btnbool)//����
        {
            _btn.image.color = myBlue;
        }
        else //����
        {
            _btn.image.color = Color.gray;
        }
    }

    /// <summary>
    /// ������ ������Ʈ�� ������ �������� �Լ�
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
    /// 1. �����յ��� �̹����� ������ ���� ��ư�� �Ҵ�
    /// 2. ��ư�� ������ ���콺�����ǿ� ��ü�� ����ٴϰ���
    /// 3. ���ʹ�ư�� ������ �� ��ġ�� ��ü�� ����(������������ �ʷϻ�, �ƴϸ� ����)
    /// 4. �����ʹ�ư�� ������ ����� ������Ʈ�� ���ְ� �����չ�ư�� ������ �ٽ� ����
    /// </summary>
    private void objsMove()
    {

    }

    //���콺�� �ִ� ��ü ȸ��
    private void objRotate()
    {

    }

}
