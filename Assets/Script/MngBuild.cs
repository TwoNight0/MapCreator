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
    private Button BtnWall;
    private Button BtnDeco;

    private List<GameObject> ListBtn;

    //Color
    private Color onColor = new Color(0.5f, 0.5f, 1, 1);
    private Color offColor = Color.gray;



    //Transform
    GameObject contents;


    private void Awake()
    {
        initMng();
    }

    // Start is called before the first frame update
    void Start()
    {
        //이미지 뽑는 함수
        //getPrefabTexture(ListDeco, Deco);

        //버튼생성
        createPrefabButton(ListWall);
        createPrefabButton(ListDeco);

        //리스트에 버튼의 게임오브젝트 넣기
        btnListAdd();

        #region Btn AddListener
        BtnWall.onClick.AddListener(() => {
            BtnState = BtnSelected.Wall;
            BtnSwitch();
        });

        BtnDeco.onClick.AddListener(() => {
            BtnState = BtnSelected.Deco;
            BtnSwitch();
        });
        #endregion


        //시작시 켜질 탭
        BtnState = BtnSelected.Deco;
        BtnSwitch();



        //test


    }

    private void initMng()
    {
        #region 오브젝트 할당
        mouseObj = new GameObject();
        mouseObj.name = "mouseObj";
        //Debug.Log(StateBtnNow);
        GameObject cansvas = GameObject.Find("Canvas");
        Right = cansvas.transform.Find("Right").gameObject;

        //캔버스 -> right(1) -> btnlayer
        // 버튼레이어 받아오기
        BtnLayer = Right.transform.Find("BtnLayer").gameObject;
        //Debug.Log(BtnLayer.name);

        //버튼 레이어에어 버튼 꺼내기 및 버튼할당
        GameObject btn = BtnLayer.transform.GetChild(0).gameObject;
        BtnWall = btn.GetComponent<Button>();
        btn = BtnLayer.transform.GetChild(1).gameObject;
        BtnDeco = btn.GetComponent<Button>();
        btn = null;
        Destroy(btn);
        #endregion

        //Content
        contents = Right.transform.Find("PrefabScrollView").GetChild(0).GetChild(0).gameObject;

        //List
        ListBtn = new List<GameObject>();
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
            case BtnSelected.Wall:
                {
                    //컬러변경
                    BtnColorChange(BtnWall, onColor);
                    BtnColorChange(BtnDeco, offColor);

                    //클릭시 deco tag 의 오브젝트 비활성화
                    onAndOffObjByTag("Decoration");
                    break;
                }
            case BtnSelected.Deco:
                {
                    //컬러변경
                    BtnColorChange(BtnWall, offColor);
                    BtnColorChange(BtnDeco, onColor);

                    // 클릭시 wall tag의 오브젝트 비활성화
                    onAndOffObjByTag("Wall");
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
    /// create로 만든 모든 버튼들을 리스트에 담는 코드
    /// </summary>
    private void btnListAdd()
    {
        int tmpCount = contents.transform.childCount;

        for (int i = 0; i < tmpCount; i++)
        {
            GameObject tmpObj = contents.transform.GetChild(i).gameObject;
            if(tmpObj != null)
            {
                ListBtn.Add(tmpObj);
            }
        }
        
    }

    /// <summary>
    /// 특정위치 밑에 있는 오브젝트하위애들을 버튼에 맞게 꺼주는 함수
    /// </summary>
    private void onAndOffObjByTag(string _offTag)
    {
        int count = ListBtn.Count;

        if(count == 0)
        {
            return;
        }

        for(int i = 0; i<count; i++)
        {
            if(ListBtn[i].tag == _offTag)
            {
                //원하는 것과 태그가 같으면 꺼주기
                ListBtn[i].SetActive(false);
            }
            else
            {
                //원하는 것과 태그가 같지 않으면 켜주기
                ListBtn[i].SetActive(true);
            }

        }
    }



    /// <summary>
    /// 프리팹에서 이미지 뽑아오는 함수
    /// </summary>
    /// <param name="_List"></param>
    private void getPrefabTexture(List<GameObject> _List, string _FolderName)
    {
        if(_List == null)
        {
            return;
        }

        int count = _List.Count;
        for (int i = 0; i< count; i++)
        {
            Texture2D tmpTexture2D = AssetPreview.GetAssetPreview(_List[i]);

            if(tmpTexture2D != null)
            {
                byte[] texturePNGBytes = tmpTexture2D.EncodeToPNG();
                //Sprite mySprite = Sprite.Create(tmpTexture2D, new Rect(0.0f, 0.0f, tmpTexture2D.width, tmpTexture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
                File.WriteAllBytes(Application.dataPath + "/PrefabImgFolder/" + _FolderName + "/"+ + i + ".png", texturePNGBytes);
            }


        }
    }

    //시작할때 쓰는기능 처음만 불러오고 나중에는 SetActive 처리하자
    //자동으로 addlistener를 넣어주는 기능을 만들어야해 (right의 image 밑에!)
    // 버튼 이미지,
    // 눌렀을때 마우스 끝에 있게 만드는기능
    //프리팹의 이미지를 가져오고 하드디스크에 이미지를 저장?
    private void createPrefabButton(List<GameObject> _List)
    {
        if (_List == null)
        {
            return;
        }

        int count = _List.Count;
        for (int i = 0; i < count; i++)
        {
            Texture2D tmpTexture2D = AssetPreview.GetAssetPreview(_List[i]);

            if (tmpTexture2D != null)
            {
                //버튼생성
                GameObject btnObj = new GameObject();
                btnObj.name = _List[i].name;
                if(_List == ListWall)
                {
                    btnObj.tag = "Wall";
                }
                else if(_List == ListDeco){
                    btnObj.tag = "Decoration";
                }

                //contents를 부모로 만듬
                btnObj.transform.parent = contents.transform;
                btnObj.AddComponent<Button>();
                btnObj.AddComponent<Image>();
                Button btntmp = btnObj.GetComponent<Button>();
                btntmp.onClick.AddListener(() => {
                    //클릭시 마우스위치에 프리팹을 소환해야해
                    mouseObj = null;
                    mouseObj = Instantiate(_List[i]);
                    Debug.Log("z");
                });
                Image Imgtmp = btnObj.GetComponent<Image>();


                //스프라이트 생성 및 할당
                Sprite mySprite = Sprite.Create(tmpTexture2D, new Rect(0.0f, 0.0f, tmpTexture2D.width, tmpTexture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
                Imgtmp.sprite = mySprite;
            }
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
