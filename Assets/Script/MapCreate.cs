using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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


    //제이슨으로 저장될리스트
    private List<MapData> mapFloor;
    private List<MapData> mapWall;
    private List<MapData> mapDeco;


    [Header("타일 사이즈")] //타일 사이즈
    [SerializeField, Tooltip("타일의 가로, 1이상이여야함")] int TileSizeRow;
    [SerializeField, Tooltip("타일의 세로, 1이상이여야함")] int TileSizeColumnm;
    private int totalPixel;
    private int prefabSize; //프리팹하나의 크기 
    private int tempZ;

    //오브젝트 고유번호
    private int objNum;

    [Tooltip("floor리스트의 최댓값")] private int max_Floor;
    [Tooltip("wall리스트의 최댓값")] private int max_Wall;



    private void Awake()
    {
        //최상위 오브젝트
        map = new GameObject();
        //상위 오브젝트들(부모)
        floor = new GameObject(); //바닥의부모
        wall = new GameObject(); //벽들의 부모
        deco = new GameObject(); //장식품들의 부모


        //리스트 할당(제이슨 저장용)
        mapFloor = new List<MapData>();
        mapWall = new List<MapData>();
        mapDeco = new List<MapData>();

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

    }

    void Start()
    {
        


    }

    void Update()
    {
        
    }

    /// <summary>
    /// 1. 저장시 subOOO 을 모두 찾고 그 밑의 MapData들을 모두 리스트에 저장
    /// 2. 저장된 리스트를 제이슨화
    /// 3. 파일로 저장
    /// </summary>
    public void SaveMap(){

        //GameObject[] subF = new GameObject[];
        //FLOOR -> SUBFLOOR 여러개

        //상위 노드를 찾음
        if (mapFloor.Count != 0) {
            for (int i = 0; i < mapFloor.Count; i++) {
                //subF = GameObject.Find("subFloor");
            }
        }
        
        GameObject subW = GameObject.Find("subWall");
        GameObject Deco = GameObject.Find("Deco");

        //리스트에 저장
        //MapData[] Mapdata = subF.GetComponentsInChildren<MapData>();

        //직렬화
        string JmapFloor = JsonConvert.SerializeObject(mapFloor);
        string JmapWall = JsonConvert.SerializeObject(mapWall);
        string JmapDeco = JsonConvert.SerializeObject(mapDeco);

        //제이슨 저장
        File.WriteAllText(Application.dataPath + "/MapJsonFolder/MapFloor.json", JmapFloor);
        File.WriteAllText(Application.dataPath + "/MapJsonFolder/MapWall.json", JmapWall);
        File.WriteAllText(Application.dataPath + "/MapJsonFolder/MapDeco.json", JmapDeco);

        //안될경우를 대비함
        //Application.persistentDataPath;
        //Application.streamingAssetsPath;


        ////저장
        //string test = JsonUtility.ToJson(data);
        //File.WriteAllText(Application.dataPath + "/MapJsonFolder/Map.json", test);

        ////리스트에 추가
        //mapFloor.Add(data);
    }

    /// <summary>
    /// 0. 스타트시 일단 한번 실행
    /// 1. 제이슨데이터 파일를 가져와 역직렬화
    /// 2. 리스트로 다시 가져오고 
    /// 3. 리스트의 길이만큼 반복문을 돌면서 위치에 맞춰 오브젝트를 생성
    /// 4. 태그와 레이어 조정
    /// </summary>
    /// <returns></returns>
    public List<MapData> LoadMap() {
        //예외처리 없으면 만듬
        //if ((File.ReadAllText(Application.dataPath + "/TestJson.json")) == null){
        //    return;
        //}

        //3개의 정보를 다해야함
        string str = File.ReadAllText(Application.dataPath + "/MapJsonFolder/Map.json");
        List<MapData> result = JsonConvert.DeserializeObject<List<MapData>>(str); //리스트까지됨
        return result; 
    }

    /// <summary>
    /// 입력한 가로 x 세로 만큼 타일을 만들어주는 메서드
    /// </summary>
    public void createFloorTile()
    {
        //프리팹이 차지하는 크기만큼 포지션을 옮겨야하기때문에 프리팹 사이즈를 측정하기위함
        MeshRenderer ren = objPrefabList_Floor[0].GetComponent<MeshRenderer>();
        prefabSize = (int)ren.bounds.size.x;

        //for문하나로 z값을 늘려주기위함
        tempZ = 0;

        //총깔아야하는 프리팹 수
        totalPixel = TileSizeRow * TileSizeColumnm;

        //Floor -> 상위 노드(this) -> 111
        GameObject subFloor = new GameObject();
        subFloor.transform.parent = floor.transform;
        subFloor.name = "subFloor";

        for (int i = 0; i< totalPixel; i++)
        {
            //z축으로 늘리기위함
            if(i % TileSizeColumnm == 0)
            {
                tempZ += prefabSize;
            }

            //position 설정
            Vector3 Vfloor = new Vector3( i * prefabSize - ((tempZ - prefabSize)* TileSizeColumnm), 0, tempZ-prefabSize);
            //Debug.Log(Vfloor);

            //오브젝트생성
            GameObject obj = Instantiate(objPrefabList_Floor[Random.Range(0, max_Floor)], subFloor.transform);
           
            //위치지정
            obj.transform.position = Vfloor;

            //MapData 넣기
            obj.AddComponent<MapData>();

            //MapData 가져오기
            MapData data = obj.GetComponent<MapData>();

            // data  position 값 할당
            data.pos = Vfloor;

            //오브젝트구분 
            obj.name = "floor"+ objNum.ToString();
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
        Cbox.size = new Vector3(TileSizeColumnm * prefabSize, 0, TileSizeRow * prefabSize);

        //콜라이더 위치 조절
        //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
        Cbox.center = new Vector3(((TileSizeColumnm * prefabSize / 2) - (prefabSize / 2)), 
            0, 
            ((TileSizeRow * prefabSize / 2) - (prefabSize / 2)));

        //다시 사용하기 위한 초기화
        TileSizeRow = 1;
        TileSizeColumnm = 1;

    }

    /// <summary>
    /// 입력한 가로에따른 타일을 만들어주는 메서드
    /// </summary>
    public void createWallTile()
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

            //data position 값 할당
            data.pos = Vwall;

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
             sizeY / (prefabSize/2),
             -(prefabSize / 2) - (sizeZ/2));

        //((TileSizeX * prefabSize / 2) - (prefabSize / 2))
        //다시 사용하기 위한 초기화
        TileSizeRow = 1;
    }

}
