using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using System.Text; //stringBuilder용
using UnityEditor;

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


    List<string> LoadLOLstr;



    private void Awake()
    {
        initData();

    }

    void Start()
    {
        //test();


    }

    void Update()
    {

    }

    

    private void test()
    {
        List<MapData> testList = new List<MapData>();
        List<MapData> testList2 = new List<MapData>();
        MapData data = new MapData();
        MapData data2 = new MapData();

        List<string> JsonStrings = new List<string>();

        data.x = 1;
        data.y = 1;
        data.z = 1;

        testList.Add(data);

        data2.x = 2;
        data2.y = 2;
        data2.z = 2;

        testList2.Add(data2);

        string str1 = JsonConvert.SerializeObject(testList);
        string str2 = JsonConvert.SerializeObject(testList2);

        Debug.Log("맵데이터 리스트 1 : " + str1);
        Debug.Log("맵데이터 리스트 2 : " + str2);

        JsonStrings.Add(str1);
        JsonStrings.Add(str2);

        string js = JsonConvert.SerializeObject(JsonStrings);

        List<string> deserailList = JsonConvert.DeserializeObject<List<string>>(js);

        Debug.Log("제이슨스트링리스트 : " + JsonStrings);
        Debug.Log("제이슨스트링리스트 : " + JsonStrings[0]);

        //tempList 사용
        List<MapData> DeSerialList = JsonConvert.DeserializeObject<List < MapData >> (deserailList[0]);

        Debug.Log("DSL: "+ DeSerialList);
        Debug.Log("DSLx: "+ DeSerialList[0].x);
        Debug.Log("DSLy: "+ DeSerialList[0].y);
        Debug.Log("DSLz: "+ DeSerialList[0].z);

        //AssetPreview.GetAssetPreview();
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

        //타일 초기화
        TileSizeRow = 1;
        TileSizecolumn = 1;

        LoadLOLstr = new List<string>();


    }

    /// <summary>
    /// 1. 저장시 subOOO 을 모두 찾고 그 밑의 MapData들을 모두 리스트에 저장
    /// 2. 저장된 리스트를 제이슨화
    /// 3. 파일로 저장
    /// </summary>
    public void BtnSaveMap()
    {
        SaveObj(floor, LoadLOLstr);
        SaveObj(wall, LoadLOLstr);
        SaveObj(deco, LoadLOLstr);
    }

    #region Save
    /// <summary>
    /// 사용x 예전 세이브 버전
    /// 저장할때 오브젝트들을 가져오고 그 오브젝트의 정보들을 담을 mapdata 데이터를 만들고 
    /// 담고 리스트에 넣고 그 리스트를 저장!
    /// 문제점 : 서브오브젝트 하나에 모든 오브젝트를 다 때려넣어서 부분별로 움직이기가 쉽지 않음
    /// </summary>
    /// <param name="_nodeName">큰 노드</param>
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

    //overloading
    /// <summary>
    /// subObject를 구분해서 만들어 주고 그 밑에 데이터를 넣음
    /// </summary>
    /// <param name="_nodeName"></param>
    private void SaveObj(GameObject _nodeName, List<string> _ListOfListStr)
    {
        // _nodeName == floor임
        // sub의 개수 
        int subCount = _nodeName.transform.childCount;
        //subObj들이 담겨있는 리스트

        if (subCount > 0) //sub의개수가 1개이상일때 작동
        {
            //floor 들의 정보를 담을 리스트 추후 제이슨으로 변환예정

            for (int i = 0; i < subCount; i++)
            {
                //MapData담을 임시그릇 제이슨화 해야함
                List<MapData> tempList = new List<MapData>(); 

                //subobj들 가져옴
                Transform subTransform = _nodeName.transform.GetChild(i);
                GameObject subObject = subTransform.gameObject;

                int objectCount = subObject.transform.childCount;

                //floor 들의 갯수
                if (objectCount > 0)
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
                string tmp = JsonConvert.SerializeObject(tempList);

                //제이슨 문자열이 담긴 string tmp를 리스트에 저장함
                _ListOfListStr.Add(tmp);
            }

            //제이슨 문자열이 담긴 List str을 이중 제이슨 저장
            string str = JsonConvert.SerializeObject(_ListOfListStr);
            //파일 저장
            File.WriteAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json", str);
        }
        else //아무것도없을때 저장하면 제이슨을 초기화함
        {
            string str = null;
            File.WriteAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json", str);
        }
    }
    #endregion

    /// <summary>
    /// 리스트에 추가 및 MapData 포지션 할당
    /// </summary>
    /// <param name="_List"> mapFloor,mapWall,mapDeco</param>
    /// <param name="_FWD">Floor, Wall, Deco</param>

    public void BtnLoadMap()
    {
        LoadMapRandom(floor, "subFloor", objPrefabList_Floor);
        LoadMapRandom(wall, "subWall", objPrefabList_Wall);
        //LoadMap(deco, "subDeco");
    }

    #region Load
    /// <summary>
    /// 해당경로에 해당하는 리스트를 반환함(없으면 빈리스트를 반환)
    /// 0. 스타트시 일단 한번 실행
    /// 1. 제이슨데이터 파일를 가져와 역직렬화
    /// 2. 리스트로 다시 가져오고 
    /// 3. 리스트의 길이만큼 반복문을 돌면서 위치에 맞춰 오브젝트를 생성
    /// 4. 태그와 레이어 조정
    /// </summary>
    /// <returns></returns>
    private void LoadMapRandom(GameObject _nodeName, string _name, List<GameObject> _PrefabList)
    { 
        //3개의 정보를 다해야함
        string str = File.ReadAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json");

        LoadLOLstr = JsonConvert.DeserializeObject<List<string>>(str);
        //LoadLOL = JsonUtility.FromJson<List<List<MapData>>>(str);


        LoadMapCreateRandom(_nodeName, _name, LoadLOLstr, _PrefabList);
    }

    private void LoadMap(GameObject _nodeName, string _name, List<GameObject> _PrefabList)
    {
        //3개의 정보를 다해야함
        string str = File.ReadAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json");

        LoadLOLstr = JsonConvert.DeserializeObject<List<string>>(str);
        //LoadLOL = JsonUtility.FromJson<List<List<MapData>>>(str);


        LoadMapCreate(_nodeName, _name, LoadLOLstr, _PrefabList);
    }

    /// <summary>
    /// 리스트를 바탕으로 오브젝트를 생성
    /// </summary>
    private void LoadMapCreateRandom(GameObject _nodeName, string _name, List<string> _LoadLOLstr, List<GameObject> _PrefabList) 
    {
        //LOL의 크기가 1개이상일때 동작
        if (_LoadLOLstr != null && _LoadLOLstr.Count > 0)
        {
            //리스트의 길이만큼 하나하나를 만들어야함
            for (int i = 0; i < _LoadLOLstr.Count; i++)//subObj의 개수만큼임 하고있음
            {
                //subObj 생성 및 이름 할당
                GameObject subObj = new GameObject(_name);

                //부모 만들어주기 floor를 subObj의 부모로 
                subObj.transform.parent = _nodeName.transform;
                //데이터 부분 (제이슨을 다시 풀어줌)
                List<MapData> tmp = JsonConvert.DeserializeObject<List<MapData>>(_LoadLOLstr[i]);

                for (int k = 0; k < tmp.Count; k++)//sub의 자식개수 만큼 반복문 도는 중
                {
                    //오브젝트 생성 및 부모!설정
                    GameObject obj = Instantiate(_PrefabList[Random.Range(0, _PrefabList.Count)], subObj.transform);

                    //pos 조정
                    obj.transform.position = new Vector3(tmp[k].x, tmp[k].y, tmp[k].z);

                    //tag 태그
                    obj.transform.tag = tmp[k].tagName;

                    //layer 레이어
                    obj.layer = tmp[k].LayerNum;
                }
                //콜라이더 만들기
                subObj.AddComponent<BoxCollider>();

                //콜라이더 가져오기
                BoxCollider Cbox = subObj.GetComponent<BoxCollider>();

                //콜라이더 사이즈 조절
                Cbox.size = new Vector3(TileSizecolumn * prefabSize, 0, TileSizeRow * prefabSize);

                //콜라이더 위치 조절
                //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
                Cbox.center = new Vector3(((TileSizecolumn * prefabSize / 2) - (prefabSize / 2)),
                    0,
                    ((TileSizeRow * prefabSize / 2) - (prefabSize / 2)));
            }
        }  
        else
        {
            Debug.Log(_name + ": " + "불러올 내용이 없습니다");
        }
      
    }

    private void LoadMapCreate(GameObject _nodeName, string _name, List<string> _LoadLOLstr, List<GameObject> _PrefabList)
    {
        //LOL의 크기가 1개이상일때 동작
        if (_LoadLOLstr != null && _LoadLOLstr.Count > 0)
        {
            //리스트의 길이만큼 하나하나를 만들어야함
            for (int i = 0; i < _LoadLOLstr.Count; i++)//subObj의 개수만큼임 하고있음
            {
                //subObj 생성 및 이름 할당
                GameObject subObj = new GameObject(_name);

                //부모 만들어주기 floor를 subObj의 부모로 
                subObj.transform.parent = _nodeName.transform;
                //데이터 부분 (제이슨을 다시 풀어줌)
                List<MapData> tmp = JsonConvert.DeserializeObject<List<MapData>>(_LoadLOLstr[i]);

                for (int k = 0; k < tmp.Count; k++)//sub의 자식개수 만큼 반복문 도는 중
                {
                    //오브젝트 생성 및 부모!설정 이부분 바꿔야함
                    GameObject obj = Instantiate(_PrefabList[Random.Range(0, _PrefabList.Count)], subObj.transform);

                    //pos 조정
                    obj.transform.position = new Vector3(tmp[k].x, tmp[k].y, tmp[k].z);

                    //tag 태그
                    obj.transform.tag = tmp[k].tagName;

                    //layer 레이어
                    obj.layer = tmp[k].LayerNum;
                }
                //콜라이더 만들기
                subObj.AddComponent<BoxCollider>();

                //콜라이더 가져오기
                BoxCollider Cbox = subObj.GetComponent<BoxCollider>();

                //콜라이더 사이즈 조절
                Cbox.size = new Vector3(TileSizecolumn * prefabSize, 0, TileSizeRow * prefabSize);

                //콜라이더 위치 조절
                //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
                Cbox.center = new Vector3(((TileSizecolumn * prefabSize / 2) - (prefabSize / 2)),
                    0,
                    ((TileSizeRow * prefabSize / 2) - (prefabSize / 2)));
            }
            

        }
        else
        {
            Debug.Log(_name + ": " + "불러올 내용이 없습니다");
        }

    }
    #endregion

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
            GameObject obj = Instantiate(objPrefabList_Floor[Random.Range(0, objPrefabList_Floor.Count)], subFloor.transform);

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
            GameObject obj = Instantiate(objPrefabList_Wall[Random.Range(0, objPrefabList_Wall.Count)], subWall.transform);

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
        Cbox.size = new Vector3(TileSizeRow * prefabSize, sizeY * 2.3f, sizeZ);

        //콜라이더 위치 조절
        //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
        Cbox.center = new Vector3(((TileSizeRow * prefabSize/2) - (prefabSize / 2)),
             (sizeY / prefabSize) +2,
             -(prefabSize / 2) - (sizeZ / 2));

        //((TileSizeX * prefabSize / 2) - (prefabSize / 2))
        //다시 사용하기 위한 초기화
        TileSizeRow = 1;
    }
    #endregion


}
