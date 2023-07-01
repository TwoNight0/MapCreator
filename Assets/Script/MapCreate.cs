using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using System.Text; //stringBuilder용



/// <summary>
/// 위치정보와 레이어,태그 정보를 가지고있어서 제이스데이터를 바탕으로 다른 프리팹으로도 같은형태의 맵을 만들수있음
/// </summary>
[System.Serializable]
public class MapData
{
    public float x,y,z;
    public int LayerNum = 6;
    public string tagName = "";
    
    public void printData()
    {
        Debug.Log("TagName : " + tagName);
        Debug.Log("LayerNum : " + LayerNum);
    }
}

public class MapCreate : MonoBehaviour
{
    [Header("프리팹 리스트")] //프리팹리스트
    [SerializeField, Tooltip("바닥에 사용될 프리팹")] private List<GameObject> objPrefabList_Floor;
    [SerializeField, Tooltip("벽에 사용될 프리팹")] private List<GameObject> objPrefabList_Wall;
    [Space(20)]

    //최상위(root)
    private GameObject map;
    //상위 노드
    private GameObject floor;
    private GameObject wall;
    private GameObject deco;

    [Header("타일 사이즈")] //타일 사이즈
    [SerializeField, Tooltip("타일의 가로, 1이상이여야함")] int TileSizeRow;
    [SerializeField, Tooltip("타일의 세로, 1이상이여야함")] int TileSizecolumn;
    private int totalPixel;
    private int prefabSize; //프리팹하나의 크기 
    private int tempZ;

    //입력값
    public InputField InputRow;
    public InputField InputColumn;

    //오브젝트 고유번호
    private int objNum;

    //벽을 안보이게 할때 사용
    bool walloff;
    bool Decooff;

    [Tooltip("floor리스트의 최댓값")] private int max_Floor;
    [Tooltip("wall리스트의 최댓값")] private int max_Wall;

    List<MapData> ListLoad;

    private void Awake()
    {
        initData();
    }

    void Start()
    {
       
        
    }

    void Update()
    {

    }

    //초기화
    private void initData() 
    {
        //최상위 오브젝트
        map = new GameObject();
        //상위 오브젝트들(부모)
        floor = new GameObject(); //바닥의부모
        wall = new GameObject(); //벽들의 부모
        deco = new GameObject(); //장식품들의 부모

        //이름할당
        map.name = "Map";
        floor.name = "Floor";
        wall.name = "Wall";
        deco.name = "Deco";

        //위치 초기화
        map.transform.position = Vector3.zero;
        floor.transform.position = Vector3.zero;
        wall.transform.position = Vector3.zero;
        deco.transform.position = Vector3.zero;

        //map을 최상위 부모로 만듬
        floor.transform.parent = map.transform;
        wall.transform.parent = map.transform;
        deco.transform.parent = map.transform;

        //프리팹 수의 최대값(랜덤하게 프리팹을 쓰기위해 사용함)
        max_Floor = objPrefabList_Floor.Count;
        max_Wall = objPrefabList_Wall.Count;

        //타일 초기화
        TileSizeRow = 1;
        TileSizecolumn = 1;

        //bool 초기화
        walloff = true;

        ListLoad = new List<MapData>();

    }

    /// <summary>
    /// 1. 저장시 subOOO 을 모두 찾고 그 밑의 MapData들을 모두 리스트에 저장
    /// 2. 저장된 리스트를 제이슨화
    /// 3. 파일로 저장
    /// </summary>
    public void BtnSaveMap()
    {
        SaveObj(floor);
        SaveObj(wall);
        SaveObj(deco);
    }

    /// <summary>
    /// 저장할때 오브젝트들을 가져오고 그 오브젝트의 정보들을 담을 mapdata 데이터를 만들고 
    /// 담고 리스트에 넣고 그 리스트를 저장!
    /// </summary>
    /// <param name="_List"></param>
    /// <param name="_arr"></param>
    /// <param name="_JsonName"></param>
    /// <param name="_SBJson"></param>

    private void SaveObj(GameObject _nodeName)
    {
        // _nodeName == floor임
        // sub의 개수 
        int subCount = _nodeName.transform.childCount;
        //subObj들이 담겨있는 리스트

        if (subCount > 0) //sub의개수가 1개이상일때 작동
        {
            //floor 들의 정보를 담을 리스트 추후 제이슨으로 변환예정
            List<MapData> tempList = new List<MapData>();

            for (int i = 0; i < subCount; i++)
            {
                //subobj들 가져옴
                Transform subTransform = _nodeName.transform.GetChild(i); 
                GameObject subObject = subTransform.gameObject;

                int objectCount = subObject.transform.childCount;

                if(objectCount > 0)
                {
                    for (int k = 0; k < objectCount; k++)
                    {
                        Transform objTransform = subObject.transform.GetChild(k);
                        GameObject Object = objTransform.gameObject;

                        //담을 데이터 생성
                        MapData data = new MapData();

                        data.x = Object.transform.position.x;
                        data.y = Object.transform.position.y;
                        data.z = Object.transform.position.z;

                        data.tagName = Object.transform.tag;
                        data.LayerNum = Object.layer;

                        //제이슨 변환용 리스트에 담기
                        tempList.Add(data);

                    }
                }
            }

            //제이슨 저장
            string str = JsonConvert.SerializeObject(tempList);
            //파일 저장
            File.WriteAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json", str);
        }
    }




    public void BtnRemoveJson()
    {
        //저장된 제이슨 초기화
  
    }

    /// <summary>
    /// 제이슨 초기화
    /// </summary>
    /// <param name="_JsonName"></param>
    /// <param name="_SBJson"></param>
    private void RemoveJson(string _JsonName, StringBuilder _SBJson)
    {
        _SBJson.Clear();
        string tmep = _SBJson.ToString();

        //제이슨 저장
        File.WriteAllText(Application.dataPath + "/MapJsonFolder/" + _JsonName + ".json", tmep);
    }

    /// <summary>
    /// 리스트에 추가 및 MapData 포지션 할당
    /// </summary>
    /// <param name="_List"> mapFloor,mapWall,mapDeco</param>
    /// <param name="_FWD">Floor, Wall, Deco</param>
 

    public void BtnLoadMap()
    {
        LoadMap(floor, "subFloor");
        LoadMap(wall, "subWall");
        LoadMap(deco, "subDeco");
    }

    /// <summary>
    /// 해당경로에 해당하는 리스트를 반환함(없으면 빈리스트를 반환)
    /// 0. 스타트시 일단 한번 실행
    /// 1. 제이슨데이터 파일를 가져와 역직렬화
    /// 2. 리스트로 다시 가져오고 
    /// 3. 리스트의 길이만큼 반복문을 돌면서 위치에 맞춰 오브젝트를 생성
    /// 4. 태그와 레이어 조정
    /// </summary>
    /// <returns></returns>
    private void LoadMap(GameObject _nodeName, string _name)
    {
        //3개의 정보를 다해야함
        string str = File.ReadAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json");
        

        ListLoad = JsonConvert.DeserializeObject<List<MapData>>(str);


        LoadMapCreate(_nodeName, _name);
    }

    /// <summary>
    /// 리스트를 바탕으로 오브젝트를 생성
    /// </summary>
    private void LoadMapCreate(GameObject _nodeName, string _name) 
    {
        if(ListLoad.Count > 0)
        {
            //subObj 생성 및 이름 할당
            GameObject subObj = new GameObject(_name);

            //부모 만들어주기
            subObj.transform.parent = _nodeName.transform;
            if (ListLoad != null && ListLoad.Count > 0)
            {
                //리스트의 길이만큼 하나하나를 만들어야함
                for (int i = 0; i < ListLoad.Count; i++)
                {
                    //생성 및 부모설정
                    GameObject obj = Instantiate(objPrefabList_Floor[Random.Range(0, max_Floor)], subObj.transform);

                    //pos 조정
                    obj.transform.position = new Vector3(ListLoad[i].x, ListLoad[i].y, ListLoad[i].z);

                    //tag 태그
                    obj.transform.tag = ListLoad[i].tagName;

                    //layer 레이어
                    obj.layer = ListLoad[i].LayerNum;

                }
            }
        
        }
        
    }

    #region  create
    /// <summary>
    /// 입력한 가로 x 세로 만큼 타일을 만들어주는 메서드
    /// </summary>
    public void BtnCreateFloorTile()
    {
        //InputField값 가져오기
        string tmp = InputRow.text;
        string tmp2 = InputColumn.text;

        //임시 int
        int resultIntX, resultIntZ = 0;

        int.TryParse(tmp, out resultIntX);
        int.TryParse(tmp2, out resultIntZ);

        //값확인
        if(resultIntX > 0 && resultIntZ > 0)
        {
            TileSizeRow = resultIntX;
            TileSizecolumn = resultIntZ;
        }

        //프리팹이 차지하는 크기만큼 포지션을 옮겨야하기때문에 프리팹 사이즈를 측정하기위함
        MeshRenderer ren = objPrefabList_Floor[0].GetComponent<MeshRenderer>();
        prefabSize = (int)ren.bounds.size.x;

        //for문하나로 z값을 늘려주기위함
        tempZ = 0;

        //총깔아야하는 프리팹 수
        totalPixel = TileSizeRow * TileSizecolumn;

        //Floor -> 상위 노드(this) -> 111
        GameObject subFloor = new GameObject();
        subFloor.transform.parent = floor.transform;
        subFloor.name = "subFloor";

        for (int i = 0; i < totalPixel; i++)
        {
            //z축으로 늘리기위함
            if (i % TileSizecolumn == 0)
            {
                tempZ += prefabSize;
            }

            //position 설정
            Vector3 Vfloor = new Vector3(i * prefabSize - ((tempZ - prefabSize) * TileSizecolumn), 0, tempZ - prefabSize);
            //Debug.Log(Vfloor);

            //오브젝트생성
            GameObject obj = Instantiate(objPrefabList_Floor[Random.Range(0, max_Floor)], subFloor.transform);

            //위치지정
            obj.transform.position = Vfloor;

            //오브젝트구분, 이름할당
            obj.name = "floor" + objNum.ToString();
            objNum++;

            //태그 지정
            obj.tag = "Ground";

            //레이어 지정
            obj.layer = 6;
        }
        //콜라이더 만들기
        subFloor.AddComponent<BoxCollider>();

        //콜라이더 가져오기
        BoxCollider Cbox = subFloor.GetComponent<BoxCollider>();

        //콜라이더 사이즈 조절
        Cbox.size = new Vector3(TileSizecolumn * prefabSize, 0, TileSizeRow * prefabSize);

        //콜라이더 위치 조절
        //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
        Cbox.center = new Vector3(((TileSizecolumn * prefabSize / 2) - (prefabSize / 2)),
            0,
            ((TileSizeRow * prefabSize / 2) - (prefabSize / 2)));

        //다시 사용하기 위한 초기화
        TileSizeRow = 1;
        TileSizecolumn = 1;
    }

    /// <summary>
    /// 입력한 가로에따른 타일을 만들어주는 메서드
    /// </summary>
    public void BtnCreateWallTile()
    {
        //프리팹이 차지하는 크기만큼 포지션을 옮겨야하기때문에 프리팹 사이즈를 측정하기위함
        MeshRenderer ren = objPrefabList_Wall[0].GetComponent<MeshRenderer>();
        prefabSize = (int)ren.bounds.size.x;
        float sizeY = ren.bounds.size.y;
        float sizeZ = ren.bounds.size.z;

        //InputField값 가져오기
        string tmp = InputRow.text;

        //임시 int
        int resultIntX = 0;

        int.TryParse(tmp, out resultIntX);

        //값확인
        if (resultIntX > 0)
        {
            TileSizeRow = resultIntX;
        }
        //Floor -> 상위 노드(this) -> 111
        GameObject subWall = new GameObject();

        // subWall의 부모노드 설정
        subWall.transform.parent = wall.transform;
        subWall.name = "subWall";

        for (int i = 0; i < TileSizeRow; i++)
        {
            //position 설정
            Vector3 Vwall = new Vector3(i * prefabSize, 0, 0);
            //Debug.Log(Vwall);

            //오브젝트생성
            GameObject obj = Instantiate(objPrefabList_Wall[Random.Range(0, max_Wall)], subWall.transform);

            //obj 위치지정
            obj.transform.position = Vwall;

            //오브젝트구분 
            obj.name = "wall" + objNum.ToString();
            objNum++;

            //태그 지정
            obj.tag = "Wall";

            //레이어 지정
            obj.layer = 7;
        }
        //콜라이더 만들기
        subWall.AddComponent<BoxCollider>();

        //콜라이더 가져오기
        BoxCollider Cbox = subWall.GetComponent<BoxCollider>();

        //콜라이더 사이즈 조절
        Cbox.size = new Vector3(TileSizeRow * prefabSize, sizeY, sizeZ);

        //콜라이더 위치 조절
        //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
        Cbox.center = new Vector3(((TileSizeRow * prefabSize / 2) - (prefabSize / 2)),
             sizeY / (prefabSize / 2),
             -(prefabSize / 2) - (sizeZ / 2));

        //((TileSizeX * prefabSize / 2) - (prefabSize / 2))
        //다시 사용하기 위한 초기화
        TileSizeRow = 1;
    }
    #endregion

    public void wallinvisible()
    {
        walloff = !walloff;
        wall.SetActive(walloff);
        
        //wall 아래 sub 아래 wall의 render들을 가져온다음 그것들의 알파도를 건들여야함
        
        if(wall.transform.childCount > 0) //서브가있는지 확인해야해
        {
            int wall;


        }


    }

}
