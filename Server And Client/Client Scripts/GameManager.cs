using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public static int Seed = 1234;
    

    public int _id = -1;
    bool _recvSaltFlag = false;
    bool _checkedValidID = false;

    string _salt;

    ServerSession _session = new ServerSession();
    Queue<Action> _jobQueue = new Queue<Action>();

    object _jobQueueLock = new object();
    CancellationTokenSource _cToken = new CancellationTokenSource();

    public MyBubbleDragon _myDragon;



    [SerializeField]
    TMP_InputField idField;
    [SerializeField]
    TMP_InputField pwField;

    [SerializeField]
    TextMeshProUGUI notificationText;

    [SerializeField]
    Canvas buttonCanvas;
    [SerializeField]
    Canvas loginCanvas;

    [SerializeField]
    TMP_InputField _IPinputField;
    [SerializeField]
    Button _enterButton;
    [SerializeField]
    TMP_InputField _IDinputField;
    [SerializeField]
    TMP_InputField _PWinputField;
    [SerializeField]
    Button _LoginButton;
    [SerializeField]
    Button _RegisterButton;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;


        DontDestroyOnLoad(this);
    }

    public void SetResolution()
    {
        //int setWidth = 1920; // 사용자 설정 너비
        //int setHeight = 1080; // 사용자 설정 높이
        //
        //int deviceWidth = Screen.width; // 기기 너비 저장
        //int deviceHeight = Screen.height; // 기기 높이 저장
        //
        //Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);
        //
        //if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)
        //{
        //    float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
        //    Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        //}
        //else
        //{
        //    float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);
        //    Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        //}

        Screen.SetResolution(800, 600, false);

    }
    void Start()
    {
        SetResolution();
        IPAddress ipAddress = IPAddress.Parse("172.30.1.8");
        IPEndPoint endPoint = new IPEndPoint(ipAddress, 8888);


        Connector connector = new Connector();

        connector.Connect(endPoint, _session, _cToken.Token);
    }


    public void PushJobQueue(Action job)
    {



        lock (_jobQueueLock)
        {
            _jobQueue.Enqueue(job);


        }


    }

    private void Flush()
    {
        while (true)
        {
            Action action = Pop();
            if (action == null)
                return;

            action.Invoke();
        }
    }

    private Action Pop()
    {
        lock (_jobQueueLock)
        {
            if (_jobQueue.Count == 0)
            {

                return null;
            }
            return _jobQueue.Dequeue();
        }
    }

    void OnApplicationQuit()
    {
        _cToken.Cancel();

        _session.Disconnect();
        //_session._recvArgs = 


    }
    public static void PrintLog(string str)
    {
        Debug.Log(str);
    }
    public void Send(ClientPacket packet)
    {
        PrintLog(packet.ID + "보냄");
        _session.Send(packet.Write());
    }

    public void InstantiateMyDragon()
    {

    }




    void Update()
    {

        while (_jobQueue.Count > 0)
        {
            Flush();
        }


    }


    public void StartGame()
    {


        MonsterSpawner.Instance.StartStage();
    }

    
    //public void Login()
    //{
    //    loginCanvas.gameObject.SetActive(false);
    //    buttonCanvas.gameObject.SetActive(true);
    //}
    public void SetSalt(string salt)
    {
        _salt = salt;
        _recvSaltFlag = true;
    }

    public void SetCheckedValidID()
    {
        _checkedValidID = true;
    }

    public void Login()
    {
        if(_id == -1)
        {
            PrintLog("id를 받지 않았음");
            return;
        }


        C_RequestLogin packet = new C_RequestLogin();

        packet.id = _id;
        packet.nameStr = idField.text;
        packet.strLength = (ushort)packet.nameStr.Length;

        _session.Send(packet.Write());


        SceneManager.LoadScene("Main");

        //StartCoroutine(LoginCoroutine());
    }

    //IEnumerator LoginCoroutine()
    //{
    //
    //
    //
    //
    //    C_RequestLogin packet = new C_RequestLogin();
    //
    //    packet.name = _IDinputField.text;
    //    packet.nameLen = (ushort)packet.name.Length;
    //
    //    //입력한 이름으로 서버에 salt 요청
    //    C_RequestSalt requestSalt = new C_RequestSalt();
    //
    //    requestSalt.name = _IDinputField.text;
    //    requestSalt.nameLen = (ushort)requestSalt.name.Length;
    //    _session.Send(requestSalt.Write());
    //
    //
    //    yield return new WaitUntil(() => _recvSaltFlag);
    //
    //    _recvSaltFlag = false;
    //
    //    if (packet.name.Length == 0)
    //    {
    //        yield break;
    //    }
    //
    //    packet.password = Cryptography.HashPasswordWithSalt(_PWinputField.text,    _salt);
    //    Debug.Log(packet.password);
    //    packet.passwordLen = (ushort)packet.password.Length;
    //
    //
    //
    //    _session.Send(packet.Write());
    //
    //}

    public void Register()
    {
        //StartCoroutine(RegisterCoroutine());
    }

    //IEnumerator RegisterCoroutine()
    //{
    //
    //
    //
    //    C_CheckAvailableID checkPkt = new C_CheckAvailableID();
    //    checkPkt.name = _IDinputField.text;
    //    checkPkt.nameLen = (ushort)checkPkt.name.Length;
    //
    //    _session.Send(checkPkt.Write());
    //
    //
    //
    //
    //    yield return new WaitUntil(() => _checkedValidID);
    //
    //    if (!_recvSaltFlag)
    //    {
    //        _notificationText.text = "이미 존재하는 아이디 입니다.";
    //        yield break;
    //    }
    //
    //    _recvSaltFlag = false;
    //
    //    C_RequestRegister packet = new C_RequestRegister();
    //
    //    packet.name = _IDinputField.text;
    //    packet.nameLen = (ushort)packet.name.Length;
    //
    //
    //    packet.password = Cryptography.HashPasswordWithSalt(_PWinputField.text, //_salt);
    //    packet.passwordLen = (ushort)packet.password.Length;
    //
    //    Debug.Log(packet.password);
    //    _session.Send(packet.Write());
    //}
    public void SelectMagician()
    {
        SceneManager.LoadScene("Main");

        C_SelectRole rolePkt = new C_SelectRole();
        rolePkt.id = _id;

        Send(rolePkt);

    }
    public void SelectHealer()
    {
        SceneManager.LoadScene("Main");


        C_SelectRole rolePkt = new C_SelectRole();
        rolePkt.id = _id;

        Send(rolePkt);
    }
}
