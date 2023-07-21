using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MngLog : MonoBehaviour
{
    static public MngLog Instance;

    public GameObject logParent;
    public TMP_FontAsset NotoSansFont;

    private int MaxLog;
    private int countLog;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        countLog = 0;
        MaxLog = 20;


        //���� �׽�Ʈ �ڵ�
        //for(int i=0; i<25; i++)
        //{
        //    test();
        //}
        //Debug.Log(countLog);
 
    }

    /// <summary>
    /// content�� �θ�� �޽����� �߰��� 
    /// </summary>
    /// <param name="_systemMessage"></param>
    public void addLog(string _systemMessage)
    {
        GameObject log = new GameObject();
        log.name = "log";
        log.transform.parent = logParent.transform;
        
        log.AddComponent<TextMeshProUGUI>();

        TextMeshProUGUI logText = log.GetComponent<TextMeshProUGUI>();
        logText.font = NotoSansFont;
        logText.rectTransform.sizeDelta = new Vector2(1200, 50);

        
        logText.text = _systemMessage;
        countLog++;

        if(countLog > 10)
        {
            for(int i=0; i<10; i++)
            {
                Deletelog(i);
            }
            countLog = 0;
        }
    }
    public void test()
    {
        GameObject log = new GameObject();
        log.name = "log";
        log.transform.parent = logParent.transform;

        log.AddComponent<TextMeshProUGUI>();

        TextMeshProUGUI logText = log.GetComponent<TextMeshProUGUI>();
        logText.font = NotoSansFont;

        logText.text = "�ѱ۵ǳ�?";
        countLog++;

        if (countLog > MaxLog)
        {
            for (int i = 0; i < 10; i++)
            {
                Deletelog(i);
            }
        }
    }

    /// <summary>
    /// �޽��� �Ѱ� ���ֱ�
    /// </summary>
    public void Deletelog()
    {
        if(logParent.transform.childCount > 0)
        {
            Destroy(logParent.transform.GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// �����ε� �ε����� ���ֱ�
    /// </summary>
    /// <param name="index"></param>
    public void Deletelog(int index)
    {
        if (logParent.transform.childCount > 0)
        {
            Destroy(logParent.transform.GetChild(index).gameObject);
        }
    }


}
