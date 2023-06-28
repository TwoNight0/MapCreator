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
    [SerializeField, Tooltip("타일의 가로, 1이상이여야함")] int TileSizeX;
    [SerializeField, Tooltip("타일의 세로, 1이상이여야함")] int TileSizeZ;
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
        deco.name = "deco";

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
        createWallTile();


    }

    void Update()
    {
        
    }

    public void SaveMap(List<MapData> _mapFloor, List<MapData> _mapWall, List<MapData> _mapDeco){
        //직렬화
        string mapFloor = JsonConvert.SerializeObject(_mapFloor);
        string mapWall = JsonConvert.SerializeObject(_mapWall);
        string mapDeco = JsonConvert.SerializeObject(_mapDeco);

        //제이슨 저장
        File.WriteAllText(Application.dataPath + "/MapJsonFolder/Map.json", mapFloor);
        File.WriteAllText(Application.dataPath + "/MapJsonFolder/Map.json", mapWall);
        File.WriteAllText(Application.dataPath + "/MapJsonFolder/Map.json", mapDeco);

        //Application.persistentDataPath;
        //Application.streamingAssetsPath;
    }

    //Start에서 로드된걸 다시 다만들어야함 
    public List<MapData> LoadMap() {
        //예외처리 없으면 만듬
        //if ((File.ReadAllText(Application.dataPath + "/TestJson.json")) == null){
        //    return;
        //}
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
        totalPixel = TileSizeX * TileSizeZ;

        //Floor -> 상위 노드(this) -> 111
        GameObject subFloor = new GameObject();
        subFloor.transform.parent = floor.transform;
        subFloor.name = "subFloor";

        for (int i = 0; i< totalPixel; i++)
        {
            //z축으로 늘리기위함
            if(i % TileSizeZ == 0)
            {
                tempZ += prefabSize;
            }

            //position 설정
            Vector3 Vfloor = new Vector3( i * prefabSize - ((tempZ - prefabSize)* TileSizeZ), 0, tempZ-prefabSize);
            //Debug.Log(Vfloor);

            //오브젝트생성
            GameObject obj = Instantiate(objPrefabList_Floor[Random.Range(0, max_Floor)], subFloor.transform);
           
            //위치지정
            obj.transform.position = Vfloor;

            //MapData 넣기
            obj.AddComponent<MapData>();

            //MapData 가져오기
            MapData data = obj.GetComponent<MapData>();
            data.pos = Vfloor;

            //테스트중
            //Debug.Log(data.pos);
            //string test = JsonConvert.SerializeObject(data)

            //string test = JsonConvert.SerializeObject(data, Formatting.None,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });
            //File.WriteAllText(Application.dataPath + "/MapJsonFolder/Map.json", test);

            //리스트에 추가
            mapFloor.Add(data);



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
        Cbox.size = new Vector3(TileSizeZ * prefabSize, 0, TileSizeX * prefabSize);

        //콜라이더 위치 조절
        //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
        Cbox.center = new Vector3(((TileSizeZ * prefabSize / 2) - (prefabSize / 2)), 
            0, 
            ((TileSizeX * prefabSize / 2) - (prefabSize / 2)));

        //다시 사용하기 위한 초기화
        TileSizeX = 0;
        TileSizeZ = 0;

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

        for (int i = 0; i < TileSizeX; i++)
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
            data.pos = Vwall;


            //리스트에 추가
            mapWall.Add(data);


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
        Cbox.size = new Vector3(TileSizeX * prefabSize, sizeY, sizeZ);

        //콜라이더 위치 조절
        //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
        Cbox.center = new Vector3(((TileSizeX * prefabSize / 2) - (prefabSize / 2)),
             sizeY / (prefabSize/2),
             -(prefabSize / 2) - (sizeZ/2));
        //((TileSizeX * prefabSize / 2) - (prefabSize / 2))
        //다시 사용하기 위한 초기화
        TileSizeX = 0;
    }

}
