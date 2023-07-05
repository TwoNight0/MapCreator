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

    BtnSelected BtnState;

    //GameObject
    private GameObject Right; //UI�� �ִ� ����
    private GameObject BtnLayer;

    //List<GameObject> prefab;
    [SerializeField]private List<GameObject> ListFloor;
    [SerializeField]private List<GameObject> ListWall;
    [SerializeField]private List<GameObject> ListDeco;

    //Button
    private Button BtnFloor;
    private Button BtnWall;
    private Button BtnDeco;

    //Color
    private Color onColor = new Color(0.5f, 0.5f, 1, 1);
    private Color offColor = Color.gray;

    private void Awake()
    {
        initMng();
    }

    // Start is called before the first frame update
    void Start()
    {
        //���۽� ���� ��
        BtnState = BtnSelected.Floor;
        BtnSwitch();

    }

    private void initMng()
    {
        #region ������Ʈ �Ҵ�
        //Debug.Log(StateBtnNow);
        GameObject cansvas = GameObject.Find("Canvas");
        Right = cansvas.transform.Find("Right").gameObject;

        //ĵ���� -> right(1) -> btnlayer
        // ��ư���̾� �޾ƿ���
        BtnLayer = Right.transform.Find("BtnLayer").gameObject;
        //Debug.Log(BtnLayer.name);

        //��ư ���̾�� ��ư ������ �� ��ư�Ҵ�
        GameObject btn = BtnLayer.transform.GetChild(0).gameObject;
        BtnFloor = btn.GetComponent<Button>();
        btn = BtnLayer.transform.GetChild(1).gameObject;
        BtnWall = btn.GetComponent<Button>();
        btn = BtnLayer.transform.GetChild(2).gameObject;
        BtnDeco = btn.GetComponent<Button>();
        btn = null;
        Destroy(btn);
        #endregion
        #region Btn AddListener
        BtnFloor.onClick.AddListener(() =>
        {
            BtnState = BtnSelected.Floor;
            BtnSwitch();
        });
        BtnWall.onClick.AddListener(() => {
            BtnState = BtnSelected.Wall;
            BtnSwitch();
        });

        BtnDeco.onClick.AddListener(() => {
            BtnState = BtnSelected.Deco;
            BtnSwitch();
        });
        #endregion
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
    public void BoolSwitch(ref bool _btnBool)
    {
        _btnBool = !_btnBool;
        Debug.Log(_btnBool);
    }

    private void BtnSwitch()
    {
        switch (BtnState)
        {
            case BtnSelected.None: Debug.Log("None"); break;
            case BtnSelected.Floor:
                {
                    //�÷�����
                    BtnColorChange(BtnFloor, onColor);
                    BtnColorChange(BtnWall, offColor);
                    BtnColorChange(BtnDeco, offColor);
                    
                    //image�� ���� ��ư�� �°� ������ ��ư�� �׷���
                    
                    break;
                }
            case BtnSelected.Wall:
                {
                    //�÷�����
                    BtnColorChange(BtnFloor, offColor);
                    BtnColorChange(BtnWall, onColor);
                    BtnColorChange(BtnDeco, offColor);


                    break;
                }
            case BtnSelected.Deco:
                {
                    //�÷�����
                    BtnColorChange(BtnFloor, offColor);
                    BtnColorChange(BtnWall, offColor);
                    BtnColorChange(BtnDeco, onColor);


                    break;
                }
        }
    }

    /// <summary>
    /// ��ư ������ �Լ�
    /// </summary>
    /// <param name="_btn">����� ��ư</param>
    private void BtnColorChange(Button _btn, Color _color)
    {
        _btn.image.color = _color;
    }

    //�������� �̹����� �������� �ϵ��ũ�� �̹����� ����
    private void getPrefabTexture()
    {

    }
    //�����Ҷ� ���±�� ó���� �ҷ����� ���߿��� SetActive ó������
    //�ڵ����� addlistener�� �־��ִ� ����� �������� (right�� image �ؿ�!)
    // ��ư �̹���,
    // �������� ���콺 ���� �ְ� ����±��
    private void makePrefabButton(List<GameObject> _prefabList)
    {
        //�ݺ����� �� �Ҵ�
        int count = _prefabList.Count;

        for (int i = 0; i < count; i++) { 
            //������Ʈ �����

            //��ư ������Ʈ �߰����ֱ� 

            //�̹��� �������ֱ�
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
