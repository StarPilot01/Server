using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Chat : MonoBehaviour
{
    static Chat _instance;
    public static Chat Instance {  get { return _instance; } }

    [SerializeField]
    TextMeshProUGUI chatText;

    List<string> strList = new List<string>();

    [SerializeField]
    int maxChat;
    private void Awake()
    {
        _instance = this;

    }


    public void AddChat(int id, string str)
    {
        if(maxChat < strList.Count)
        {
            strList.RemoveAt(0);
        }
        
        
        strList.Add(BubbleDragonManager.Instance.GetNickNameById(id) + " : "+ str);

        RefreshChat();
    }

    void RefreshChat()
    {
        chatText.text = "";

        for(int i = 0; i < strList.Count; i++) 
        {
            chatText.text += strList[i];
            chatText.text += '\n';
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
