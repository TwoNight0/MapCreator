using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using System.Text; //stringBuilder��
using UnityEditor;

/// <summary>
/// ��ġ������ ���̾�,�±� ������ �������־ ���̽������͸� �������� �ٸ� ���������ε� ���������� ���� ���������
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

        Debug.Log("�ʵ����� ����Ʈ 1 : " + str1);
        Debug.Log("�ʵ����� ����Ʈ 2 : " + str2);

        JsonStrings.Add(str1);
        JsonStrings.Add(str2);

        string js = JsonConvert.SerializeObject(JsonStrings);

        List<string> deserailList = JsonConvert.DeserializeObject<List<string>>(js);

        Debug.Log("���̽���Ʈ������Ʈ : " + JsonStrings);
        Debug.Log("���̽���Ʈ������Ʈ : " + JsonStrings[0]);

        //tempList ���
        List<MapData> DeSerialList = JsonConvert.DeserializeObject<List < MapData >> (deserailList[0]);

        Debug.Log("DSL: "+ DeSerialList);
        Debug.Log("DSLx: "+ DeSerialList[0].x);
        Debug.Log("DSLy: "+ DeSerialList[0].y);
        Debug.Log("DSLz: "+ DeSerialList[0].z);

        //AssetPreview.GetAssetPreview();
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

        //Ÿ�� �ʱ�ȭ
        TileSizeRow = 1;
        TileSizecolumn = 1;

        LoadLOLstr = new List<string>();


    }

    /// <summary>
    /// 1. ����� subOOO �� ��� ã�� �� ���� MapData���� ��� ����Ʈ�� ����
    /// 2. ����� ����Ʈ�� ���̽�ȭ
    /// 3. ���Ϸ� ����
    /// </summary>
    public void BtnSaveMap()
    {
        SaveObj(floor, LoadLOLstr);
        SaveObj(wall, LoadLOLstr);
        SaveObj(deco, LoadLOLstr);
    }

    #region Save
    /// <summary>
    /// ���x ���� ���̺� ����
    /// �����Ҷ� ������Ʈ���� �������� �� ������Ʈ�� �������� ���� mapdata �����͸� ����� 
    /// ��� ����Ʈ�� �ְ� �� ����Ʈ�� ����!
    /// ������ : ���������Ʈ �ϳ��� ��� ������Ʈ�� �� �����־ �κк��� �����̱Ⱑ ���� ����
    /// </summary>
    /// <param name="_nodeName">ū ���</param>
    private void SaveObj(GameObject _nodeName)
    {
        // _nodeName == floor��
        // sub�� ���� 
        int subCount = _nodeName.transform.childCount;
        //subObj���� ����ִ� ����Ʈ

        if (subCount > 0) //sub�ǰ����� 1���̻��϶� �۵�
        {
            //floor ���� ������ ���� ����Ʈ ���� ���̽����� ��ȯ����
            List<MapData> tempList = new List<MapData>();

            for (int i = 0; i < subCount; i++)
            {
                //subobj�� ������
                Transform subTransform = _nodeName.transform.GetChild(i); 
                GameObject subObject = subTransform.gameObject;

                int objectCount = subObject.transform.childCount;

                if(objectCount > 0)
                {
                    for (int k = 0; k < objectCount; k++)
                    {
                        Transform objTransform = subObject.transform.GetChild(k);
                        GameObject Object = objTransform.gameObject;

                        //���� ������ ����
                        MapData data = new MapData();

                        data.x = Object.transform.position.x;
                        data.y = Object.transform.position.y;
                        data.z = Object.transform.position.z;

                        data.tagName = Object.transform.tag;
                        data.LayerNum = Object.layer;

                        //���̽� ��ȯ�� ����Ʈ�� ���
                        tempList.Add(data);

                    }
                }
            }

            //���̽� ����
            string str = JsonConvert.SerializeObject(tempList);

            //���� ����
            File.WriteAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json", str);
        }
    }

    //overloading
    /// <summary>
    /// subObject�� �����ؼ� ����� �ְ� �� �ؿ� �����͸� ����
    /// </summary>
    /// <param name="_nodeName"></param>
    private void SaveObj(GameObject _nodeName, List<string> _ListOfListStr)
    {
        // _nodeName == floor��
        // sub�� ���� 
        int subCount = _nodeName.transform.childCount;
        //subObj���� ����ִ� ����Ʈ

        if (subCount > 0) //sub�ǰ����� 1���̻��϶� �۵�
        {
            //floor ���� ������ ���� ����Ʈ ���� ���̽����� ��ȯ����

            for (int i = 0; i < subCount; i++)
            {
                //MapData���� �ӽñ׸� ���̽�ȭ �ؾ���
                List<MapData> tempList = new List<MapData>(); 

                //subobj�� ������
                Transform subTransform = _nodeName.transform.GetChild(i);
                GameObject subObject = subTransform.gameObject;

                int objectCount = subObject.transform.childCount;

                //floor ���� ����
                if (objectCount > 0)
                {
                    for (int k = 0; k < objectCount; k++)
                    {
                        Transform objTransform = subObject.transform.GetChild(k);
                        GameObject Object = objTransform.gameObject;

                        //���� ������ ����
                        MapData data = new MapData();

                        data.x = Object.transform.position.x;
                        data.y = Object.transform.position.y;
                        data.z = Object.transform.position.z;

                        data.tagName = Object.transform.tag;
                        data.LayerNum = Object.layer;

                        //���̽� ��ȯ�� ����Ʈ�� ���
                        tempList.Add(data);

                    }
                }
                string tmp = JsonConvert.SerializeObject(tempList);

                //���̽� ���ڿ��� ��� string tmp�� ����Ʈ�� ������
                _ListOfListStr.Add(tmp);
            }

            //���̽� ���ڿ��� ��� List str�� ���� ���̽� ����
            string str = JsonConvert.SerializeObject(_ListOfListStr);
            //���� ����
            File.WriteAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json", str);
        }
        else //�ƹ��͵������� �����ϸ� ���̽��� �ʱ�ȭ��
        {
            string str = null;
            File.WriteAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json", str);
        }
    }
    #endregion

    /// <summary>
    /// ����Ʈ�� �߰� �� MapData ������ �Ҵ�
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
    /// �ش��ο� �ش��ϴ� ����Ʈ�� ��ȯ��(������ �󸮽�Ʈ�� ��ȯ)
    /// 0. ��ŸƮ�� �ϴ� �ѹ� ����
    /// 1. ���̽������� ���ϸ� ������ ������ȭ
    /// 2. ����Ʈ�� �ٽ� �������� 
    /// 3. ����Ʈ�� ���̸�ŭ �ݺ����� ���鼭 ��ġ�� ���� ������Ʈ�� ����
    /// 4. �±׿� ���̾� ����
    /// </summary>
    /// <returns></returns>
    private void LoadMapRandom(GameObject _nodeName, string _name, List<GameObject> _PrefabList)
    { 
        //3���� ������ ���ؾ���
        string str = File.ReadAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json");

        LoadLOLstr = JsonConvert.DeserializeObject<List<string>>(str);
        //LoadLOL = JsonUtility.FromJson<List<List<MapData>>>(str);


        LoadMapCreateRandom(_nodeName, _name, LoadLOLstr, _PrefabList);
    }

    private void LoadMap(GameObject _nodeName, string _name, List<GameObject> _PrefabList)
    {
        //3���� ������ ���ؾ���
        string str = File.ReadAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json");

        LoadLOLstr = JsonConvert.DeserializeObject<List<string>>(str);
        //LoadLOL = JsonUtility.FromJson<List<List<MapData>>>(str);


        LoadMapCreate(_nodeName, _name, LoadLOLstr, _PrefabList);
    }

    /// <summary>
    /// ����Ʈ�� �������� ������Ʈ�� ����
    /// </summary>
    private void LoadMapCreateRandom(GameObject _nodeName, string _name, List<string> _LoadLOLstr, List<GameObject> _PrefabList) 
    {
        //LOL�� ũ�Ⱑ 1���̻��϶� ����
        if (_LoadLOLstr != null && _LoadLOLstr.Count > 0)
        {
            //����Ʈ�� ���̸�ŭ �ϳ��ϳ��� ��������
            for (int i = 0; i < _LoadLOLstr.Count; i++)//subObj�� ������ŭ�� �ϰ�����
            {
                //subObj ���� �� �̸� �Ҵ�
                GameObject subObj = new GameObject(_name);

                //�θ� ������ֱ� floor�� subObj�� �θ�� 
                subObj.transform.parent = _nodeName.transform;
                //������ �κ� (���̽��� �ٽ� Ǯ����)
                List<MapData> tmp = JsonConvert.DeserializeObject<List<MapData>>(_LoadLOLstr[i]);

                for (int k = 0; k < tmp.Count; k++)//sub�� �ڽİ��� ��ŭ �ݺ��� ���� ��
                {
                    //������Ʈ ���� �� �θ�!����
                    GameObject obj = Instantiate(_PrefabList[Random.Range(0, _PrefabList.Count)], subObj.transform);

                    //pos ����
                    obj.transform.position = new Vector3(tmp[k].x, tmp[k].y, tmp[k].z);

                    //tag �±�
                    obj.transform.tag = tmp[k].tagName;

                    //layer ���̾�
                    obj.layer = tmp[k].LayerNum;
                }
                //�ݶ��̴� �����
                subObj.AddComponent<BoxCollider>();

                //�ݶ��̴� ��������
                BoxCollider Cbox = subObj.GetComponent<BoxCollider>();

                //�ݶ��̴� ������ ����
                Cbox.size = new Vector3(TileSizecolumn * prefabSize, 0, TileSizeRow * prefabSize);

                //�ݶ��̴� ��ġ ����
                //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
                Cbox.center = new Vector3(((TileSizecolumn * prefabSize / 2) - (prefabSize / 2)),
                    0,
                    ((TileSizeRow * prefabSize / 2) - (prefabSize / 2)));
            }
        }  
        else
        {
            Debug.Log(_name + ": " + "�ҷ��� ������ �����ϴ�");
        }
      
    }

    private void LoadMapCreate(GameObject _nodeName, string _name, List<string> _LoadLOLstr, List<GameObject> _PrefabList)
    {
        //LOL�� ũ�Ⱑ 1���̻��϶� ����
        if (_LoadLOLstr != null && _LoadLOLstr.Count > 0)
        {
            //����Ʈ�� ���̸�ŭ �ϳ��ϳ��� ��������
            for (int i = 0; i < _LoadLOLstr.Count; i++)//subObj�� ������ŭ�� �ϰ�����
            {
                //subObj ���� �� �̸� �Ҵ�
                GameObject subObj = new GameObject(_name);

                //�θ� ������ֱ� floor�� subObj�� �θ�� 
                subObj.transform.parent = _nodeName.transform;
                //������ �κ� (���̽��� �ٽ� Ǯ����)
                List<MapData> tmp = JsonConvert.DeserializeObject<List<MapData>>(_LoadLOLstr[i]);

                for (int k = 0; k < tmp.Count; k++)//sub�� �ڽİ��� ��ŭ �ݺ��� ���� ��
                {
                    //������Ʈ ���� �� �θ�!���� �̺κ� �ٲ����
                    GameObject obj = Instantiate(_PrefabList[Random.Range(0, _PrefabList.Count)], subObj.transform);

                    //pos ����
                    obj.transform.position = new Vector3(tmp[k].x, tmp[k].y, tmp[k].z);

                    //tag �±�
                    obj.transform.tag = tmp[k].tagName;

                    //layer ���̾�
                    obj.layer = tmp[k].LayerNum;
                }
                //�ݶ��̴� �����
                subObj.AddComponent<BoxCollider>();

                //�ݶ��̴� ��������
                BoxCollider Cbox = subObj.GetComponent<BoxCollider>();

                //�ݶ��̴� ������ ����
                Cbox.size = new Vector3(TileSizecolumn * prefabSize, 0, TileSizeRow * prefabSize);

                //�ݶ��̴� ��ġ ����
                //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
                Cbox.center = new Vector3(((TileSizecolumn * prefabSize / 2) - (prefabSize / 2)),
                    0,
                    ((TileSizeRow * prefabSize / 2) - (prefabSize / 2)));
            }
            

        }
        else
        {
            Debug.Log(_name + ": " + "�ҷ��� ������ �����ϴ�");
        }

    }
    #endregion

    #region  create
    /// <summary>
    /// �Է��� ���� x ���� ��ŭ Ÿ���� ������ִ� �޼���
    /// </summary>
    public void BtnCreateFloorTile()
    {
        //InputField�� ��������
        string tmp = InputRow.text;
        string tmp2 = InputColumn.text;

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
            GameObject obj = Instantiate(objPrefabList_Floor[Random.Range(0, objPrefabList_Floor.Count)], subFloor.transform);

            //��ġ����
            obj.transform.position = Vfloor;

            //������Ʈ����, �̸��Ҵ�
            obj.name = "floor" + objNum.ToString();
            objNum++;

            //�±� ����
            obj.tag = "Ground";

            //���̾� ����
            obj.layer = 6;
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

        //InputField�� ��������
        string tmp = InputRow.text;

        //�ӽ� int
        int resultIntX = 0;

        int.TryParse(tmp, out resultIntX);

        //��Ȯ��
        if (resultIntX > 0)
        {
            TileSizeRow = resultIntX;
        }
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
            GameObject obj = Instantiate(objPrefabList_Wall[Random.Range(0, objPrefabList_Wall.Count)], subWall.transform);

            //obj ��ġ����
            obj.transform.position = Vwall;

            //������Ʈ���� 
            obj.name = "wall" + objNum.ToString();
            objNum++;

            //�±� ����
            obj.tag = "Wall";

            //���̾� ����
            obj.layer = 7;
        }
        //�ݶ��̴� �����
        subWall.AddComponent<BoxCollider>();

        //�ݶ��̴� ��������
        BoxCollider Cbox = subWall.GetComponent<BoxCollider>();

        //�ݶ��̴� ������ ����
        Cbox.size = new Vector3(TileSizeRow * prefabSize, sizeY * 2.3f, sizeZ);

        //�ݶ��̴� ��ġ ����
        //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
        Cbox.center = new Vector3(((TileSizeRow * prefabSize/2) - (prefabSize / 2)),
             (sizeY / prefabSize) +2,
             -(prefabSize / 2) - (sizeZ / 2));

        //((TileSizeX * prefabSize / 2) - (prefabSize / 2))
        //�ٽ� ����ϱ� ���� �ʱ�ȭ
        TileSizeRow = 1;
    }
    #endregion


}
