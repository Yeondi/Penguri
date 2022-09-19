#define debug
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameManager sharedInstance = null;

    private GoogleSheetManager gs;
    [SerializeField]
    private ItemStatusManager ism;

    private PenguriManager penguriManager;

    [SerializeField]
    private MoneyManager moneyManager;
    GameObject go_MoneyManager;

    [SerializeField]
    private BuffController buff;
    [SerializeField]
    private EventController eventController;


    [Header("Move")]
    public GameObject PengsPortrait;
    public bool move;
    public bool moveDown;
    Vector2 OriginPos;

    Vector2 target;
    float targetYValue = 50.0f;

    private Dictionary<string, List<string>> m_savedData;

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
            Destroy(gameObject);
        else
            sharedInstance = this;

        DontDestroyOnLoad(gameObject);
        //tsv = gameObject.AddComponent<TSVLoader>();
        //gs = gameObject.AddComponent<GoogleSheetManager>();
        Init();
        OriginPos = PengsPortrait.transform.localPosition;
        ism = gameObject.AddComponent<ItemStatusManager>();
        penguriManager = gameObject.AddComponent<PenguriManager>();
        target = new Vector2(PengsPortrait.transform.localPosition.x, PengsPortrait.transform.localPosition.y + targetYValue);
        buff = gameObject.AddComponent<BuffController>();

        moneyManager = Instantiate(moneyManager);

    }
    //���� Touch To Start �������� �̽�  Panel�� �����鼭 ��ġ�Ǿ������

    public void DEBUG_BRIDGE(int nTypeNumber)
    {
        if (nTypeNumber == 0)
            moneyManager.DEBUG_ADDMoney();
        else if (nTypeNumber == 1)
        {
            StartCoroutine(eventController.Rain());
            Debug.Log("Rain");
        }
    }

    private void Update()
    {
        //���⼭ �� ���ڿ�������� �ƿ� �ٸ��ɷ� ��ü
        //if (move)
        //{
        //    PengsPortrait.transform.localPosition = Vector2.Lerp(PengsPortrait.transform.localPosition,
        //        new Vector2(PengsPortrait.transform.localPosition.x,
        //        PengsPortrait.transform.localPosition.y + targetYValue), 1f);
            
        //}
        //if (moveDown)
        //{
        //    PengsPortrait.transform.localPosition = OriginPos;
        //    moveDown = false;
        //}
        //StartCoroutine(movingStartPage(new Vector2(go.transform.position.x,go.transform.position.y + 300.0f), 5.0f));
    }

    private void Init()
    {
        //gs.recall = true;
    }
    public void isMoving(bool bSignal)
    {
        move = move == true ? false : true;
        if (move)
            PengsPortrait.transform.localPosition = new Vector2(PengsPortrait.transform.localPosition.x,
                    PengsPortrait.transform.localPosition.y + targetYValue);
        else if(!move)
            PengsPortrait.transform.localPosition = OriginPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
    }

    public void setTsvData(Dictionary<string, List<string>> data)
    {
        m_savedData = data;
    }

    public Dictionary<string, List<string>> getTsvData()
    {
        return m_savedData;
    }

    public PenguriManager getPenguriManager()
    {
        return penguriManager;
    }

    public BuffController GetBuffController()
    {
        return buff;
    }

    public ItemStatusManager GetItemStatusManager()
    {
        return ism;
    }
}

public class PenguriManager : MonoBehaviour
{
    Dictionary<string, List<string>> dict;
    Penguri penguri;

    [SerializeField]
    string Sync_Key;
    [SerializeField]
    List<float> SynchronizedData;

#if debug
    public void DEBUG__Start() // ���߿� �Լ� �� ���¿��� ����
    {
        dict = GameManager.sharedInstance.getTsvData();
        penguri.setData("EggStage", float.Parse(dict["EggStage"][0]),
        float.Parse(dict["EggStage"][1]), float.Parse(dict["EggStage"][2]),
        float.Parse(dict["EggStage"][3]), float.Parse(dict["EggStage"][4]),
        float.Parse(dict["EggStage"][5]), float.Parse(dict["EggStage"][6]));
        penguri.setHunger(float.Parse(dict["EggStage"][0]));
        penguri.setTemperature(float.Parse(dict["EggStage"][1]));
        // ��ġ�� �µ����� 0.004
        // �ʴ� ��Ⱘ�� -1(����)
        // �ʴ� �µ� ���� 0.006
    }
#endif

    private void Start()
    {
        penguri = GameManager.sharedInstance.gameObject.GetComponent<Penguri>();
        SynchronizedData = new List<float>();
    }

    public void SyncStatus()
    {
        Sync_Key = penguri.getKey();
        SynchronizedData = penguri.SyncDataToGM();
    }

    private void FixedUpdate()
    {
        SyncStatus();

        if (SynchronizedData[0] <= 30f && Sync_Key != "EggStage" && Sync_Key != "")
        {
            GameManager.sharedInstance.GetBuffController().Starve(SynchronizedData[0]);
        }

        if (SynchronizedData[1] <= 32f && Sync_Key != "")
        {
            GameManager.sharedInstance.GetBuffController().Hypothermia(SynchronizedData[1]);
        }
    }
}


//public class PopupManager : MonoBehaviour
//{
//    public void init()
//    {

//    }

//    public void openItemPopup()
//    {

//    }

//    public void getInfo()
//    {
//        /*
//         * Item PopUp
//         * Title
//         * Image
//         * Name
//         * Description = Read From Table
//         * Open Item
//         *      Title
//         *      Cost
//         *      
//         *      
//         * ������ ���� -> ���� Text = Title
//         *              �θ� Image = Image
//         */
//    }
//}