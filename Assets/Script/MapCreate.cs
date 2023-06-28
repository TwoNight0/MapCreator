using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class MapData{
    
    [SerializeField] int sizeX;
    [SerializeField] int sizeZ;
    Vector3 position;

    private int LayerNum;
    private string tagName;
    
    public void printData()
    {
        Debug.Log("Level : "  );
        Debug.Log("Position : " );
    }
}


public class MapCreate : MonoBehaviour
{
    [SerializeField] private List<GameObject> objList_Floor;
    [SerializeField] private List<GameObject> objList_Wall;

    private void Awake(){
        //�ֻ��� ������Ʈ
        GameObject map = new GameObject();

        //���� ������Ʈ��(�θ�)
        GameObject floor = new GameObject();
        GameObject wall = new GameObject();

        map.transform.position = Vector3.zero;
        floor.transform.position = Vector3.zero;
        wall.transform.position = Vector3.zero;
    }

    //������Ʈ(ũ��,��ġ,���̾�,�ױ�)�� Ư�� �θ���ؼ� �ν��Ͻÿ���Ʈ, -> ��ġ����


    // Start is called before the first frame update
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SaveMap(MapData _map){
        File.WriteAllText(Application.dataPath + "/TestJson.json", JsonUtility.ToJson(_map));
    }

    private MapData LoadMap() {
        string str = File.ReadAllText(Application.dataPath + "/TestJson.json");
        MapData result = JsonUtility.FromJson<MapData>(str);
        return result; 
    }

    private void createRow(int size, GameObject _parent)
    {
        for(int i = 0; i<size; i++)
        {

        }
    }

    private void createColumn(int size, GameObject _parent)
    {

    }


}
