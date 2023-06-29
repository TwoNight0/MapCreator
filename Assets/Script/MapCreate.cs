using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using System.Text;


/// <summary>
/// ��ġ������ ���̾�,�±� ������ �������־ ���̽������͸� �������� �ٸ� ���������ε� ���������� ���� ���������
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

    //subObj ������ ����Ʈ
    private List<GameObject> mapFloor;
    private List<GameObject> mapWall;
    private List<GameObject> mapDeco;

    //���̽� ����� arr
    MapData[] arrFloor;
    MapData[] arrWall;
    MapData[] arrDeco;

    [Header("Ÿ�� ������")] //Ÿ�� ������
    [SerializeField, Tooltip("Ÿ���� ����, 1�̻��̿�����")] int TileSizeRow;
    [SerializeField, Tooltip("Ÿ���� ����, 1�̻��̿�����")] int TileSizecolumn;
    private int totalPixel;
    private int prefabSize; //�������ϳ��� ũ�� 
    private int tempZ;

    //�Է°�
    public InputField InputRow;
    public InputField InputColumn;

    //������Ʈ ������ȣ
    private int objNum;

    //���̽������
    StringBuilder JsonSbFloor;
    StringBuilder JsonSbWall;
    StringBuilder JsonSbDeco;


    [Tooltip("floor����Ʈ�� �ִ�")] private int max_Floor;
    [Tooltip("wall����Ʈ�� �ִ�")] private int max_Wall;

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

    //�ʱ�ȭ
    private void initData() 
    {
        //�ֻ��� ������Ʈ
        map = new GameObject();
        //���� ������Ʈ��(�θ�)
        floor = new GameObject(); //�ٴ��Ǻθ�
        wall = new GameObject(); //������ �θ�
        deco = new GameObject(); //���ǰ���� �θ�

        //����Ʈ �Ҵ� subObj ������
        mapFloor = new List<GameObject>();
        mapWall = new List<GameObject>();
        mapDeco = new List<GameObject>();

        //�̸��Ҵ�
        map.name = "Map";
        floor.name = "Floor";
        wall.name = "Wall";
        deco.name = "Deco";

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

        //Ÿ�� �ʱ�ȭ
        TileSizeRow = 1;
        TileSizecolumn = 1;

        //StringBuilder �ʱ�ȭ
        JsonSbFloor = new StringBuilder();
        JsonSbWall = new StringBuilder();
        JsonSbDeco = new StringBuilder();
    }

    /// <summary>
    /// 1. ����� subOOO �� ��� ã�� �� ���� MapData���� ��� ����Ʈ�� ����
    /// 2. ����� ����Ʈ�� ���̽�ȭ
    /// 3. ���Ϸ� ����
    /// </summary>
    public void BtnSaveMap()
    {
        //����� ���̽� �ʱ�ȭ
        RemoveJson("MapFloor", JsonSbFloor);
        RemoveJson("MapWall", JsonSbWall);
        RemoveJson("MapDeco", JsonSbDeco);

        //����Ʈ �ʱ�ȭ
        mapFloor.Clear();
        mapWall.Clear();
        mapDeco.Clear();

        //����Ʈ�߰� �� MapData ������ �Ҵ�
        AddObjList(mapFloor, "Floor");
        AddObjList(mapWall, "Wall");
        AddObjList(mapDeco, "Deco");

        //����
        SaveObj(mapFloor, arrFloor, "MapFloor", JsonSbFloor);
        SaveObj(mapWall, arrWall, "MapWall", JsonSbWall);
        SaveObj(mapDeco, arrDeco, "MapDeco", JsonSbDeco);
    }

    private void SaveObj(List<GameObject> _List, MapData[] _arr, string _JsonName, StringBuilder _SBJson)
    {
        if (_List.Count != 0)
        {
            //����Ʈ�� ũ�⸦ ��.  ����Ʈ�� ũ��� �����Ҷ� �߰� ����
            for (int i = 0; i < _List.Count; i++)
            {
                _arr = _List[i].GetComponentsInChildren<MapData>();
                //����ȭ �� ���ڿ� ��ü 
                for (int k = 0; k < _arr.Length; k++)
                {
                    string tmp = JsonUtility.ToJson(_arr[k]);
                    _SBJson.Append(tmp);
                    tmp = null;
                }
            }
            //StringBuilder -> string ��ȯ
            string tmep = _SBJson.ToString();
            Debug.Log(tmep);

            //���̽� ����
            File.WriteAllText(Application.dataPath + "/MapJsonFolder/" + _JsonName + ".json", tmep);
        }
    }
    public void BtnRemoveJson()
    {
        //����� ���̽� �ʱ�ȭ
        RemoveJson("MapFloor", JsonSbFloor);
        RemoveJson("MapWall", JsonSbWall);
        RemoveJson("MapDeco", JsonSbDeco);
    }

    /// <summary>
    /// ���̽� �ʱ�ȭ
    /// </summary>
    /// <param name="_JsonName"></param>
    /// <param name="_SBJson"></param>
    private void RemoveJson(string _JsonName, StringBuilder _SBJson)
    {
        _SBJson.Clear();
        string tmep = _SBJson.ToString();

        //���̽� ����
        File.WriteAllText(Application.dataPath + "/MapJsonFolder/" + _JsonName + ".json", tmep);
    }

    /// <summary>
    /// ����Ʈ�� �߰� �� MapData ������ �Ҵ�
    /// </summary>
    /// <param name="_List"> mapFloor,mapWall,mapDeco</param>
    /// <param name="_FWD">Floor, Wall, Deco</param>
    private void AddObjList(List<GameObject> _List, string _FWD)
    {
        //���� ��� ã�� obj = Floor
        GameObject obj = GameObject.Find(_FWD);
        // obj = Floor
        // obj.child = subFloor
        // obj.child.child = floor <= ã�¿�����Ʈ 
        // �� ������Ʈ�� ��ġ���� 
        // MapData.pos�� �־����

        //������� �Ʒ��� ������ ���� ����Ʈ�� ���� 
        if (obj.transform.childCount > 0)
        { // subObj�� ����? ������ �Ʒ���
            //subObj = subFloor
            int count = obj.transform.childCount; //floor0 ~�� ����
            for (int i = 0; i < count; i++)
            {
                _List.Add(obj.transform.GetChild(i).gameObject); //ex) Floor�� subFloor�� ����
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
    /// �ش��ο� �ش��ϴ� ����Ʈ�� ��ȯ��(������ �󸮽�Ʈ�� ��ȯ)
    /// 0. ��ŸƮ�� �ϴ� �ѹ� ����
    /// 1. ���̽������� ���ϸ� ������ ������ȭ
    /// 2. ����Ʈ�� �ٽ� �������� 
    /// 3. ����Ʈ�� ���̸�ŭ �ݺ����� ���鼭 ��ġ�� ���� ������Ʈ�� ����
    /// 4. �±׿� ���̾� ����
    /// </summary>
    /// <returns></returns>
    private List<MapData> LoadMap(string _JsonName)
    {
        //��ο� ����Ȱ� ������ ���ο�� ����
        if ((File.ReadAllText(Application.dataPath + "/MapJsonFolder/" + _JsonName + ".json")) == null){
            List<MapData> newList = new List<MapData>();
            Debug.Log("���� ���� �󸮽�Ʈ�� ��ȯ��");
            return newList;
        }

        //3���� ������ ���ؾ���
        string str = File.ReadAllText(Application.dataPath + "/MapJsonFolder/" + _JsonName + ".json");
        //List<MapData> result = JsonConvert.DeserializeObject<List<MapData>>(str); //������
        List<MapData> result = JsonUtility.FromJson<List<MapData>>(str);
        Debug.Log("����� ����");
        return result;
    }

    private void LoadMapCreate(List<MapData> _loadResult, string _name, GameObject _parent) 
    {
        if(_loadResult == null) {
            return;
        }

        //subObj ���� �� �̸� �Ҵ�
        GameObject subObj = new GameObject(_name);

        //�θ� ������ֱ�
        subObj.transform.parent = _parent.transform;

        //����Ʈ�� ���̸�ŭ �ϳ��ϳ��� ��������
        for(int i = 0; i<_loadResult.Count; i++)
        {
            //���� �� �θ���
            GameObject obj = Instantiate(objPrefabList_Floor[Random.Range(0, max_Floor)], subObj.transform);

            //pos ����
            obj.transform.position = _loadResult[i].pos;

            //tag �±�
            obj.transform.tag = _loadResult[i].tag;

            //layer ���̾�
            obj.layer = _loadResult[i].LayerNum;

        }

    }

    #region  create
    /// <summary>
    /// �Է��� ���� x ���� ��ŭ Ÿ���� ������ִ� �޼���
    /// </summary>
    public void BtnCreateFloorTile()
    {
        //InputField�� ��������
        string tmp = InputRow.text;
        string tmp2 = InputRow.text;

        //�ӽ� int
        int resultIntX, resultIntZ = 0;

        int.TryParse(tmp, out resultIntX);
        int.TryParse(tmp2, out resultIntZ);

        //��Ȯ��
        if(resultIntX > 0 && resultIntZ > 0)
        {
            TileSizeRow = resultIntX;
            TileSizecolumn = resultIntZ;
        }

        //�������� �����ϴ� ũ�⸸ŭ �������� �Űܾ��ϱ⶧���� ������ ����� �����ϱ�����
        MeshRenderer ren = objPrefabList_Floor[0].GetComponent<MeshRenderer>();
        prefabSize = (int)ren.bounds.size.x;

        //for���ϳ��� z���� �÷��ֱ�����
        tempZ = 0;

        //�ѱ�ƾ��ϴ� ������ ��
        totalPixel = TileSizeRow * TileSizecolumn;

        //Floor -> ���� ���(this) -> 111
        GameObject subFloor = new GameObject();
        subFloor.transform.parent = floor.transform;
        subFloor.name = "subFloor";

        ////����Ʈ�� �߰�
        mapFloor.Add(subFloor);

        for (int i = 0; i < totalPixel; i++)
        {
            //z������ �ø�������
            if (i % TileSizecolumn == 0)
            {
                tempZ += prefabSize;
            }

            //position ����
            Vector3 Vfloor = new Vector3(i * prefabSize - ((tempZ - prefabSize) * TileSizecolumn), 0, tempZ - prefabSize);
            //Debug.Log(Vfloor);

            //������Ʈ����
            GameObject obj = Instantiate(objPrefabList_Floor[Random.Range(0, max_Floor)], subFloor.transform);

            //��ġ����
            obj.transform.position = Vfloor;

            //MapData �ֱ�
            obj.AddComponent<MapData>();

            //MapData ��������
            MapData data = obj.GetComponent<MapData>();

            //������Ʈ����, �̸��Ҵ�
            obj.name = "floor" + objNum.ToString();
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
        Cbox.size = new Vector3(TileSizecolumn * prefabSize, 0, TileSizeRow * prefabSize);

        //�ݶ��̴� ��ġ ����
        //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
        Cbox.center = new Vector3(((TileSizecolumn * prefabSize / 2) - (prefabSize / 2)),
            0,
            ((TileSizeRow * prefabSize / 2) - (prefabSize / 2)));

        //�ٽ� ����ϱ� ���� �ʱ�ȭ
        TileSizeRow = 1;
        TileSizecolumn = 1;
    }

    /// <summary>
    /// �Է��� ���ο����� Ÿ���� ������ִ� �޼���
    /// </summary>
    public void BtnCreateWallTile()
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

        for (int i = 0; i < TileSizeRow; i++)
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
        Cbox.size = new Vector3(TileSizeRow * prefabSize, sizeY, sizeZ);

        //�ݶ��̴� ��ġ ����
        //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
        Cbox.center = new Vector3(((TileSizeRow * prefabSize / 2) - (prefabSize / 2)),
             sizeY / (prefabSize / 2),
             -(prefabSize / 2) - (sizeZ / 2));

        //((TileSizeX * prefabSize / 2) - (prefabSize / 2))
        //�ٽ� ����ϱ� ���� �ʱ�ȭ
        TileSizeRow = 1;
    }
    #endregion
}
