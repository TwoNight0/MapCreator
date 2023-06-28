using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class MapData{
    Vector3 position;
    private int objNum; //개별 오브젝트넘버
    private int LayerNum;
    private string tagName;
    
    public void printData()
    {
        Debug.Log("Position : " + position);
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


    //제이슨으로 저장될리스트
    private List<MapData> mapFloor;
    private List<MapData> mapWall;
    private List<MapData> mapDeco;



    [Header("타일 사이즈")] //타일 사이즈
    [SerializeField, Tooltip("타일의 가로")] int TileSizeX;
    [SerializeField, Tooltip("타일의 높이")] int TileSizeY; //wall만들때 사용
    [SerializeField, Tooltip("타일의 세로")] int TileSizeZ;
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
        GameObject map = new GameObject();
        //상위 오브젝트들(부모)
        GameObject floor = new GameObject(); //바닥의부모
        GameObject wall = new GameObject(); //벽들의 부모
        GameObject deco = new GameObject(); //장식품들의 부모

        //이름할당
        map.name = "Map";
        floor.name = "Floor";
        wall.name = "Wall";
        deco.name = "deco";

        //위초기화
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
        SaveMap(mapFloor, mapWall, mapDeco);
        Debug.Log(mapFloor);
        Debug.Log(Application.dataPath);
    }

    void Update()
    {

    }

    private void SaveMap(List<MapData> _mapFloor, List<MapData> _mapWall, List<MapData> _mapDeco){
        string mapFloor = JsonConvert.SerializeObject(_mapFloor);
        string mapWall = JsonConvert.SerializeObject(_mapFloor);
        string mapDeco = JsonConvert.SerializeObject(_mapFloor);
        File.WriteAllText(Application.dataPath + "/MapJsonFolder/Map.json", mapFloor);
        File.WriteAllText(Application.dataPath + "/MapJsonFolder/Map.json", mapWall);
        File.WriteAllText(Application.dataPath + "/MapJsonFolder/Map.json", mapDeco);

        //Application.persistentDataPath;
        //Application.streamingAssetsPath;
    }

  
    //Start에서 로드된걸 다시 다만들어야함 
    private List<MapData> LoadMap() {
        //예외처리
        //if ((File.ReadAllText(Application.dataPath + "/TestJson.json")) == null){
        //    return;
        //}
        string str = File.ReadAllText(Application.dataPath + "/MapJsonFolder/Map.json");
        List<MapData> result = JsonConvert.DeserializeObject<List<MapData>>(str); //리스트까지됨
        return result; 
    }

    /// <summary>
    /// 바닥타일을 원하는 가로 x 세로 만큼 만들어주는 타일
    /// </summary>
    /// <param name="_parent"></param>
    //이오브젝트의 사이즈즌 4 정도의 간격을 계속줘야함 총타일 % z == 0 z변경해서 위로올리자
    private void createFloorTile(GameObject _parent)
    {
        //프리팹이 차지하는 크기만큼 포지션을 옮겨야하기때문에 프리팹 사이즈를 측정하기위함
        MeshRenderer ren = objPrefabList_Floor[0].GetComponent<MeshRenderer>();
        prefabSize = (int)ren.bounds.size.x;

        //for문하나로 z값을 늘려주기위함
        tempZ = 0;

        //총깔아야하는 프리팹 수
        totalPixel = TileSizeX * TileSizeZ;

        //상위오브젝트
        for (int i = 0; i< totalPixel; i++)
        {
            if(i % TileSizeZ == 0)
            {
                tempZ += prefabSize;
            }
            Vector3 Vfloor = new Vector3( i * prefabSize, 0, tempZ);
            //오브젝트생성(프리팹, 벡터(i * 4, 0 , ), 부모) -> 위치, 부모, 해결
            GameObject obj = Instantiate(objPrefabList_Floor[Random.Range(0, max_Floor)], _parent.transform);
            MapData data = obj.GetComponent<MapData>();

            //위치지정
            obj.transform.position = Vfloor;

            //리스트에 추가
            mapFloor.Add(data);

            //오브젝트구분 objNum++
            

            //태그 지정


            //레이어 지정
        }
        //콜라이더 만들기

        //콜라이더 크기 및 위치 수정
    }

    private void createWallTile(GameObject _parent)
    {

    }

    private void createFWTile(GameObject _parent)
    {

    }


}
