using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager _instance;
    public static UIManager Instance { get { return _instance; }  }

    [SerializeField]
    TextMeshProUGUI scoreText;

    int _score;
    public int Score { get { return _score; } set { _score = value;  RefreshUI(); }  }

    Dictionary<int , TextMeshProUGUI> _speechBubbleDic = new Dictionary<int, TextMeshProUGUI> ();
    Dictionary<int , Coroutine> _speechBubbleCoroutineDic = new Dictionary<int, Coroutine> ();

    [SerializeField]
    TMP_InputField _inputField;
    // Start is called before the first frame update
    Coroutine hideCoroutine;

    [SerializeField]
    TextMeshProUGUI middleText;
    private void Awake()
    {
        _instance = this; 
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
          
            
    }

    IEnumerator HideSpeechBubble(int id)
    {
        yield return new WaitForSeconds(2f);

        _speechBubbleDic[id].text = "";

    }
    public void OnClickSendButton()
    {
        //_speechBubbleDic[GameManager.Instance._id].text = _inputField.text;
        

        C_SendChat sendPkt = new C_SendChat ();

        sendPkt.id = GameManager.Instance._id;
        sendPkt.chatStr = _inputField.text;

       




        GameManager.Instance.Send(sendPkt);


        _inputField.text = "";

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideSpeechBubble(GameManager.Instance._id));

    }

    public void ShowSpeechBubbleText(int id, string text)
    {
        _speechBubbleDic[id].text = text;

        _speechBubbleCoroutineDic.TryGetValue(id, out Coroutine coroutine);

        if(coroutine == null)
        {
            Coroutine newCoroutine = StartCoroutine(HideSpeechBubble(id));

            _speechBubbleCoroutineDic.Add(id, newCoroutine);
        }
        else
        {
            StopCoroutine(coroutine);

            StartCoroutine(HideSpeechBubble(id));
        }

    }

    public void ShowMiddleText(string text)
    {
        middleText.text = text;
    }
    void RefreshUI()
    {
        scoreText.text = "Score : " + _score.ToString();
    }

    public void AddSpeechBubbleDic(int id, TextMeshProUGUI textUGUI)
    {
        _speechBubbleDic.Add(id , textUGUI);

    }
}
