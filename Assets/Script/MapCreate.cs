using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// ��ġ������ ���̾�,�±� ������ �������־ ���̽������͸� �������� �ٸ� ���������ε� ���������� ���� ���������
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
    [Header("������ ����Ʈ")] //�����ո���Ʈ
    [SerializeField, Tooltip("�ٴڿ� ���� ������")] private List<GameObject> objPrefabList_Floor;
    [SerializeField, Tooltip("���� ���� ������")] private List<GameObject> objPrefabList_Wall;
    [Space(20)]

    //�ֻ���(root)
    private GameObject map;
    //���� ���
    private GameObject floor;
    private GameObject wall;
    private GameObject deco;


    //���̽����� ����ɸ���Ʈ
    private List<MapData> mapFloor;
    private List<MapData> mapWall;
    private List<MapData> mapDeco;


    [Header("Ÿ�� ������")] //Ÿ�� ������
    [SerializeField, Tooltip("Ÿ���� ����, 1�̻��̿�����")] int TileSizeX;
    [SerializeField, Tooltip("Ÿ���� ����, 1�̻��̿�����")] int TileSizeZ;
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
        map = new GameObject();
        //���� ������Ʈ��(�θ�)
        floor = new GameObject(); //�ٴ��Ǻθ�
        wall = new GameObject(); //������ �θ�
        deco = new GameObject(); //���ǰ���� �θ�


        //����Ʈ �Ҵ�(���̽� �����)
        mapFloor = new List<MapData>();
        mapWall = new List<MapData>();
        mapDeco = new List<MapData>();

        //�̸��Ҵ�
        map.name = "Map";
        floor.name = "Floor";
        wall.name = "Wall";
        deco.name = "deco";

        //��ġ �ʱ�ȭ
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
        createWallTile();


    }

    void Update()
    {
        
    }

    public void SaveMap(List<MapData> _mapFloor, List<MapData> _mapWall, List<MapData> _mapDeco){
        //����ȭ
        string mapFloor = JsonConvert.SerializeObject(_mapFloor);
        string mapWall = JsonConvert.SerializeObject(_mapWall);
        string mapDeco = JsonConvert.SerializeObject(_mapDeco);

        //���̽� ����
        File.WriteAllText(Application.dataPath + "/MapJsonFolder/Map.json", mapFloor);
        File.WriteAllText(Application.dataPath + "/MapJsonFolder/Map.json", mapWall);
        File.WriteAllText(Application.dataPath + "/MapJsonFolder/Map.json", mapDeco);

        //Application.persistentDataPath;
        //Application.streamingAssetsPath;
    }

    //Start���� �ε�Ȱ� �ٽ� �ٸ������� 
    public List<MapData> LoadMap() {
        //����ó�� ������ ����
        //if ((File.ReadAllText(Application.dataPath + "/TestJson.json")) == null){
        //    return;
        //}
        string str = File.ReadAllText(Application.dataPath + "/MapJsonFolder/Map.json");
        List<MapData> result = JsonConvert.DeserializeObject<List<MapData>>(str); //����Ʈ������
        return result; 
    }

    /// <summary>
    /// �Է��� ���� x ���� ��ŭ Ÿ���� ������ִ� �޼���
    /// </summary>
    public void createFloorTile()
    {
        //�������� �����ϴ� ũ�⸸ŭ �������� �Űܾ��ϱ⶧���� ������ ����� �����ϱ�����
        MeshRenderer ren = objPrefabList_Floor[0].GetComponent<MeshRenderer>();
        prefabSize = (int)ren.bounds.size.x;

        //for���ϳ��� z���� �÷��ֱ�����
        tempZ = 0;

        //�ѱ�ƾ��ϴ� ������ ��
        totalPixel = TileSizeX * TileSizeZ;

        //Floor -> ���� ���(this) -> 111
        GameObject subFloor = new GameObject();
        subFloor.transform.parent = floor.transform;
        subFloor.name = "subFloor";

        for (int i = 0; i< totalPixel; i++)
        {
            //z������ �ø�������
            if(i % TileSizeZ == 0)
            {
                tempZ += prefabSize;
            }

            //position ����
            Vector3 Vfloor = new Vector3( i * prefabSize - ((tempZ - prefabSize)* TileSizeZ), 0, tempZ-prefabSize);
            //Debug.Log(Vfloor);

            //������Ʈ����
            GameObject obj = Instantiate(objPrefabList_Floor[Random.Range(0, max_Floor)], subFloor.transform);
           
            //��ġ����
            obj.transform.position = Vfloor;

            //MapData �ֱ�
            obj.AddComponent<MapData>();

            //MapData ��������
            MapData data = obj.GetComponent<MapData>();
            data.pos = Vfloor;

            //�׽�Ʈ��
            //Debug.Log(data.pos);
            //string test = JsonConvert.SerializeObject(data)

            //string test = JsonConvert.SerializeObject(data, Formatting.None,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });
            //File.WriteAllText(Application.dataPath + "/MapJsonFolder/Map.json", test);

            //����Ʈ�� �߰�
            mapFloor.Add(data);



            //������Ʈ���� 
            obj.name = "floor"+ objNum.ToString();
            objNum++;

            //�±� ����
            obj.tag = "Ground";
            data.tagName = "Ground";

            //���̾� ����
            obj.layer = 6;
            data.LayerNum = 6;
        }
        //�ݶ��̴� �����
        subFloor.AddComponent<BoxCollider>();

        //�ݶ��̴� ��������
        BoxCollider Cbox = subFloor.GetComponent<BoxCollider>();

        //�ݶ��̴� ������ ����
        Cbox.size = new Vector3(TileSizeZ * prefabSize, 0, TileSizeX * prefabSize);

        //�ݶ��̴� ��ġ ����
        //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
        Cbox.center = new Vector3(((TileSizeZ * prefabSize / 2) - (prefabSize / 2)), 
            0, 
            ((TileSizeX * prefabSize / 2) - (prefabSize / 2)));

        //�ٽ� ����ϱ� ���� �ʱ�ȭ
        TileSizeX = 0;
        TileSizeZ = 0;

    }

    /// <summary>
    /// �Է��� ���ο����� Ÿ���� ������ִ� �޼���
    /// </summary>
    public void createWallTile()
    {
        //�������� �����ϴ� ũ�⸸ŭ �������� �Űܾ��ϱ⶧���� ������ ����� �����ϱ�����
        MeshRenderer ren = objPrefabList_Wall[0].GetComponent<MeshRenderer>();
        prefabSize = (int)ren.bounds.size.x;
        float sizeY = ren.bounds.size.y;
        float sizeZ = ren.bounds.size.z;

        //Floor -> ���� ���(this) -> 111
        GameObject subWall = new GameObject();

        // subWall�� �θ��� ����
        subWall.transform.parent = wall.transform;
        subWall.name = "subWall";

        for (int i = 0; i < TileSizeX; i++)
        {
            //position ����
            Vector3 Vwall = new Vector3(i * prefabSize, 0, 0);
            //Debug.Log(Vwall);

            //������Ʈ����
            GameObject obj = Instantiate(objPrefabList_Wall[Random.Range(0, max_Wall)], subWall.transform);

            //obj ��ġ����
            obj.transform.position = Vwall;

            //MapData �ֱ�
            obj.AddComponent<MapData>();

            //MapData ��������
            MapData data = obj.GetComponent<MapData>();
            data.pos = Vwall;


            //����Ʈ�� �߰�
            mapWall.Add(data);


            //������Ʈ���� 
            obj.name = "wall" + objNum.ToString();
            objNum++;

            //�±� ����
            obj.tag = "Wall";
            data.tagName = "Wall";

            //���̾� ����
            obj.layer = 7;
            data.LayerNum = 7;
        }
        //�ݶ��̴� �����
        subWall.AddComponent<BoxCollider>();

        //�ݶ��̴� ��������
        BoxCollider Cbox = subWall.GetComponent<BoxCollider>();

        //�ݶ��̴� ������ ����
        Cbox.size = new Vector3(TileSizeX * prefabSize, sizeY, sizeZ);

        //�ݶ��̴� ��ġ ����
        //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
        Cbox.center = new Vector3(((TileSizeX * prefabSize / 2) - (prefabSize / 2)),
             sizeY / (prefabSize/2),
             -(prefabSize / 2) - (sizeZ/2));
        //((TileSizeX * prefabSize / 2) - (prefabSize / 2))
        //�ٽ� ����ϱ� ���� �ʱ�ȭ
        TileSizeX = 0;
    }

}
