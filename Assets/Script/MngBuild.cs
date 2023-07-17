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
        objRotate();
        objSetPosition();
    }

    private void FixedUpdate()
    {
        //objsMove();
        //testMove(0);
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
            //여기에 고질적인 문제가있음 처음 에셋 프리뷰실행하면 null이뜸;
            Texture2D tmpTexture2D = AssetPreview.GetAssetPreview(_List[i]);

            if (tmpTexture2D != null)
            {
                //버튼생성
                GameObject btnObj = new GameObject();
                //숫자를 이름으로함
                btnObj.name = i.ToString();
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
                    int index = System.Convert.ToInt32(btnObj.name);
                    createPrefabObj(btnObj.tag, index);
                });
                Image Imgtmp = btnObj.GetComponent<Image>();
                

                //스프라이트 생성 및 할당
                Sprite mySprite = Sprite.Create(tmpTexture2D, new Rect(0.0f, 0.0f, tmpTexture2D.width, tmpTexture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
                Imgtmp.sprite = mySprite;
            }
        }
    }
   

    /// <summary>
    /// 버튼을 누르면 0,0,0 위치에 생성 
    /// </summary>
    /// <param name="_tag"></param>
    private void createPrefabObj(string _tag, int _nameAsNum)
    {
        if(mouseObj.transform.childCount == 0)
        {
            if(_tag == "Wall")
            {
                //오브젝트를 생성하고 마우스오브젝트를 부모로 만듬 
                GameObject newObj = Instantiate(ListWall[_nameAsNum], mouseObj.transform);
            }
            else if(_tag == "Decoration")
            {
                GameObject newObj = Instantiate(ListDeco[_nameAsNum], mouseObj.transform);
            }
        }
        else
        {
            return;
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

    //마우스 오브젝트 이동
    private void objsMove()
    {
        //#0
        //Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(mousepos);
        
        if(mouseObj.transform.childCount > 0)
        {
            //#1
            Vector3 mousepos = Input.mousePosition;
            //Vector3 offset = mousepos - mouseObj.transform.position;
            //Vector3 movethispoint = mousepos + offset;
            Debug.Log("mousepos : "+ mousepos);
            //Debug.Log("movepos : "+ movethispoint);
            mouseObj.transform.position = new Vector3 (mousepos.x, 0, mousepos.y);
            //mouseObj.transform.position = mousepos;
        }
  
    }
    private void testMove(int _num)
    {
        switch (_num)
        {
            //screenToWorldPoint 사용. 문제점 조작감이 이상함
            case 0:
            { 
                //Vector3 movePoint = Input.mousePosition;
                Vector3 mousePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y));
                mousePoint.z = mousePoint.z + 280;
                Debug.Log(mousePoint);
                Vector3 moveVector = new Vector3(mousePoint.x, 0, -mousePoint.z);
                mouseObj.transform.position = moveVector;

            }
            break;
            //ray사용
            case 1:
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit)) {
                        //맞은 물체의 월드포지션을 가져옴
                        Debug.Log(hit.transform.position);
                    }
                    //Debug.Log(ray);
            }
            break;
        }

    }

    //마우스에 있는 물체 회전(오브젝트 아래의 자손의 rotation을 변경해야함
    private void objRotate()
    {
        float add = 0;
        if (mouseObj.transform.childCount > 0 && Input.GetKeyDown(KeyCode.R))
        {
            GameObject tmp = mouseObj.transform.GetChild(0).gameObject;
            add += 90;
            Vector3 tmpV = new Vector3(0, add, 0);
            tmp.transform.Rotate(tmpV);
        }
    }

    /// <summary>
    /// 마우스 왼쪽을 누르면
    /// 아래오브젝트의 tag에 따라 map의 sub를 만들고 그 아래에 물체를 두고 부모를 설정
    /// 즉 부모변경하는 메소드
    /// </summary>
    private void objSetPosition() { 
        //mouseobj의 자식이 있고 왼쪽 클릭했을시
        if(mouseObj.transform.childCount > 0 && Input.GetMouseButton(0))
        {
            GameObject tmp = mouseObj.transform.GetChild(0).gameObject;
            GameObject Map = GameObject.Find("Map");

            switch (tmp.layer)
            {
                //wall
                case 7:
                    {
                        GameObject Wall = Map.transform.GetChild(1).gameObject;
                        GameObject subWall = new GameObject();
                        subWall.transform.parent = Wall.transform;

                        tmp.transform.parent = subWall.transform;
                    }
                    break;
                //deco
                case 8: 
                    { 
                    
                    }
                    break;
                default: break;

            }
        }
    
    }

    //자식이 없을때 그 물체를 클릭하면 오브젝트를 mouseobj의 하위오브젝트로 만들어주는 함수
    private void objPutOn()
    {

    }

}
