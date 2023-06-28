using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class MapData{
    Vector3 position;
    private int objNum; //���� ������Ʈ�ѹ�
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
    [Header("������ ����Ʈ")] //�����ո���Ʈ
    [SerializeField, Tooltip("�ٴڿ� ���� ������")] private List<GameObject> objPrefabList_Floor;
    [SerializeField, Tooltip("���� ���� ������")] private List<GameObject> objPrefabList_Wall;
    [Space(20)]


    //���̽����� ����ɸ���Ʈ
    private List<MapData> mapFloor;
    private List<MapData> mapWall;
    private List<MapData> mapDeco;



    [Header("Ÿ�� ������")] //Ÿ�� ������
    [SerializeField, Tooltip("Ÿ���� ����")] int TileSizeX;
    [SerializeField, Tooltip("Ÿ���� ����")] int TileSizeY; //wall���鶧 ���
    [SerializeField, Tooltip("Ÿ���� ����")] int TileSizeZ;
    private int totalPixel;
    private int prefabSize; //�������ϳ��� ũ�� 
    private int tempZ;

    //������Ʈ ������ȣ
    private int objNum;

    [Tooltip("floor����Ʈ�� �ִ�")] private int max_Floor;
    [Tooltip("wall����Ʈ�� �ִ�")] private int max_Wall;



    private void Awake()
    {
        //�ֻ��� ������Ʈ
        GameObject map = new GameObject();
        //���� ������Ʈ��(�θ�)
        GameObject floor = new GameObject(); //�ٴ��Ǻθ�
        GameObject wall = new GameObject(); //������ �θ�
        GameObject deco = new GameObject(); //���ǰ���� �θ�

        //�̸��Ҵ�
        map.name = "Map";
        floor.name = "Floor";
        wall.name = "Wall";
        deco.name = "deco";

        //���ʱ�ȭ
        map.transform.position = Vector3.zero;
        floor.transform.position = Vector3.zero;
        wall.transform.position = Vector3.zero;
        deco.transform.position = Vector3.zero;

        //map�� �ֻ��� �θ�� ����
        floor.transform.parent = map.transform;
        wall.transform.parent = map.transform;
        deco.transform.parent = map.transform;

        //������ ���� �ִ밪(�����ϰ� �������� �������� �����)
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

  
    //Start���� �ε�Ȱ� �ٽ� �ٸ������� 
    private List<MapData> LoadMap() {
        //����ó��
        //if ((File.ReadAllText(Application.dataPath + "/TestJson.json")) == null){
        //    return;
        //}
        string str = File.ReadAllText(Application.dataPath + "/MapJsonFolder/Map.json");
        List<MapData> result = JsonConvert.DeserializeObject<List<MapData>>(str); //����Ʈ������
        return result; 
    }

    /// <summary>
    /// �ٴ�Ÿ���� ���ϴ� ���� x ���� ��ŭ ������ִ� Ÿ��
    /// </summary>
    /// <param name="_parent"></param>
    //�̿�����Ʈ�� �������� 4 ������ ������ �������� ��Ÿ�� % z == 0 z�����ؼ� ���οø���
    private void createFloorTile(GameObject _parent)
    {
        //�������� �����ϴ� ũ�⸸ŭ �������� �Űܾ��ϱ⶧���� ������ ����� �����ϱ�����
        MeshRenderer ren = objPrefabList_Floor[0].GetComponent<MeshRenderer>();
        prefabSize = (int)ren.bounds.size.x;

        //for���ϳ��� z���� �÷��ֱ�����
        tempZ = 0;

        //�ѱ�ƾ��ϴ� ������ ��
        totalPixel = TileSizeX * TileSizeZ;

        //����������Ʈ
        for (int i = 0; i< totalPixel; i++)
        {
            if(i % TileSizeZ == 0)
            {
                tempZ += prefabSize;
            }
            Vector3 Vfloor = new Vector3( i * prefabSize, 0, tempZ);
            //������Ʈ����(������, ����(i * 4, 0 , ), �θ�) -> ��ġ, �θ�, �ذ�
            GameObject obj = Instantiate(objPrefabList_Floor[Random.Range(0, max_Floor)], _parent.transform);
            MapData data = obj.GetComponent<MapData>();

            //��ġ����
            obj.transform.position = Vfloor;

            //����Ʈ�� �߰�
            mapFloor.Add(data);

            //������Ʈ���� objNum++
            

            //�±� ����


            //���̾� ����
        }
        //�ݶ��̴� �����

        //�ݶ��̴� ũ�� �� ��ġ ����
    }

    private void createWallTile(GameObject _parent)
    {

    }

    private void createFWTile(GameObject _parent)
    {

    }


}
