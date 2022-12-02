#define debug
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //스트링테이블 작업

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

    bool isLoadingFinish = false;

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
            Destroy(gameObject);
        else
            sharedInstance = this;

        DontDestroyOnLoad(gameObject);
        Init();
        OriginPos = PengsPortrait.transform.localPosition;
        penguriManager = gameObject.AddComponent<PenguriManager>();
        target = new Vector2(PengsPortrait.transform.localPosition.x, PengsPortrait.transform.localPosition.y + targetYValue);

        isLoadingFinish = true;

    }

    private void Init()
    {
        //gs.recall = true;
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

    public void setIsLoadingFinish(bool __Signal)
    {
        isLoadingFinish = __Signal;
    }

    public bool getIsLoadingFinish()
    {
        return isLoadingFinish;
    }
}

public class PenguriManager : MonoBehaviour
{
    Dictionary<string, List<string>> dict;
    [SerializeField]
    string Sync_Key;
    [SerializeField]
    List<float> SynchronizedData;

#if debug
    public void DEBUG__Start() // 개발용 함수 알 상태에서 시작
    {
        dict = GameManager.sharedInstance.getTsvData();
        Penguri.sharedInstance.setData("EggStage", float.Parse(dict["EggStage"][0]),
        float.Parse(dict["EggStage"][1]), float.Parse(dict["EggStage"][2]),
        float.Parse(dict["EggStage"][3]), float.Parse(dict["EggStage"][4]),
        float.Parse(dict["EggStage"][5]), float.Parse(dict["EggStage"][6]));
        Penguri.sharedInstance.setHunger(float.Parse(dict["EggStage"][0]));
        Penguri.sharedInstance.setTemperature(float.Parse(dict["EggStage"][1]));
        // 터치당 온도증가 0.004
        // 초당 허기감소 -1(없음)
        // 초당 온도 감소 0.006
    }
#endif

    private void Start()
    {
        SynchronizedData = new List<float>();
    }

    public void SyncStatus()
    {
        Sync_Key = Penguri.sharedInstance.getKey();
        SynchronizedData = Penguri.sharedInstance.SyncDataToGM();
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
//         * 누른애 기준 -> 형제 Text = Title
//         *              부모 Image = Image
//         */
//    }
//}