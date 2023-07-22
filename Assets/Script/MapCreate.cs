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
    //subPosition
    public float x,y,z;

    public float rotation;

    // name(0) , 싱글에서 사용
    public string prefabName = "";

    //멀티에서 사용
    // 가로
    public int row;

    // 세로
    public int col;

}

public class MapCreate : MonoBehaviour
{

    [Header("프리팹 리스트")] //프리팹리스트
    [SerializeField, Tooltip("바닥에 사용될 프리팹")] private List<GameObject> objPrefabList_Floor;
    [SerializeField, Tooltip("벽에 사용될 프리팹")] private List<GameObject> objPrefabList_Wall;
    [SerializeField, Tooltip("데코에 사용될 프리팹")] private List<GameObject> objPrefabList_Deco; 
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

    private void Awake()
    {
        initData();
    }

    #region testing
    private void test()
    {
        //제이슨 저장 실험
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

    private void testInstance()
    {
        int findIndex = FindObjectInList(objPrefabList_Deco, "01_DiningHall_01_WoodenChear#1");
        GameObject obj = Instantiate(objPrefabList_Deco[findIndex]);
        obj.name = "잘됬나";
    }

    #endregion

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

    }
    
    public void BtnSaveMap()
    {
        SaveObj(floor);
        SaveObj(wall);
        SaveObj(deco);
    }

    #region Save
    /// <summary>
    /// 1. 저장시 subOOO 을 모두 찾고 그 밑의 MapData들을 모두 리스트에 저장
    /// 2. 저장된 리스트를 제이슨화
    /// 3. 파일로 저장
    /// </summary>
    private void SaveObj(GameObject _nodeName)
    {
        // _nodeName == floor임
        // sub의 개수 
        int subCount = _nodeName.transform.childCount;
        //subObj들이 담겨있는 리스트

        if (subCount > 0) //sub의개수가 1개이상일때 작동
        {
            //MapData담을 임시그릇 제이슨화 해야함
            List<MapData> tempList = new List<MapData>();

            //floor 들의 정보를 담을 리스트 추후 제이슨으로 변환예정
            for (int i = 0; i < subCount; i++)
            {
                //subobj들 가져옴
                GameObject subObject = _nodeName.transform.GetChild(i).gameObject;

                //저장 형식 생성
                MapData data = new MapData();

                //sub 포지션 저장
                data.x = subObject.transform.position.x;
                data.y = subObject.transform.position.y;
                data.z = subObject.transform.position.z;

                data.rotation = subObject.transform.eulerAngles.y;

                //가로 세로 저장(콜라이더 크기 사용
                //콜라이더 가져오기
                BoxCollider collider = subObject.GetComponent<BoxCollider>();

                if(collider != null)
                {
                    //4는 프리팹들의 기본사이즈임 좀더 정확하게하려면 list의 render를 가져와서 사이즈를 측정해야함
                    //가로
                    data.row = (int)(collider.size.x / 4);
                    //세로
                    data.col = (int)(collider.size.z / 4);
                }

                //이름  저장(싱글)
                data.prefabName = subObject.transform.GetChild(0).name;

                //리스트에 넣기
                tempList.Add(data);
            }

            //List<MapData> templist 제이슨화
            string tmp = JsonConvert.SerializeObject(tempList);

            //파일 저장
            File.WriteAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json", tmp);
            MngLog.Instance.addLog(_nodeName + "이 저장되었습니다");
        }
        else //저장시 sub노드가 없으면 아무것도 없는 빈문자열로 저장된 데이터를 초기화함
        {
            string str = null;
            File.WriteAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json", str);
            MngLog.Instance.addLog(_nodeName + "저장할 오브젝트가 없습니다");
        }
    }
    #endregion

    public void BtnLoadMap()
    {
        LoadMulti_Random(floor, "subFloor", objPrefabList_Floor);
        LoadMulti_Random(wall, "subWall", objPrefabList_Wall);
        LoadSingle(deco, "subDeco", objPrefabList_Deco);
    }

    #region Load

    /// <summary>
    /// 멀티 오브젝트 블러오기 및 생성
    /// </summary>
    private void LoadMulti_Random(GameObject _nodeName, string _name, List<GameObject> _PrefabList) 
    {
        //3개의 정보를 다해야함
        string JsonStr = File.ReadAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json");

        //List<MapData>
        List<MapData> LoadListMapData = JsonConvert.DeserializeObject<List<MapData>>(JsonStr);
        MngLog.Instance.addLog(_nodeName.name + ": 제이슨 불러오기 완료");

        //LOL의 크기가 1개이상일때 동작
        if (LoadListMapData != null && LoadListMapData.Count > 0)
        {
            //List<MapData>의 개수만큼 돔
            for (int i = 0; i < LoadListMapData.Count; i++)//subObj의 개수만큼임 하고있음
            {
                //col이 0보다 큼으로 바닥타일
                if(LoadListMapData[i].col > 0)
                {
                    LoadCreateFloorTile(LoadListMapData[i].row, LoadListMapData[i].col, LoadListMapData[i].x, LoadListMapData[i].y, LoadListMapData[i].z, LoadListMapData[i].rotation);
                }
                else // 0보다 작음으로 벽타일
                {
                    LoadCreateWallTile(LoadListMapData[i].row, LoadListMapData[i].x, LoadListMapData[i].y, LoadListMapData[i].z, LoadListMapData[i].rotation);
                }
            }
        }  
        else
        {
            Debug.Log(_name + ": 불러올 내용이 없습니다");
            MngLog.Instance.addLog(_name + ": 불러올 내용이 없습니다");
        }
      
    }

    /// <summary>
    /// 싱글 오브젝트 불러오기 및 생성
    /// </summary>
    private void LoadSingle(GameObject _nodeName, string _name, List<GameObject> _PrefabList)
    {
        //3개의 정보를 다해야함
        string JsonStr = File.ReadAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json");

        //List<MapData>
        List<MapData> LoadListMapData = JsonConvert.DeserializeObject<List<MapData>>(JsonStr);
        MngLog.Instance.addLog(_nodeName.name + ": 제이슨 불러오기 완료");

        //LOL의 크기가 1개이상일때 동작
        if (LoadListMapData != null && LoadListMapData.Count > 0)
        {
            //List<MapData>의 개수만큼 돔
            for (int i = 0; i < LoadListMapData.Count; i++)//subObj의 개수만큼임 하고있음
            {
                //Floor -> 상위 노드(this) -> 111
                GameObject subDeco = new GameObject();
                subDeco.transform.parent = deco.transform;
                subDeco.name = "subDeco";

                int foundIndex = -1;

                //(clone)문구 제거
                string findString = LoadListMapData[i].prefabName.Substring(0, LoadListMapData[i].prefabName.Length - 7);
                for (int j = 0; j < _PrefabList.Count; j++)
                {
                    if(_PrefabList[j].name == findString)
                    {
                        foundIndex = j; // 일치하는 아이템의 인덱스를 저장하고
                        break; // 더 이상 검색할 필요 없으므로 반복문 종료
                    }
                }

                if (foundIndex != -1)
                {
                    // 일치하는 아이템의 인덱스를 사용하여 원하는 작업을 수행
                    GameObject obj = Instantiate(_PrefabList[foundIndex], subDeco.transform);
                    obj.name = _PrefabList[foundIndex].name;
                }
                else
                {
                    // 일치하는 아이템이 없을 경우
                    Debug.Log("일치하는 아이템이 없음");
                    MngLog.Instance.addLog("일치하는 아이템이 없어 생성하지 못했습니다");
                }

                //sub 위치 설정
                subDeco.transform.position = new Vector3(LoadListMapData[i].x, LoadListMapData[i].y, LoadListMapData[i].z);

                //sub rotation 설정
                //Debug.Log(LoadListMapData[i].rotation);
                subDeco.transform.rotation = Quaternion.Euler(0f, LoadListMapData[i].rotation, 0f);

            }
        }
        else
        {
            Debug.Log(_name + ": 불러올 내용이 없습니다");
            MngLog.Instance.addLog(_name + ": 불러올 내용이 없습니다");
        }

    }

    private int FindObjectInList(List<GameObject> _List, string _name)
    {
        int cnt = _List.Count;
        int resultIndex = -1;
        for(int  i =  0; i<cnt; i++)
        {
            if (_List[i].name == _name)
            {
                resultIndex = i;
            }

        }

        return resultIndex;
    }


    #endregion

    #region  create
    /// <summary>
    /// 입력한 가로 x 세로 만큼 타일을 만들어주는 메서드
    /// </summary>
    public void CreateFloorTile()
    {
        //InputField값 가져오기
        string tmp = InputRow.text;
        string tmp2 = InputColumn.text;

        //임시 int
        int resultIntX = 1;
        int resultIntZ = 1;

        int.TryParse(tmp, out resultIntX);
        int.TryParse(tmp2, out resultIntZ);

        //값확인
        if(resultIntX > 0 && resultIntZ > 0)
        {
            TileSizeRow = resultIntZ;
            TileSizecolumn = resultIntX;
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
     
            obj.name = obj.name.ToString();
   

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

        //Log
        MngLog.Instance.addLog(resultIntX + " x "  + resultIntZ + " 크기의 floor를  생성했습니다." );

    }

    /// <summary>
    /// 입력한 가로에따른 타일을 만들어주는 메서드
    /// </summary>
    public void CreateWallTile()
    {
        //프리팹이 차지하는 크기만큼 포지션을 옮겨야하기때문에 프리팹 사이즈를 측정하기위함
        MeshRenderer ren = objPrefabList_Wall[0].GetComponent<MeshRenderer>();
        prefabSize = (int)ren.bounds.size.x;
        float sizeY = ren.bounds.size.y;
        float sizeZ = ren.bounds.size.z;

        //InputField값 가져오기
        string tmp = InputRow.text;

        //임시 int
        int resultIntX = 1;

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
            obj.name = obj.name.ToString();

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

        //Log
        MngLog.Instance.addLog(resultIntX + " x 1 크기의 wall을  생성했습니다.");
    }

    public void LoadCreateFloorTile(int row, int col , float x, float y, float z, float rotation)
    {
        //프리팹이 차지하는 크기만큼 포지션을 옮겨야하기때문에 프리팹 사이즈를 측정하기위함
        MeshRenderer ren = objPrefabList_Floor[0].GetComponent<MeshRenderer>();
        prefabSize = (int)ren.bounds.size.x;

        //for문하나로 z값을 늘려주기위함
        tempZ = 0;

        //총깔아야하는 프리팹 수
        totalPixel = row * col;

        //Floor -> 상위 노드(this) -> 111
        GameObject subFloor = new GameObject();
        subFloor.transform.parent = floor.transform;
        subFloor.name = "subFloor";

        for (int i = 0; i < totalPixel; i++)
        {
            //z축으로 늘리기위함
            if (i % col == 0)
            {
                tempZ += prefabSize;
            }

            //position 설정
            Vector3 Vfloor = new Vector3(i * prefabSize - ((tempZ - prefabSize) * col), 0, tempZ - prefabSize);
            //Debug.Log(Vfloor);

            //오브젝트생성
            GameObject obj = Instantiate(objPrefabList_Floor[Random.Range(0, objPrefabList_Floor.Count)], subFloor.transform);

            //위치지정
            obj.transform.position = Vfloor;

            //오브젝트구분, 이름할당

            obj.name = obj.name.ToString();

            //태그 지정
            obj.tag = "Ground";

            //레이어 지정
            obj.layer = 6;
        }

        //sub position 설정
        subFloor.transform.position = new Vector3(x, y, z);

        //sub rotation 설정
        subFloor.transform.localEulerAngles = new Vector3(0f, rotation, 0f);

        //콜라이더 만들기
        subFloor.AddComponent<BoxCollider>();

        //콜라이더 가져오기
        BoxCollider Cbox = subFloor.GetComponent<BoxCollider>();

        //콜라이더 사이즈 조절
        Cbox.size = new Vector3(col * prefabSize, 0, row * prefabSize);

        //콜라이더 위치 조절
        //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
        Cbox.center = new Vector3(((col * prefabSize / 2) - (prefabSize / 2)),
            0,
            ((row * prefabSize / 2) - (prefabSize / 2)));

        //Log
        MngLog.Instance.addLog(row + " x " + col + " 크기의 floor를  생성했습니다.");

    }

    public void LoadCreateWallTile(int row, float x, float y, float z, float rotation)
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

        //하위 오브젝트 생성
        for (int i = 0; i < row; i++)
        {
            //position 설정
            Vector3 Vwall = new Vector3(i * prefabSize, 0, 0);
            Debug.Log(i +" :"+ Vwall);
            
            //오브젝트생성(랜덤)
            GameObject obj = Instantiate(objPrefabList_Wall[Random.Range(0, objPrefabList_Wall.Count)], subWall.transform);

            //obj 위치지정
            obj.transform.position = Vwall;

            //오브젝트구분 
            obj.name = obj.name.ToString();

            //태그 지정
            obj.tag = "Wall";

            //레이어 지정
            obj.layer = 7;
        }

        //sub position 설정
        subWall.transform.position = new Vector3(x, y, z);

        //sub rotation 설정
        subWall.transform.localEulerAngles = new Vector3(0f, rotation, 0f);

        //콜라이더 만들기
        subWall.AddComponent<BoxCollider>();

        //콜라이더 가져오기
        BoxCollider Cbox = subWall.GetComponent<BoxCollider>();

        //콜라이더 사이즈 조절
        Cbox.size = new Vector3(row * prefabSize, sizeY * 2.3f, sizeZ);

        //콜라이더 위치 조절
        Cbox.center = new Vector3(((row * prefabSize / 2) - (prefabSize / 2)),
             (sizeY / prefabSize) + 2,
             -(prefabSize / 2) - (sizeZ / 2));

        //Log
        MngLog.Instance.addLog( row + " x 1 크기의 wall을  생성했습니다.");
    }

    #endregion


}
