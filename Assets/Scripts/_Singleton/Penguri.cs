using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Penguri : MonoBehaviour
{
    public static Penguri sharedInstance = null;

    [SerializeField]
    Canvas m_canvas;
    GraphicRaycaster m_gr;
    PointerEventData m_ped;

    [SerializeField]
    TMP_Text Status_Food;
    [SerializeField]
    TMP_Text Status_Temperature;

    [Header("Peng's Info")]
    [SerializeField]
    [Description("Peng's Portrait")]
    List<Sprite> Sprites;

    Image currentImage;

    [SerializeField]
    [Description("현재 성장 단계")]
    string m_currentKey = "";
    [SerializeField]
    [Description("현재 허기")]
    float m_currentHunger = 0f;
    [SerializeField]
    [Description("현재 체온")]
    float m_currentTemperature = 0f;
    [SerializeField]
    [Description("최대 허기")]
    float m_MaxHunger = 0f;
    [SerializeField]
    [Description("최대 체온")]
    float m_MaxTemperature = 0f;
    [SerializeField]
    [Description("터치당 체온 상승 값")]
    float m_IncreaseTemperatureByTouch = 0f;
    [SerializeField]
    [Description("1초당 허기 감소 값")]
    float m_DecreaseHungerAmountPerSec = 0f;
    float m_DecreaseHungerAmountPerSec_origin = 0f;
    [SerializeField]
    [Description("1초당 체온 감소 값")]
    float m_DecreaseTemperatureAmountPerSec = 0f;
    float m_DecreaseTemperatureAmountPerSec_origin = 0f;

    [SerializeField]
    [Description("탑 패널에 있는 정보창 / 초당 포만감 감소량 표시기")]
    TMP_Text TopPanel_Info_Food;
    [SerializeField]
    [Description("탑패널에 있는 정보창 / 초당 체온 감소량 표시기")]
    TMP_Text TopPanel_Info_Temp;

    public Dictionary<string, List<string>> m_savedData;

    [SerializeField]
    float m_BabyTime = 0f; // 진화까지 필요한 시간

    public BuffController buff;

    float m_AddedIncreaseAmountHunger = 0f;
    float m_AddedIncreaseAmountTemperature = 0f;

    bool m_usedWarmItem = false;

    int stageId = 0; // 스테이지별 id값

    [SerializeField]
    int AddHeartAmount = 1; // 터치당 하트 획득 수

    [SerializeField]
    float bak_value_temp = 0f; // 초당 체온 백업변수
    [SerializeField]
    float bak_value_hunger = 0f; // 초당 허기 백업변수


    #region 코루틴_WaitForSeconds
    public static readonly Dictionary<float, WaitForSeconds> m_timeInterval = new Dictionary<float, WaitForSeconds>(new FloatComparer());

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if(!m_timeInterval.TryGetValue(seconds,out wfs))
        {
            m_timeInterval.Add(seconds,wfs = new WaitForSeconds(seconds));
        }
        return wfs;
    }
    #endregion

    private void Start()
    {
        if (sharedInstance != null && sharedInstance != this)
            Destroy(gameObject);
        else
            sharedInstance = this;


        m_gr = m_canvas.GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);
        currentImage = gameObject.GetComponent<Image>();

        buff = GameManager.sharedInstance.GetBuffController();

        init();

        UpdateTopPanel_Info();
    }

    void init()
    {
        Status_Food.text = m_currentHunger + "/" + m_MaxHunger;
        Status_Temperature.text = m_currentTemperature + "/" + m_MaxTemperature;

        StartCoroutine(DecreaseHungerAndTemperaturePerSec());

    }

    void Update()
    {
        if (m_savedData == null)
        {
            m_savedData = GameManager.sharedInstance.getTsvData();
            m_AddedIncreaseAmountTemperature = m_IncreaseTemperatureByTouch;
        }

        UpdateTopPanel_Info();

        if (Input.GetMouseButtonDown(0))
        {
            m_ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_gr.Raycast(m_ped, results);

            if (results.Count > 0)
            {
                if (results[0].gameObject.name == "P_Image" && MoneyManager.sharedInstance != null)
                    OnTouched();
            }
        }

        updateGuageBar();
        m_BabyTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        checkEvolutionCondition();
        MaxBalance();
        //PaceMaker();

        if ((m_currentHunger == m_MaxHunger && m_currentTemperature == m_MaxTemperature) && m_currentKey != "EggStage" && m_currentKey != "")
            StartCoroutine(buff.Happiness());

        if (m_currentHunger == m_MaxHunger && m_currentKey != "EggStage" && m_currentKey != "")
            StartCoroutine(buff.FoodBaby());

    }

    void OnTouched() // 터치됐을때
    {
        AddTemperatureByTouch();
        AddHeartByTouch();

        if (QuestHandler.sharedInstance.getEventType1() == "Touch" && QuestHandler.sharedInstance.getEventType2() == "Penguri") // 수정예정
        {
            Broadcast.sharedInstance.Notify("Touch", "Penguri", 1);
        }
    }

    void BackUpValue()
    {
        m_DecreaseHungerAmountPerSec_origin = m_DecreaseHungerAmountPerSec;
        m_DecreaseTemperatureAmountPerSec_origin = m_DecreaseTemperatureAmountPerSec;
    }

    public void LoadSequence(string __key, float _loadedHunger, float _loadedTemperature) // 로드할때
    {
        this.setData(__key, _loadedHunger, _loadedTemperature,
        float.Parse(m_savedData[__key][2]),
        float.Parse(m_savedData[__key][3]),
        float.Parse(m_savedData[__key][4]),
        float.Parse(m_savedData[__key][5]),
        float.Parse(m_savedData[__key][6])); // 가져온 값을 토대로 1차 세팅

        BackUpValue();

        switch (__key)
        {
            case "BabyStage1":
                currentImage.sprite = Sprites[0];
                stageId = 1;
                break;
            case "BabyStage2":
                currentImage.sprite = Sprites[1];
                stageId = 2;
                break;
            case "GrowthStage1":
                currentImage.sprite = Sprites[2];
                currentImage.SetNativeSize();
                stageId = 3;
                break;
            case "GrowthStage2":
                currentImage.sprite = Sprites[3];
                currentImage.SetNativeSize();
                stageId = 4;
                break;
            case "AdultStage":
                currentImage.sprite = Sprites[4];
                currentImage.SetNativeSize();
                stageId = 5;
                break;
        }
    }

    public void setData(string _key,
        float _Hunger,
        float _Temperature,
        float _MaxHunger,
        float _MaxTemperature,
        float _IncreaseTemperatureByTouch,
        float _DecreaseHungerAmountPerSec,
        float _DecreaseTemperatureAmountPerSec) // 초기데이터 셋팅
    {
        m_currentKey = _key;
        m_currentHunger = _Hunger;
        m_currentTemperature = _Temperature;
        m_MaxHunger = _MaxHunger;
        m_MaxTemperature = _MaxTemperature;
        m_IncreaseTemperatureByTouch = _IncreaseTemperatureByTouch;
        m_DecreaseHungerAmountPerSec = _DecreaseHungerAmountPerSec;
        m_DecreaseTemperatureAmountPerSec = _DecreaseTemperatureAmountPerSec;

        BackUpValue();
    }

    public IEnumerator DecreaseHungerAndTemperaturePerSec() // 초당 허기,온도 감소치
    {
        while (true)
        {
            if (buff.isFull)
                m_currentHunger -= 0;
            else
                m_currentHunger -= m_DecreaseHungerAmountPerSec;

            if (m_usedWarmItem)
                m_currentTemperature -= 0;
            else
                m_currentTemperature -= m_DecreaseTemperatureAmountPerSec;
            yield return WaitForSeconds(1f);
        }
    }

    public IEnumerator StopDecreaseTemperature(float __duration) // 초당 체온하강 멈춤
    {
        yield return null;
        Debug.Log("체온 하강 멈춤");
        bak_value_temp = m_DecreaseTemperatureAmountPerSec;
        m_DecreaseTemperatureAmountPerSec = 0;
        yield return WaitForSeconds(__duration);

        m_DecreaseTemperatureAmountPerSec = bak_value_temp;
        Debug.Log("체온 하강 재개");
    }

    public IEnumerator StopDecreaseHunger(float __duration) // 초당 허기하강 멈춤
    {
        yield return null;
        Debug.Log("허기 하강 멈춤");
        bak_value_hunger = m_DecreaseHungerAmountPerSec;
        m_DecreaseHungerAmountPerSec = 0;
        yield return WaitForSeconds(__duration);

        Debug.Log("허기 하강 재개");
        m_DecreaseHungerAmountPerSec = bak_value_hunger;
    }

    public void AddTemperatureByTouch() // 터치당 체온증가
    {
        m_currentTemperature += (m_IncreaseTemperatureByTouch * buff.m_Weight);
        Status_Temperature.text = m_currentTemperature + "/" + m_MaxTemperature;
    }

    public void AddHeartByTouch() // 터치당 하트증가
    {
        MoneyManager.sharedInstance.AddHeart(AddHeartAmount * (int)buff.m_Weight);
    }

    void checkEvolutionCondition() // 진화조건체크  출시 전 m_BabyTime 수정하셔야합니다.
    {
        if (m_currentKey == "EggStage" && m_currentTemperature >= 37.5f ||
            m_currentKey == "BabyStage1" && m_currentTemperature >= 37.5f && m_BabyTime > 10f/*259200f*/ ||
            m_currentKey == "BabyStage2" && m_currentTemperature >= 37.5f && m_BabyTime > 10f/*604800f*/ ||
            m_currentKey == "GrowthStage1" && m_currentTemperature >= 37.5f && m_BabyTime > 10f/*1296000f*/ ||
            m_currentKey == "GrowthStage2" && m_currentTemperature >= 37.5f && m_BabyTime > 10f/*2419200f*/ ||
            m_currentKey == "AdultStage" && m_currentTemperature >= 37.5f && m_BabyTime > 10f)
        {
            ToTheNextStep(m_currentKey);
        }
    }

    public void ToTheNextStep(string _currentKey) // 진화조건 , 디버프 없는상태 전제조건 추가
    {
        if (_currentKey == "EggStage")
        {
            m_currentKey = "BabyStage1";
            stageId = 1;
            currentImage.sprite = Sprites[0];
        }
        else if (_currentKey == "BabyStage1")
        {
            m_currentKey = "BabyStage2";
            stageId = 2;
            currentImage.sprite = Sprites[1];
        }
        else if (_currentKey == "BabyStage2")
        {
            m_currentKey = "GrowthStage1";
            stageId = 3;
            currentImage.sprite = Sprites[2];
            currentImage.SetNativeSize();
        }
        else if (_currentKey == "GrowthStage1")
        {
            m_currentKey = "GrowthStage2";
            stageId = 4;
            currentImage.sprite = Sprites[3];
            currentImage.SetNativeSize();
        }
        else if (_currentKey == "GrowthStage2")
        {
            m_currentKey = "AdultStage";
            stageId = 5;
            currentImage.sprite = Sprites[4];
            currentImage.SetNativeSize();
        }

        if (m_currentKey == QuestHandler.sharedInstance.getEventType2())
        {
            QuestHandler.sharedInstance.CompleteQuest();
        }

        this.setData(m_currentKey, float.Parse(m_savedData[m_currentKey][0]),
            float.Parse(m_savedData[m_currentKey][1]),
            float.Parse(m_savedData[m_currentKey][2]),
            float.Parse(m_savedData[m_currentKey][3]),
            float.Parse(m_savedData[m_currentKey][4]),
                float.Parse(m_savedData[m_currentKey][5]),
                float.Parse(m_savedData[m_currentKey][6]));
        m_BabyTime = 0f;
        UpdateTopPanel_Info();
    }

    public void MaxBalance() // 초과되는 최대값 조절하는 함수
    {
        if (m_currentTemperature > 37.5f)
            m_currentTemperature = 37.5f;
        else if (m_currentTemperature < 0f)
            m_currentTemperature = 0f;

        if (m_currentHunger > m_MaxHunger)
            m_currentHunger = m_MaxHunger;
        else if (m_currentHunger < 0f)
            m_currentHunger = 0f;
    }

    void updateGuageBar() // 게이지바 수치 변경
    {
        Status_Food.text = m_currentHunger + "/" + m_MaxHunger;
        Status_Temperature.text = m_currentTemperature.ToString("F1") + "/" + m_MaxTemperature.ToString("F1");
    }

    public void ADD_GettingWarmUp(float amount, int ea) // 온도 더하는 함수
    {
        m_currentTemperature += amount;
    }

    public void ADD_FillHunger(float amount, int ea) // 허기 더하는 함수
    {
        //Bar_Food.fillAmount += (amount * 0.01f);
        m_currentHunger += (amount * ea);
    }

    public List<float> SyncDataToGM()
    {
        return new List<float>() { this.getCurrentHungerPercentage(), this.getCurrentTemperature() };
    }

    public IEnumerator MultipleStatusIDSpeed(string _ID, string _Type = "", float _Power = 0f, float _Time = 0f) // 특정 능력치 증가(Increase) / 감소(Decrease) 속도를 원하는 배율로 조정
    {// ID : Increase or Decrease / _Type = Heat or Hunger / _Power = 배율 / _Time = 지속시간
        yield return null;

        if (_ID == "Increase")
        {
            if (_Type == "Heat")
            {
                m_DecreaseTemperatureAmountPerSec *= _Power;
                yield return WaitForSeconds(_Time);
                BackUpValue();
            }
            else if (_Type == "Hunger")
            {
                m_DecreaseHungerAmountPerSec *= _Power;
                yield return WaitForSeconds(_Time);
                BackUpValue();
            }
        }
        else if (_ID == "Decrease")
        {
            if (_Type == "Heat")
            {//if -> rain event -> Decrease", "Heat", 1.2f, 150f
                m_DecreaseTemperatureAmountPerSec *= -(_Power);
                yield return WaitForSeconds(_Time);
                BackUpValue(); // 깎인 값 원상복구
            }
            else if (_Type == "Hunger")
            {
                m_DecreaseHungerAmountPerSec *= -(_Power);
                yield return WaitForSeconds(_Time);
                BackUpValue();
            }
        }
        else if (_ID == "Clean")
        {
            yield return null;
            BackUpValue();
        }
    }

    public void UpdateTopPanel_Info() // 탑패널 초당 감소량 표시기 업데이트
    {
        TopPanel_Info_Food.text = m_DecreaseHungerAmountPerSec.ToString() + "/sec";
        TopPanel_Info_Temp.text = m_DecreaseTemperatureAmountPerSec.ToString() + "/sec";
    }

    #region SET_AND_GET_FUNCTIONS

    public void setKey(string __key)
    {
        m_currentKey = __key;
    }

    public string getKey()
    {
        return m_currentKey;
    }

    public void setHunger(float _hunger)
    {
        m_currentHunger = _hunger;
    }

    public float getCurrentHunger()
    {
        return m_currentHunger;
    }

    public void setTemperature(float _temperature)
    {
        m_currentTemperature = _temperature;
    }

    public float getCurrentTemperature()
    {
        return m_currentTemperature;
    }

    public float getMaxHunger()
    {
        return m_MaxHunger;
    }

    public float getMaxTemperature()
    {
        return m_MaxTemperature;
    }

    public float getIncreaseTemperatureByTouch()
    {
        return m_IncreaseTemperatureByTouch;
    }

    public void setDecreaseHungerAmountPerSec(float __Amount)
    {
        m_DecreaseHungerAmountPerSec = __Amount;
    }

    public float getDecreaseHungerAmountPerSec()
    {
        return m_DecreaseHungerAmountPerSec;
    }

    public float getDecreaseTemperatureAmountPerSec()
    {
        return m_DecreaseTemperatureAmountPerSec;
    }

    public float getCurrentHungerPercentage()
    {
        return m_currentHunger / m_MaxHunger;
    }

    public void setAddedIncreaseAmountHunger(float mul_Data)
    {
        m_AddedIncreaseAmountHunger *= mul_Data;
    }

    public float getAddedIncreaseAmountHunger()
    {
        return m_AddedIncreaseAmountHunger;
    }

    public void setAddedIncreaseAmountTemperature(float mul_Data)
    {
        m_AddedIncreaseAmountTemperature *= mul_Data;
    }

    public float getAddedIncreaseAmountTemperature()
    {
        return m_AddedIncreaseAmountTemperature;
    }

    public void setUsedWarmItem(bool bData)
    {
        m_usedWarmItem = bData;
    }

    public bool getUsetWarmItem()
    {
        return m_usedWarmItem;
    }

    public int getStageId()
    {
        return stageId;
    }

    public void setAddHeartAmount(int __Amount)
    {
        AddHeartAmount = __Amount;
    }

    public void setIncreaseTemperatureByTouch(float __Amount)
    {
        m_IncreaseTemperatureByTouch += __Amount;
    }

    public int getAddHeartAmount()
    {
        return AddHeartAmount;
    }

    public void setPreservationOfStat(float __temp = 0.0f, float __hunger = 0.0f) // 시간당 체온,포만감 감소 완화
    {
        m_DecreaseTemperatureAmountPerSec = __temp;
        m_DecreaseHungerAmountPerSec += __hunger;
    }

    #endregion
}
