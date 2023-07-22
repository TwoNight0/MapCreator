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
    //subPosition
    public float x,y,z;

    public float rotation;

    // name(0) , �̱ۿ��� ���
    public string prefabName = "";

    //��Ƽ���� ���
    // ����
    public int row;

    // ����
    public int col;

}

public class MapCreate : MonoBehaviour
{

    [Header("������ ����Ʈ")] //�����ո���Ʈ
    [SerializeField, Tooltip("�ٴڿ� ���� ������")] private List<GameObject> objPrefabList_Floor;
    [SerializeField, Tooltip("���� ���� ������")] private List<GameObject> objPrefabList_Wall;
    [SerializeField, Tooltip("���ڿ� ���� ������")] private List<GameObject> objPrefabList_Deco; 
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

    private void Awake()
    {
        initData();
    }

    #region testing
    private void test()
    {
        //���̽� ���� ����
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

    private void testInstance()
    {
        int findIndex = FindObjectInList(objPrefabList_Deco, "01_DiningHall_01_WoodenChear#1");
        GameObject obj = Instantiate(objPrefabList_Deco[findIndex]);
        obj.name = "�߉糪";
    }

    #endregion

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

    }
    
    public void BtnSaveMap()
    {
        SaveObj(floor);
        SaveObj(wall);
        SaveObj(deco);
    }

    #region Save
    /// <summary>
    /// 1. ����� subOOO �� ��� ã�� �� ���� MapData���� ��� ����Ʈ�� ����
    /// 2. ����� ����Ʈ�� ���̽�ȭ
    /// 3. ���Ϸ� ����
    /// </summary>
    private void SaveObj(GameObject _nodeName)
    {
        // _nodeName == floor��
        // sub�� ���� 
        int subCount = _nodeName.transform.childCount;
        //subObj���� ����ִ� ����Ʈ

        if (subCount > 0) //sub�ǰ����� 1���̻��϶� �۵�
        {
            //MapData���� �ӽñ׸� ���̽�ȭ �ؾ���
            List<MapData> tempList = new List<MapData>();

            //floor ���� ������ ���� ����Ʈ ���� ���̽����� ��ȯ����
            for (int i = 0; i < subCount; i++)
            {
                //subobj�� ������
                GameObject subObject = _nodeName.transform.GetChild(i).gameObject;

                //���� ���� ����
                MapData data = new MapData();

                //sub ������ ����
                data.x = subObject.transform.position.x;
                data.y = subObject.transform.position.y;
                data.z = subObject.transform.position.z;

                data.rotation = subObject.transform.eulerAngles.y;

                //���� ���� ����(�ݶ��̴� ũ�� ���
                //�ݶ��̴� ��������
                BoxCollider collider = subObject.GetComponent<BoxCollider>();

                if(collider != null)
                {
                    //4�� �����յ��� �⺻�������� ���� ��Ȯ�ϰ��Ϸ��� list�� render�� �����ͼ� ����� �����ؾ���
                    //����
                    data.row = (int)(collider.size.x / 4);
                    //����
                    data.col = (int)(collider.size.z / 4);
                }

                //�̸�  ����(�̱�)
                data.prefabName = subObject.transform.GetChild(0).name;

                //����Ʈ�� �ֱ�
                tempList.Add(data);
            }

            //List<MapData> templist ���̽�ȭ
            string tmp = JsonConvert.SerializeObject(tempList);

            //���� ����
            File.WriteAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json", tmp);
            MngLog.Instance.addLog(_nodeName + "�� ����Ǿ����ϴ�");
        }
        else //����� sub��尡 ������ �ƹ��͵� ���� ���ڿ��� ����� �����͸� �ʱ�ȭ��
        {
            string str = null;
            File.WriteAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json", str);
            MngLog.Instance.addLog(_nodeName + "������ ������Ʈ�� �����ϴ�");
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
    /// ��Ƽ ������Ʈ ������ �� ����
    /// </summary>
    private void LoadMulti_Random(GameObject _nodeName, string _name, List<GameObject> _PrefabList) 
    {
        //3���� ������ ���ؾ���
        string JsonStr = File.ReadAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json");

        //List<MapData>
        List<MapData> LoadListMapData = JsonConvert.DeserializeObject<List<MapData>>(JsonStr);
        MngLog.Instance.addLog(_nodeName.name + ": ���̽� �ҷ����� �Ϸ�");

        //LOL�� ũ�Ⱑ 1���̻��϶� ����
        if (LoadListMapData != null && LoadListMapData.Count > 0)
        {
            //List<MapData>�� ������ŭ ��
            for (int i = 0; i < LoadListMapData.Count; i++)//subObj�� ������ŭ�� �ϰ�����
            {
                //col�� 0���� ŭ���� �ٴ�Ÿ��
                if(LoadListMapData[i].col > 0)
                {
                    LoadCreateFloorTile(LoadListMapData[i].row, LoadListMapData[i].col, LoadListMapData[i].x, LoadListMapData[i].y, LoadListMapData[i].z, LoadListMapData[i].rotation);
                }
                else // 0���� �������� ��Ÿ��
                {
                    LoadCreateWallTile(LoadListMapData[i].row, LoadListMapData[i].x, LoadListMapData[i].y, LoadListMapData[i].z, LoadListMapData[i].rotation);
                }
            }
        }  
        else
        {
            Debug.Log(_name + ": �ҷ��� ������ �����ϴ�");
            MngLog.Instance.addLog(_name + ": �ҷ��� ������ �����ϴ�");
        }
      
    }

    /// <summary>
    /// �̱� ������Ʈ �ҷ����� �� ����
    /// </summary>
    private void LoadSingle(GameObject _nodeName, string _name, List<GameObject> _PrefabList)
    {
        //3���� ������ ���ؾ���
        string JsonStr = File.ReadAllText(Application.dataPath + "/MapJsonFolder/" + _nodeName + ".json");

        //List<MapData>
        List<MapData> LoadListMapData = JsonConvert.DeserializeObject<List<MapData>>(JsonStr);
        MngLog.Instance.addLog(_nodeName.name + ": ���̽� �ҷ����� �Ϸ�");

        //LOL�� ũ�Ⱑ 1���̻��϶� ����
        if (LoadListMapData != null && LoadListMapData.Count > 0)
        {
            //List<MapData>�� ������ŭ ��
            for (int i = 0; i < LoadListMapData.Count; i++)//subObj�� ������ŭ�� �ϰ�����
            {
                //Floor -> ���� ���(this) -> 111
                GameObject subDeco = new GameObject();
                subDeco.transform.parent = deco.transform;
                subDeco.name = "subDeco";

                int foundIndex = -1;

                //(clone)���� ����
                string findString = LoadListMapData[i].prefabName.Substring(0, LoadListMapData[i].prefabName.Length - 7);
                for (int j = 0; j < _PrefabList.Count; j++)
                {
                    if(_PrefabList[j].name == findString)
                    {
                        foundIndex = j; // ��ġ�ϴ� �������� �ε����� �����ϰ�
                        break; // �� �̻� �˻��� �ʿ� �����Ƿ� �ݺ��� ����
                    }
                }

                if (foundIndex != -1)
                {
                    // ��ġ�ϴ� �������� �ε����� ����Ͽ� ���ϴ� �۾��� ����
                    GameObject obj = Instantiate(_PrefabList[foundIndex], subDeco.transform);
                    obj.name = _PrefabList[foundIndex].name;
                }
                else
                {
                    // ��ġ�ϴ� �������� ���� ���
                    Debug.Log("��ġ�ϴ� �������� ����");
                    MngLog.Instance.addLog("��ġ�ϴ� �������� ���� �������� ���߽��ϴ�");
                }

                //sub ��ġ ����
                subDeco.transform.position = new Vector3(LoadListMapData[i].x, LoadListMapData[i].y, LoadListMapData[i].z);

                //sub rotation ����
                //Debug.Log(LoadListMapData[i].rotation);
                subDeco.transform.rotation = Quaternion.Euler(0f, LoadListMapData[i].rotation, 0f);

            }
        }
        else
        {
            Debug.Log(_name + ": �ҷ��� ������ �����ϴ�");
            MngLog.Instance.addLog(_name + ": �ҷ��� ������ �����ϴ�");
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
    /// �Է��� ���� x ���� ��ŭ Ÿ���� ������ִ� �޼���
    /// </summary>
    public void CreateFloorTile()
    {
        //InputField�� ��������
        string tmp = InputRow.text;
        string tmp2 = InputColumn.text;

        //�ӽ� int
        int resultIntX = 1;
        int resultIntZ = 1;

        int.TryParse(tmp, out resultIntX);
        int.TryParse(tmp2, out resultIntZ);

        //��Ȯ��
        if(resultIntX > 0 && resultIntZ > 0)
        {
            TileSizeRow = resultIntZ;
            TileSizecolumn = resultIntX;
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
     
            obj.name = obj.name.ToString();
   

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

        //Log
        MngLog.Instance.addLog(resultIntX + " x "  + resultIntZ + " ũ���� floor��  �����߽��ϴ�." );

    }

    /// <summary>
    /// �Է��� ���ο����� Ÿ���� ������ִ� �޼���
    /// </summary>
    public void CreateWallTile()
    {
        //�������� �����ϴ� ũ�⸸ŭ �������� �Űܾ��ϱ⶧���� ������ ����� �����ϱ�����
        MeshRenderer ren = objPrefabList_Wall[0].GetComponent<MeshRenderer>();
        prefabSize = (int)ren.bounds.size.x;
        float sizeY = ren.bounds.size.y;
        float sizeZ = ren.bounds.size.z;

        //InputField�� ��������
        string tmp = InputRow.text;

        //�ӽ� int
        int resultIntX = 1;

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
            obj.name = obj.name.ToString();

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

        //Log
        MngLog.Instance.addLog(resultIntX + " x 1 ũ���� wall��  �����߽��ϴ�.");
    }

    public void LoadCreateFloorTile(int row, int col , float x, float y, float z, float rotation)
    {
        //�������� �����ϴ� ũ�⸸ŭ �������� �Űܾ��ϱ⶧���� ������ ����� �����ϱ�����
        MeshRenderer ren = objPrefabList_Floor[0].GetComponent<MeshRenderer>();
        prefabSize = (int)ren.bounds.size.x;

        //for���ϳ��� z���� �÷��ֱ�����
        tempZ = 0;

        //�ѱ�ƾ��ϴ� ������ ��
        totalPixel = row * col;

        //Floor -> ���� ���(this) -> 111
        GameObject subFloor = new GameObject();
        subFloor.transform.parent = floor.transform;
        subFloor.name = "subFloor";

        for (int i = 0; i < totalPixel; i++)
        {
            //z������ �ø�������
            if (i % col == 0)
            {
                tempZ += prefabSize;
            }

            //position ����
            Vector3 Vfloor = new Vector3(i * prefabSize - ((tempZ - prefabSize) * col), 0, tempZ - prefabSize);
            //Debug.Log(Vfloor);

            //������Ʈ����
            GameObject obj = Instantiate(objPrefabList_Floor[Random.Range(0, objPrefabList_Floor.Count)], subFloor.transform);

            //��ġ����
            obj.transform.position = Vfloor;

            //������Ʈ����, �̸��Ҵ�

            obj.name = obj.name.ToString();

            //�±� ����
            obj.tag = "Ground";

            //���̾� ����
            obj.layer = 6;
        }

        //sub position ����
        subFloor.transform.position = new Vector3(x, y, z);

        //sub rotation ����
        subFloor.transform.localEulerAngles = new Vector3(0f, rotation, 0f);

        //�ݶ��̴� �����
        subFloor.AddComponent<BoxCollider>();

        //�ݶ��̴� ��������
        BoxCollider Cbox = subFloor.GetComponent<BoxCollider>();

        //�ݶ��̴� ������ ����
        Cbox.size = new Vector3(col * prefabSize, 0, row * prefabSize);

        //�ݶ��̴� ��ġ ����
        //Cbox.transform.position = new Vector3(((TileSizeZ * prefabSize /2) -2), 0, TileSizeX * prefabSize);
        Cbox.center = new Vector3(((col * prefabSize / 2) - (prefabSize / 2)),
            0,
            ((row * prefabSize / 2) - (prefabSize / 2)));

        //Log
        MngLog.Instance.addLog(row + " x " + col + " ũ���� floor��  �����߽��ϴ�.");

    }

    public void LoadCreateWallTile(int row, float x, float y, float z, float rotation)
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

        //���� ������Ʈ ����
        for (int i = 0; i < row; i++)
        {
            //position ����
            Vector3 Vwall = new Vector3(i * prefabSize, 0, 0);
            Debug.Log(i +" :"+ Vwall);
            
            //������Ʈ����(����)
            GameObject obj = Instantiate(objPrefabList_Wall[Random.Range(0, objPrefabList_Wall.Count)], subWall.transform);

            //obj ��ġ����
            obj.transform.position = Vwall;

            //������Ʈ���� 
            obj.name = obj.name.ToString();

            //�±� ����
            obj.tag = "Wall";

            //���̾� ����
            obj.layer = 7;
        }

        //sub position ����
        subWall.transform.position = new Vector3(x, y, z);

        //sub rotation ����
        subWall.transform.localEulerAngles = new Vector3(0f, rotation, 0f);

        //�ݶ��̴� �����
        subWall.AddComponent<BoxCollider>();

        //�ݶ��̴� ��������
        BoxCollider Cbox = subWall.GetComponent<BoxCollider>();

        //�ݶ��̴� ������ ����
        Cbox.size = new Vector3(row * prefabSize, sizeY * 2.3f, sizeZ);

        //�ݶ��̴� ��ġ ����
        Cbox.center = new Vector3(((row * prefabSize / 2) - (prefabSize / 2)),
             (sizeY / prefabSize) + 2,
             -(prefabSize / 2) - (sizeZ / 2));

        //Log
        MngLog.Instance.addLog( row + " x 1 ũ���� wall��  �����߽��ϴ�.");
    }

    #endregion


}
