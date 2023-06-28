using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class MapData{
    Vector3 position;

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
    [SerializeField, Tooltip("바닥에 사용될 프리팹")] private List<GameObject> objList_Floor;
    [SerializeField, Tooltip("벽에 사용될 프리팹")] private List<GameObject> objList_Wall;
    [Space(20)]

    [Header("타일 사이즈")] //타일 사이즈
    [SerializeField, Tooltip("타일의 가로")] int TileSizeX;
    [SerializeField, Tooltip("타일의 세로")] int TileSizeZ;

    [Tooltip("floor리스트의 최댓값")] private int max_Floor;
    [Tooltip("wall리스트의 최댓값")] private int max_Wall;



    private void Awake(){
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

        //프리팹 수의 최대값지정
        max_Floor = objList_Floor.Count;
        max_Wall = objList_Wall.Count;

    }

    void Start()
    {
          
    }

    void Update()
    {

    }

    private void SaveMap(MapData _map){
        File.WriteAllText(Application.dataPath + "/TestJson.json", JsonUtility.ToJson(_map));
    }

    //시작할때 이걸 저장된걸 불러오면될듯 
    private MapData LoadMap() {
        string str = File.ReadAllText(Application.dataPath + "/TestJson.json");
        MapData result = JsonUtility.FromJson<MapData>(str);
        return result; 
    }


    //이오브젝트의 사이즈즌 4 정도의 간격을 계속줘야함 총타일 % z == 0 z변경해서 위로올리자
    private void createTile(int sizeX, GameObject _parent)
    {

        for(int i = 0; i< sizeX; i++)
        {
            //오브젝트생성(프리팹, 벡터(i * 4, 0 , ), 부모) -> 위치, 부모, 해결
            GameObject obj = Instantiate(objList_Floor[Random.Range(0, max_Floor)]);


            //만들때마다 리스트에 추가해야겠지..?
            //이름지정 ground + i
            

            //태그 지정


            //레이어 지정
        }
    }


}
