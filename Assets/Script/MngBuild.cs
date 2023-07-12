using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
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
    private GameObject Right; //UI가 있는 영역
    private GameObject BtnLayer;
    private GameObject mouseObj;

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
        //시작시 켜질 탭
        BtnState = BtnSelected.Floor;
        BtnSwitch();

        //test
        getPrefabTexture(ListFloor);

    }

    private void initMng()
    {
        #region 오브젝트 할당
        mouseObj = new GameObject();
        //Debug.Log(StateBtnNow);
        GameObject cansvas = GameObject.Find("Canvas");
        Right = cansvas.transform.Find("Right").gameObject;

        //캔버스 -> right(1) -> btnlayer
        // 버튼레이어 받아오기
        BtnLayer = Right.transform.Find("BtnLayer").gameObject;
        //Debug.Log(BtnLayer.name);

        //버튼 레이어에어 버튼 꺼내기 및 버튼할당
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

    //나중에 게임시스템으로 변경
    public void UpdataBtnAction()
    {
        
    }

  
    /// <summary>
    /// 버튼 스위치
    /// </summary>
    /// <param name="_btn"> 바꿀 bool value</param>
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
                    //컬러변경
                    BtnColorChange(BtnFloor, onColor);
                    BtnColorChange(BtnWall, offColor);
                    BtnColorChange(BtnDeco, offColor);
                    
                    //image에 켜진 버튼에 맞게 프리팹 버튼을 그려줌
                    
                    break;
                }
            case BtnSelected.Wall:
                {
                    //컬러변경
                    BtnColorChange(BtnFloor, offColor);
                    BtnColorChange(BtnWall, onColor);
                    BtnColorChange(BtnDeco, offColor);


                    break;
                }
            case BtnSelected.Deco:
                {
                    //컬러변경
                    BtnColorChange(BtnFloor, offColor);
                    BtnColorChange(BtnWall, offColor);
                    BtnColorChange(BtnDeco, onColor);


                    break;
                }
        }
    }

    /// <summary>
    /// 버튼 색변경 함수
    /// </summary>
    /// <param name="_btn">적용될 버튼</param>
    private void BtnColorChange(Button _btn, Color _color)
    {
        _btn.image.color = _color;
    }

    /// <summary>
    /// 프리팹에서 이미지 뽑아오는 함수
    /// </summary>
    /// <param name="_List"></param>
    private void getPrefabTexture(List<GameObject> _List)
    {
        if(_List == null)
        {
            return;
        }

        Transform contents = Right.transform.Find("PrefabScrollView").GetChild(0).GetChild(0).transform;
        int count = _List.Count;
        for (int i = 0; i< count; i++)
        {
            Texture2D tmpTexture2D = AssetPreview.GetAssetPreview(_List[i]);
            if(tmpTexture2D != null)
            {
                Sprite mySprite = Sprite.Create(tmpTexture2D, new Rect(0.0f, 0.0f, tmpTexture2D.width, tmpTexture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
                
            }


            File.WriteAllText(Application.dataPath + "/PrefabImgFolder/" + i +".png", "PrefabImg" + i);
        }
    }

    //시작할때 쓰는기능 처음만 불러오고 나중에는 SetActive 처리하자
    //자동으로 addlistener를 넣어주는 기능을 만들어야해 (right의 image 밑에!)
    // 버튼 이미지,
    // 눌렀을때 마우스 끝에 있게 만드는기능
    //프리팹의 이미지를 가져오고 하드디스크에 이미지를 저장?
    private void createPrefabButton(List<GameObject> _List)
    {
        Transform contents = Right.transform.Find("PrefabScrollView").GetChild(0).GetChild(0).transform;
        int count = _List.Count;
        for (int i = 0; i < count; i++)
        {
           
            GameObject btnObj = new GameObject();
            btnObj.transform.parent = contents;
            btnObj.AddComponent<Button>();
            btnObj.AddComponent<Image>();
            Button btntmp = btnObj.GetComponent<Button>();
            btntmp.onClick.AddListener(() => {
                //클릭시 마우스위치에 프리팹을 소환해야해
                mouseObj = null;
                mouseObj = Instantiate(_List[i]);

            });
            Image Imgtmp = btnObj.GetComponent<Image>();
            //Imgtmp.sprite = mySprite;

            //btntmp.image.sprite = mySprite;

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
