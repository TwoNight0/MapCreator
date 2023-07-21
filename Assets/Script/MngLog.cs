using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MngLog : MonoBehaviour
{
    static public MngLog Instance;

    public GameObject logParent;
    public TMP_FontAsset NotoSansFont;

    public int MaxLog;

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
        test();
        test();
        Deletelog();
    }

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
    }

    public void test()
    {
        GameObject log = new GameObject();
        log.name = "log";
        log.transform.parent = logParent.transform;

        log.AddComponent<TextMeshProUGUI>();

        TextMeshProUGUI logText = log.GetComponent<TextMeshProUGUI>();
        logText.font = NotoSansFont;

        logText.text = "한글되나?";

        
        
    }

    public void Deletelog()
    {
        if(logParent.transform.childCount > 0)
        {
            Destroy(logParent.transform.GetChild(0).gameObject);
        }


    }
    


}
