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
    [Header("������ ����Ʈ")] //�����ո���Ʈ
    [SerializeField, Tooltip("�ٴڿ� ���� ������")] private List<GameObject> objList_Floor;
    [SerializeField, Tooltip("���� ���� ������")] private List<GameObject> objList_Wall;
    [Space(20)]

    [Header("Ÿ�� ������")] //Ÿ�� ������
    [SerializeField, Tooltip("Ÿ���� ����")] int TileSizeX;
    [SerializeField, Tooltip("Ÿ���� ����")] int TileSizeZ;

    [Tooltip("floor����Ʈ�� �ִ�")] private int max_Floor;
    [Tooltip("wall����Ʈ�� �ִ�")] private int max_Wall;



    private void Awake(){
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

        //������ ���� �ִ밪����
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

    //�����Ҷ� �̰� ����Ȱ� �ҷ�����ɵ� 
    private MapData LoadMap() {
        string str = File.ReadAllText(Application.dataPath + "/TestJson.json");
        MapData result = JsonUtility.FromJson<MapData>(str);
        return result; 
    }


    //�̿�����Ʈ�� �������� 4 ������ ������ �������� ��Ÿ�� % z == 0 z�����ؼ� ���οø���
    private void createTile(int sizeX, GameObject _parent)
    {

        for(int i = 0; i< sizeX; i++)
        {
            //������Ʈ����(������, ����(i * 4, 0 , ), �θ�) -> ��ġ, �θ�, �ذ�
            GameObject obj = Instantiate(objList_Floor[Random.Range(0, max_Floor)]);


            //���鶧���� ����Ʈ�� �߰��ؾ߰���..?
            //�̸����� ground + i
            

            //�±� ����


            //���̾� ����
        }
    }


}
