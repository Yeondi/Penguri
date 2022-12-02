using System;
using System.IO;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using TMPro;

[System.Serializable]
public class QuestInfo
{
    public string Quest_Key;
    public int Quest_Id;
    public int Quest_Progress;
    public int Quest_Goal;

    public QuestInfo(string quest_Key = "" , int quest_Id = 0, int quest_Progress = 0, int quest_Goal = 0)
    {
        Quest_Key = quest_Key;
        Quest_Id = quest_Id;
        Quest_Progress = quest_Progress;
        Quest_Goal = quest_Goal;
    }
}

[System.Serializable]
public class ItemInfo
{
    public bool b_Shrimp = false;
    public bool b_SmallOctopus = false;
    public bool b_Sardine = false;
    public bool b_Mackerel = false;
    public bool b_Squid = false;
    public bool b_Octopus = false;

    public bool b_Cocoa = false;
    public bool b_Handwarmer = false;
    public bool b_Cushion = false;
    public bool b_Beanie = false;
    public bool b_Muffler = false;
    public bool b_Cape = false;

    public int b_Diet = 0;
    public int b_Recycling = 0;
    public int b_Energy = 0;
    public int b_RenewableEnergy = 0;
    public int b_Transport = 0;
    public int b_Industry = 0;
    public int b_MarineProtection = 0;
    public int b_ForestAndSoil = 0;

    public ItemInfo(bool _Shrimp,bool _SmallOctopus,bool _Sardine,bool _Mackerel,bool _Squid, bool _Octopus
        ,bool _Cocoa,bool _Handwarmer,bool _Cushion,bool _Beanie, bool _Muffler, bool _Cape
        , int _Diet, int _Recycling, int _Energy, int _RenewableEnergy, int _Transport, int _Industry, int _MarineProtection, int _ForestAndSoil)
    {
        b_Shrimp = _Shrimp;
        b_SmallOctopus = _SmallOctopus;
        b_Sardine = _Sardine;
        b_Mackerel = _Mackerel;
        b_Squid = _Squid;
        b_Octopus = _Octopus;

        b_Cocoa = _Cocoa;
        b_Handwarmer = _Handwarmer;
        b_Cushion = _Cushion;
        b_Beanie = _Beanie;
        b_Muffler = _Muffler;
        b_Cape = _Cape;

        b_Diet = _Diet;
        b_Recycling = _Recycling;
        b_Energy = _Energy;
        b_RenewableEnergy = _RenewableEnergy;
        b_Transport = _Transport;
        b_Industry = _Industry;
        b_MarineProtection = _MarineProtection;
        b_ForestAndSoil = _ForestAndSoil;

    }
}

public class UserData
{
    public string m_nickname;
    public int m_heart = 0;
    public int m_coin = 0;
    //public string Quest_Key = "Tutorial_01";

    public float m_Temperature = 0f;
    public float m_Hunger = 0f;

    public QuestInfo questInfo;
    public ItemInfo itemInfo;

    public string currentStage = "EggStage";

    public int FoodStage = 0;  // deprecated
    public int WarmItemStage = 0; // deprecated
    public int GlobalWarmingStage = 0; // deprecated

    public string LastLoginDateTime; // 날짜
    public string LastLoginTime; // 시간 - 종료 후 재접속시 페널티 계산
    public int Day_Attendance = 0;
}



[System.Serializable]
public class UserDataManager : MonoBehaviour
{
    public static UserDataManager sharedInstance = null;

    private static readonly string PrivateKey = "1718hy9dsf0jsdlfjds0pa9ids78ahgf81h32re";

    UserData player = new UserData();

    string path;
    string filename = "save.atree";

    [SerializeField]
    QuestHandler QuestHandler;

    public TMP_Text test_text;

    public bool doesNotExistSaveFile = false;

    bool CallingSucceeded = false;

    public bool bGoodToLoad = false;

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
            Destroy(gameObject);
        else
            sharedInstance = this;

        DontDestroyOnLoad(gameObject);


        path = Application.persistentDataPath + "/";
        print(path);

        #region Tutorial
        //var jsonData = JsonUtility.ToJson(player);

        //newData = JsonUtility.FromJson<UserData>(jsonData);

        //print("닉네임 : " + newData.m_nickname + " 하트 : " + newData.m_heart + " 코인 : " + newData.m_coin);
        #endregion

        StartCoroutine(init());


    }


    public IEnumerator init()
    {
        yield return new WaitForEndOfFrame();
        while (!CallingSucceeded)
        {
            if (GameManager.sharedInstance.getIsLoadingFinish() && bGoodToLoad)
            {
                Debug.Log("불러오기 시작");
                LoadData();
                CallingSucceeded = true;

                StartCoroutine(AsyncSave());
            }
            else
            {
                yield return new WaitForSeconds(1f);
                StopCoroutine(init());
                StartCoroutine(init());
            }
        }
    }

    #region Save_And_Load
    public void SaveData()
    {
        var ItInfo = ItemStatusManager.sharedInstance;

        player.questInfo = new QuestInfo(QuestHandler.getKey(),QuestHandler.getId(),QuestHandler.getProgress(),QuestHandler.getGoal());

        player.itemInfo = new ItemInfo(ItInfo.GetShrimp().isPurchased, ItInfo.GetSmallOctopus().isPurchased, ItInfo.GetSardine().isPurchased, ItInfo.GetMackerel().isPurchased, ItInfo.GetSquid().isPurchased, ItInfo.GetOctopus().isPurchased,
            ItInfo.GetCocoa().isPurchased, ItInfo.GetHandwarmer().isPurchased, ItInfo.GetCushion().isPurchased, ItInfo.GetBeanie().isPurchased, ItInfo.GetMuffler().isPurchased, ItInfo.GetCape().isPurchased,
            ItInfo.GetDiet().getLevel(), ItInfo.GetRecycling().getLevel(), ItInfo.GetEnergy().getLevel(), ItInfo.GetRenewableEnergy().getLevel(), ItInfo.GetTransport().getLevel(), ItInfo.GetIndustry().getLevel(), ItInfo.GetMarineProtection().getLevel(), ItInfo.GetForestAndSoil().getLevel());

        player.m_heart = MoneyManager.sharedInstance.nHeart;
        player.m_coin = MoneyManager.sharedInstance.nCoin;

        player.m_Hunger = Penguri.sharedInstance.getCurrentHunger();
        player.m_Temperature = Penguri.sharedInstance.getCurrentTemperature();

        QuestHandler.isLoaded = true;

        player.currentStage = Penguri.sharedInstance.getKey();
        //player.FoodStage = ItemStatusManager.sharedInstance.b_Status_Level;
        //player.WarmItemStage = ItemStatusManager.sharedInstance.b_Status_Level;
        //player.GlobalWarmingStage = ItemStatusManager.sharedInstance.b_Status_Level;
        player.LastLoginDateTime = NTP.sharedInstance.getServerTime().ToShortDateString().Replace("-","");
        player.LastLoginTime = NTP.sharedInstance.getServerTime().ToShortTimeString();
        Debug.Log("최종 접속 시간 : "  + player.LastLoginTime);
        player.Day_Attendance = PopUpController.sharedInstance.GetAttendance().Day_Attendance;

        string jsonString = SaveData_ToJson(player);
        string encryptString = Encrypt(jsonString);
        SaveFile(encryptString);

        printData(true);
    }

    private static void SaveFile(string jsonData)
    {
        using (FileStream fs = new FileStream(GetPath(), FileMode.Create, FileAccess.Write))
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

            fs.Write(bytes, 0, bytes.Length);
        }
    }

    public UserData LoadData()
    {
        if (bGoodToLoad)
        {
            if (!File.Exists(GetPath()))
            {
                print("파일이 존재하지 않습니다. 새 프로파일을 생성합니다.");
                doesNotExistSaveFile = true;
                CreateNewData();
                return null;
            }
            else
            {

                string encryptedData = LoadFile(GetPath());
                string decryptedData = Decrypt(encryptedData);

                Debug.Log(decryptedData);

                UserData player = JsonToData(decryptedData);

                MoneyManager.sharedInstance.LoadData(player.m_heart, player.m_coin); // 재화 로드
                //QuestHandler.LoadData(player.questInfo.Quest_Key);
                QuestHandler.LoadData(player.questInfo);
                Penguri.sharedInstance.LoadSequence(player.currentStage,player.m_Hunger,player.m_Temperature);

                ItemStatusManager.sharedInstance.Load_ItemStatus(player.itemInfo);

                PopUpController.sharedInstance.GetAttendance().Day_Attendance = player.Day_Attendance;

                if(Calculate_TimeDifference(player))
                {
                    PopUpController.sharedInstance.Attendance_Sequence(false);
                }

                printData(false);
                Debug.Log("불러오기 완료");

                return player;
            }

        }
        return null;

    }

    private static string LoadFile(string path)
    {
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, (int)fs.Length);

            string jsonString = System.Text.Encoding.UTF8.GetString(bytes);
            return jsonString;
        }
    }
    #endregion

    #region About_Json
    private static string SaveData_ToJson(UserData player)
    {
        string data = JsonUtility.ToJson(player);

#if UNITY_EDITOR
        File.WriteAllText(GetPath_Debug(), data); // json으로 빼서 데이터 저장내역 보여줌 / 디버그용이고, 실제 디바이스에선 이 기록이 남지 않습니다.
#endif

        return data;
    }

    private UserData JsonToData(string jsonData)
    {
        UserData data = JsonUtility.FromJson<UserData>(jsonData);

        return data;
    }
    #endregion

    #region Encrypt_And_Decrypt
    private static string Encrypt(string data)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
        RijndaelManaged rm = CreateRijndaelManaged();
        ICryptoTransform ct = rm.CreateEncryptor();
        byte[] results = ct.TransformFinalBlock(bytes, 0, bytes.Length);
        return System.Convert.ToBase64String(results, 0, results.Length);
    }

    private static string Decrypt(string data)
    {
        byte[] bytes = System.Convert.FromBase64String(data);
        RijndaelManaged rm = CreateRijndaelManaged();
        ICryptoTransform ct = rm.CreateDecryptor();
        byte[] resultArray = ct.TransformFinalBlock(bytes, 0, bytes.Length);
        return System.Text.Encoding.UTF8.GetString(resultArray);
    }

    private static RijndaelManaged CreateRijndaelManaged()
    {
        byte[] keyArray = Encoding.UTF8.GetBytes(PrivateKey);
        var result = new RijndaelManaged();

        var newKeysArray = new byte[16];
        Array.Copy(keyArray, 0, newKeysArray, 0, 16);

        result.Key = newKeysArray;
        result.Mode = CipherMode.ECB;
        result.Padding = PaddingMode.PKCS7;
        return result;
    }

    static string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "save.atree");
    }

    static string GetPath_Debug()
    {
        return Path.Combine(Application.persistentDataPath, "save.json");
    }
    #endregion

    void CreateNewData() // 저장파일이 감지되지 않을시 새로운 데이터를 만들고 즉시저장
    {
        player.questInfo = new QuestInfo();

        player.currentStage = "EggStage";
        player.m_heart = 0;
        player.m_coin = 0;
        //player.questInfo = new QuestInfo("Tutorial_01",1,0,30);
        QuestHandler.SetData("Tutorial_01");
        player.questInfo = new QuestInfo(QuestHandler.getKey(), QuestHandler.getId(), QuestHandler.getProgress(), QuestHandler.getGoal());

        Penguri.sharedInstance.setKey(player.currentStage);
        Penguri.sharedInstance.LoadSequence(player.currentStage,0f,35f);
        PopUpController.sharedInstance.Attendance_Sequence(true);

        SaveData();
    }

    IEnumerator AsyncSave() // 비동기 저장
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(10f);
            SaveData();
            Debug.Log("자동저장 되었습니다.");
        }
    }

    bool Calculate_TimeDifference(UserData playerData) // 출석보상 지급 판단을 위한 마지막 로그인과 현재 로그인 시간을 대조함수
    {
        var playerData_ToShortString = int.Parse(playerData.LastLoginDateTime); // 플레이어 세이브 내 저장된 시간에서 날짜만 추출 후 int로 캐스팅

        var DateTime_Now = int.Parse(NTP.sharedInstance.getServerTime().ToShortDateString().Replace("-", "")); // 현재 시간에서 날짜만 추출 후 int로 캐스팅

        //debug
        //var DateTime_Now = int.Parse(DateTime.Now.ToShortDateString().Replace("-", ""));


        if (playerData_ToShortString < DateTime_Now) // 년도(4자리)+월(2자리)+일(2자리) 총 8자리 int형 수 비교 후 현재시간 값이 더 크면 더 나중이니 true리턴
            return true;

        return false;
    }

    void printData(bool isSave)
    {
        if (isSave)
        {
            if (test_text != null)
            {
                test_text.text = "닉네임 : " + player.m_nickname + "<br>하트 : " + player.m_heart + "<br>코인 : " + player.m_coin + "<br>현재 퀘스트 : " + player.questInfo.Quest_Key +
                "<br>성장단계 : " + player.currentStage + "<br>현재 해금 단계 : " + (player.FoodStage + (player.WarmItemStage*10) + (player.GlobalWarmingStage * 100)) + "<br>저장되었습니다.";
            }
            else
            {
                Debug.Log("닉네임 : " + player.m_nickname + "<br>하트 : " + player.m_heart + "<br>코인 : " + player.m_coin + "<br>현재 퀘스트 : " + player.questInfo.Quest_Key +
                "<br>성장단계 : " + player.currentStage + "<br>현재 해금 단계 : " + (player.FoodStage + (player.WarmItemStage * 10) + (player.GlobalWarmingStage * 100)) + "<br>저장되었습니다.");
            }
        }
        else
        {
            if (test_text != null)
            {
                test_text.text = "닉네임 : " + player.m_nickname + "<br>하트 : " + player.m_heart + "<br>코인 : " + player.m_coin + "<br>현재 퀘스트 : " + QuestHandler.getKey() +
                    "<br>성장단계 : " + player.currentStage + "<br>현재 해금 단계 : " + (player.FoodStage + (player.WarmItemStage * 10) + (player.GlobalWarmingStage * 100)) + "<br>불러오기가 완료되었습니다.";
            }
            else
            {
                Debug.Log("닉네임 : " + player.m_nickname + "하트 : " + MoneyManager.sharedInstance.nHeart + "코인 : " + MoneyManager.sharedInstance.nCoin + "현재 퀘스트 : " + QuestHandler.getKey() +
                    "성장단계 : " + Penguri.sharedInstance.getKey() + "<br>현재 해금 단계 : " + (player.FoodStage + (player.WarmItemStage * 10) + (player.GlobalWarmingStage * 100)) + "불러오기가 완료되었습니다.");
            }
        }
    }
}
