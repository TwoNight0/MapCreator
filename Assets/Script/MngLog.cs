using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MngLog : MonoBehaviour
{
    static public MngLog Instance;

    public GameObject logParent;
    public TMP_FontAsset NotoSansFont; 

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
    }

    public void addLog(string _systemMessage)
    {
        GameObject log = new GameObject();
        log.name = "log";
        log.transform.parent = logParent.transform;
        
        log.AddComponent<TextMeshProUGUI>();

        TextMeshProUGUI logText = log.GetComponent<TextMeshProUGUI>();
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
        // Apply the changes
        logText.UpdateFontAsset();
        logText.ForceMeshUpdate();

        logText.text = "한글되나?";

        
        
    }
    


}
