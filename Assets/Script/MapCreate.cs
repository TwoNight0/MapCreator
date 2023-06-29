using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using System.Text;


/// <summary>
/// 위치정보와 레이어,태그 정보를 가지고있어서 제이스데이터를 바탕으로 다른 프리팹으로도 같은형태의 맵을 만들수있음
/// </summary>
[System.Serializable]
public class MapData : MonoBehaviour
{
    //public float x,y,z;
    public Vector3 pos;

    public int LayerNum = 6;
    public string tagName = "";
    
    public void printData()
    {
        Debug.Log("Position : " + pos);
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

    //subObj 추적용 리스트
    private List<GameObject> mapFloor;
    private List<GameObject> mapWall;
    private List<GameObject> mapDeco;

    //제이슨 저장용 arr
    MapData[] arrFloor;
    MapData[] arrWall;
    MapData[] arrDeco;

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

    //제이슨저장용
    StringBuilder JsonSbFloor;
    StringBuilder JsonSbWall;
    StringBuilder JsonSbDeco;


    [Tooltip("floor리스트의 최댓값")] private int max_Floor;
    [Tooltip("wall리스트의 최댓값")] private int max_Wall;

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

        //리스트 할당 subObj 추적용
        mapFloor = new List<GameObject>();
        mapWall = new List<GameObject>();
        mapDeco = new List<GameObject>();

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

        //StringBuilder 초기화
        JsonSbFloor = new StringBuilder();
        JsonSbWall = new StringBuilder();
        JsonSbDeco = new StringBuilder();
    }

    /// <summary>
    /// 1. 저장시 subOOO 을 모두 찾고 그 밑의 MapData들을 모두 리스트에 저장
    /// 2. 저장된 리스트를 제이슨화
    /// 3. 파일로 저장
    /// </summary>
    public void BtnSaveMap()
    {
        //저장된 제이슨 초기화
        RemoveJson("MapFloor", JsonSbFloor);
        RemoveJson("MapWall", JsonSbWall);
        RemoveJson("MapDeco", JsonSbDeco);

        //리스트 초기화
        mapFloor.Clear();
        mapWall.Clear();
        mapDeco.Clear();

        //리스트추가 및 MapData 포지션 할당
        AddObjList(mapFloor, "Floor");
        AddObjList(mapWall, "Wall");
        AddObjList(mapDeco, "Deco");

        //저장
        SaveObj(mapFloor, arrFloor, "MapFloor", JsonSbFloor);
        SaveObj(mapWall, arrWall, "MapWall", JsonSbWall);
        SaveObj(mapDeco, arrDeco, "MapDeco", JsonSbDeco);
    }

    private void SaveObj(List<GameObject> _List, MapData[] _arr, string _JsonName, StringBuilder _SBJson)
    {
        if (_List.Count != 0)
        {
            //리스트의 크기를 비교.  리스트의 크기는 생성할때 추가 중임
            for (int i = 0; i < _List.Count; i++)
            {
                _arr = _List[i].GetComponentsInChildren<MapData>();
                //직렬화 및 문자열 합체 
                for (int k = 0; k < _arr.Length; k++)
                {
                    string tmp = JsonUtility.ToJson(_arr[k]);
                    _SBJson.Append(tmp);
                    tmp = null;
                }
            }
            //StringBuilder -> string 변환
            string tmep = _SBJson.ToString();
            Debug.Log(tmep);

            //제이슨 저장
            File.WriteAllText(Application.dataPath + "/MapJsonFolder/" + _JsonName + ".json", tmep);
        }
    }
    public void BtnRemoveJson()
    {
        //저장된 제이슨 초기화
        RemoveJson("MapFloor", JsonSbFloor);
        RemoveJson("MapWall", JsonSbWall);
        RemoveJson("MapDeco", JsonSbDeco);
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
    private void AddObjList(List<GameObject> _List, string _FWD)
    {
        //상위 노드 찾음 obj = Floor
        GameObject obj = GameObject.Find(_FWD);
        // obj = Floor
        // obj.child = subFloor
        // obj.child.child = floor <= 찾는오브젝트 
        // 이 오브젝트의 위치값을 
        // MapData.pos에 넣어야해

        //상위노드 아래의 개수에 따라 리스트에 넣음 
        if (obj.transform.childCount > 0)
        { // subObj가 있음? 있으면 아래로
            //subObj = subFloor
            int count = obj.transform.childCount; //floor0 ~의 개수
            for (int i = 0; i < count; i++)
            {
                _List.Add(obj.transform.GetChild(i).gameObject); //ex) Floor에 subFloor을 넣음
                GameObject subObj = obj.transform.GetChild(i).gameObject; //subFloor
                if (subObj.transform.childCount > 0)
                {
                    for (int k = 0; k < subObj.transform.childCount; k++)
                    {
                        GameObject smallObj = subObj.transform.GetChild(k).gameObject;
                        MapData data = smallObj.GetComponent<MapData>();
                        data.pos = smallObj.transform.position;
                    }
                }
            }
        }
    }

    public void BtnLoadMap()
    {
        LoadMapCreate(LoadMap("MapFloor"),"subFloor",floor);
        LoadMapCreate(LoadMap("MapWall"),"subWall",wall);
        LoadMapCreate(LoadMap("MapDeco"),"subDeco",deco);
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
    private List<MapData> LoadMap(string _JsonName)
    {
        //경로에 저장된게 없으면 새로운걸 만듬
        if ((File.ReadAllText(Application.dataPath + "/MapJsonFolder/" + _JsonName + ".json")) == null){
            List<MapData> newList = new List<MapData>();
            Debug.Log("값이 없어 빈리스트를 반환함");
            return newList;
        }

        //3개의 정보를 다해야함
        string str = File.ReadAllText(Application.dataPath + "/MapJsonFolder/" + _JsonName + ".json");
        //List<MapData> result = JsonConvert.DeserializeObject<List<MapData>>(str); //오류뜸
        List<MapData> result = JsonUtility.FromJson<List<MapData>>(str);
        Debug.Log("제대로 들어옴");
        return result;
    }

    private void LoadMapCreate(List<MapData> _loadResult, string _name, GameObject _parent) 
    {
        if(_loadResult == null) {
            return;
        }

        //subObj 생성 및 이름 할당
        GameObject subObj = new GameObject(_name);

        //부모 만들어주기
        subObj.transform.parent = _parent.transform;

        //리스트의 길이만큼 하나하나를 만들어야함
        for(int i = 0; i<_loadResult.Count; i++)
        {
            //생성 및 부모설정
            GameObject obj = Instantiate(objPrefabList_Floor[Random.Range(0, max_Floor)], subObj.transform);

            //pos 조정
            obj.transform.position = _loadResult[i].pos;

            //tag 태그
            obj.transform.tag = _loadResult[i].tag;

            //layer 레이어
            obj.layer = _loadResult[i].LayerNum;

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
        string tmp2 = InputRow.text;

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

        ////리스트에 추가
        mapFloor.Add(subFloor);

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

            //MapData 넣기
            obj.AddComponent<MapData>();

            //MapData 가져오기
            MapData data = obj.GetComponent<MapData>();

            //오브젝트구분, 이름할당
            obj.name = "floor" + objNum.ToString();
            objNum++;

            //태그 지정
            obj.tag = "Ground";
            data.tagName = "Ground";

            //레이어 지정
            obj.layer = 6;
            data.LayerNum = 6;
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

            //MapData 넣기
            obj.AddComponent<MapData>();

            //MapData 가져오기
            MapData data = obj.GetComponent<MapData>();

            //오브젝트구분 
            obj.name = "wall" + objNum.ToString();
            objNum++;

            //태그 지정
            obj.tag = "Wall";
            data.tagName = "Wall";

            //레이어 지정
            obj.layer = 7;
            data.LayerNum = 7;
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
}
