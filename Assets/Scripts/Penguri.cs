using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Penguri : MonoBehaviour
{
    public static Penguri sharedInstance = null;

    [SerializeField]
    Canvas m_canvas;
    GraphicRaycaster m_gr;
    PointerEventData m_ped;

    Image Bar_Food;
    Image Bar_Temperature;

    [Header("Peng's Info")]
    [SerializeField]
    string m_currentKey = "";
    [SerializeField]
    float m_currentHunger = 0f;
    [SerializeField]
    float m_currentTemperature = 0f;
    [SerializeField]
    float m_MaxHunger = 0f;
    [SerializeField]
    float m_MaxTemperature = 0f;
    [SerializeField]
    float m_IncreaseTemperatureByTouch = 0f;
    [SerializeField]
    float m_DecreaseHungerAmountPerSec = 0f;
    [SerializeField]
    float m_DecreaseTemperatureAmountPerSec = 0f;

    public Dictionary<string, List<string>> m_savedData;

    [SerializeField]
    float m_BabyTime = 0f;

    BuffController buff;

    float m_AddedIncreaseAmountHunger = 0f;
    float m_AddedIncreaseAmountTemperature = 0f;

    //// 최종 터치 효율에 기여하는애
    //[SerializeField]
    //float m_AddAmountEfficiencyOfHeart = 1f;
    //[SerializeField]
    //float m_AddAmountEfficiencyOfTemperature = 1f;

    //펭귄 상태변화 디버그용
    Color DEBUG_BabyStage1 = Color.green;
    Color DEBUG_BabyStage2 = Color.blue;
    Color DEBUG_GrowthStage1 = Color.cyan;
    Color DEBUG_GrowthStage2 = Color.magenta;
    Color DEBUG_AdultStage = Color.red;

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
            Destroy(gameObject);
        else
            sharedInstance = this;


        m_gr = m_canvas.GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);

        Bar_Food = GameObject.Find("Start").transform.GetChild(4).GetChild(3).GetComponent<Image>();
        Bar_Temperature = GameObject.Find("Start").transform.GetChild(5).GetChild(3).GetComponent<Image>();

        buff = GameManager.sharedInstance.GetBuffController();

        init();
    }

    //void Start()
    //{
    //    m_gr = m_canvas.GetComponent<GraphicRaycaster>();
    //    m_ped = new PointerEventData(null);

    //    Bar_Food = GameObject.Find("Start").transform.GetChild(4).GetChild(3).GetComponent<Image>();
    //    Bar_Temperature = GameObject.Find("Start").transform.GetChild(5).GetChild(3).GetComponent<Image>();

    //    buff = GameManager.sharedInstance.GetBuffController();

    //    init();
    //}

    void init()
    {
        Bar_Food.fillAmount = 0.0f;
        Bar_Temperature.fillAmount = 35.0f / 37.5f;

        //m_AddedIncreaseAmountHunger = m_currentHunger;
        //m_AddedIncreaseAmountTemperature = m_currentTemperature;

        //Debug.Log("FOOOOD" + m_AddedIncreaseAmountHunger);
        //Debug.Log("Temp" + m_AddedIncreaseAmountTemperature);

        //StartCoroutine(DecreaseTemperaturePerSec());
        StartCoroutine(DecreaseHungerAndTemperaturePerSec());

    }

    void Update()
    {
        if (m_savedData == null)
        {
            m_savedData = GameManager.sharedInstance.getTsvData();
            //m_AddedIncreaseAmountHunger = ;
            m_AddedIncreaseAmountTemperature = m_IncreaseTemperatureByTouch;
        }
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Debug.Log("드래그중?");
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                m_ped.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                m_gr.Raycast(m_ped, results);

                if (results.Count > 0)
                {
                    if (results[0].gameObject.name == "P_Image")
                        OnTouched();
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            m_ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_gr.Raycast(m_ped, results);

            if (results.Count > 0)
            {
                if (results[0].gameObject.name == "P_Image")
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
        PaceMaker();

        if ((m_currentHunger == m_MaxHunger && m_currentTemperature == m_MaxTemperature) && m_currentKey != "EggStage" && m_currentKey != "")
            StartCoroutine(buff.Happiness());

        if (m_currentHunger == m_MaxHunger && m_currentKey != "EggStage" && m_currentKey != "")
            StartCoroutine(buff.FoodBaby());


    }

    void OnTouched()
    {
        AddTemperatureByTouch();
        AddHeartByTouch();
    }

    public void setData(string _key,
        float _Hunger,
        float _Temperature,
        float _MaxHunger,
        float _MaxTemperature,
        float _IncreaseTemperatureByTouch,
        float _DecreaseHungerAmountPerSec,
        float _DecreaseTemperatureAmountPerSec)
    {
        m_currentKey = _key;
        m_currentHunger = _Hunger;
        m_currentTemperature = _Temperature;
        m_MaxHunger = _MaxHunger;
        m_MaxTemperature = _MaxTemperature;
        m_IncreaseTemperatureByTouch = _IncreaseTemperatureByTouch;
        m_DecreaseHungerAmountPerSec = _DecreaseHungerAmountPerSec;
        m_DecreaseTemperatureAmountPerSec = _DecreaseTemperatureAmountPerSec;
    }

    public IEnumerator DecreaseHungerAndTemperaturePerSec()
    {
        while (true)
        {
            if (buff.isFull)
                m_currentHunger -= 0;
            else
                m_currentHunger -= m_DecreaseHungerAmountPerSec;
            m_currentTemperature -= m_DecreaseTemperatureAmountPerSec;
            //Debug.Log("현재 허기 : " + m_currentHunger);
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    public void AddTemperatureByTouch()
    {
        m_currentTemperature += (m_AddedIncreaseAmountTemperature * buff.m_Weight);
        Bar_Temperature.fillAmount += (m_AddedIncreaseAmountTemperature * buff.m_Weight);
    }

    public void AddHeartByTouch()
    {
        //int _Amount = (int)m_AdditionalAmountToHeart == 0 ? 1 : (int)m_AdditionalAmountToHeart;
        MoneyManager.instance.AddHeart(1 * (int)buff.m_Weight);
    }

    void checkEvolutionCondition()
    {
        if (m_currentKey == "EggStage" && m_currentTemperature >= 37.5f ||
            m_currentKey == "BabyStage1" && m_currentTemperature >= 37.5f && m_BabyTime > 10f/*259200f*/ ||
            m_currentKey == "BabyStage2" && m_currentTemperature >= 37.5f && m_BabyTime > 10f/*604800f*/ ||
            m_currentKey == "GrowthStage1" && m_currentTemperature >= 37.5f && m_BabyTime > 10f/*1296000f*/ ||
            m_currentKey == "GrowthStage2" && m_currentTemperature >= 37.5f && m_BabyTime > 10f/*2419200f*/)
        {
            ToTheNextStep(m_currentKey);
        }
    }

    public void ToTheNextStep(string _currentKey) // 진화조건 , 디버프 없는상태 전제조건 추가
    {
        if (_currentKey == "EggStage")
        {
            m_currentKey = "BabyStage1";
            gameObject.GetComponent<Image>().color = DEBUG_BabyStage1;
        }
        else if (_currentKey == "BabyStage1")
        {
            m_currentKey = "BabyStage2";
            gameObject.GetComponent<Image>().color = DEBUG_BabyStage2;
        }
        else if (_currentKey == "BabyStage2")
        {
            m_currentKey = "GrowthStage1";
            gameObject.GetComponent<Image>().color = DEBUG_GrowthStage1;
        }
        else if (_currentKey == "GrowthStage1")
        {
            m_currentKey = "GrowthStage2";
            gameObject.GetComponent<Image>().color = DEBUG_GrowthStage2;
        }
        else if (_currentKey == "GrowthStage2")
        {
            m_currentKey = "AdultStage";
            gameObject.GetComponent<Image>().color = DEBUG_AdultStage;
        }

        this.setData(m_currentKey, float.Parse(m_savedData[m_currentKey][0]),
            float.Parse(m_savedData[m_currentKey][1]),
            float.Parse(m_savedData[m_currentKey][2]),
            float.Parse(m_savedData[m_currentKey][3]),
            float.Parse(m_savedData[m_currentKey][4]),
                float.Parse(m_savedData[m_currentKey][5]),
                float.Parse(m_savedData[m_currentKey][6]));
        m_BabyTime = 0f;
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

    void PaceMaker() // 실제 허기/온도 값과 Guage Bar로 표현되는 값의 동기화
    {
        if (m_MaxHunger > 0f)
            Bar_Food.fillAmount = m_currentHunger / m_MaxHunger;
        Bar_Temperature.fillAmount = m_currentTemperature / m_MaxTemperature;
    }

    void updateGuageBar()
    {
        Bar_Food.transform.GetChild(0).GetComponent<TMP_Text>().text = m_currentHunger + "/" + m_MaxHunger;
        Bar_Temperature.transform.GetChild(0).GetComponent<TMP_Text>().text = m_currentTemperature.ToString("F2") + "/" + m_MaxTemperature.ToString("F2");
    }

    public void ADD_GettingWarmUp(float amount, int ea)
    {
        Bar_Temperature.fillAmount += amount;
        m_currentTemperature += amount;
        Debug.Log(Bar_Temperature.fillAmount);
    }

    public void ADD_FillHunger(float amount, int ea)
    {
        //Bar_Food.fillAmount += (amount * 0.01f);
        m_currentHunger += amount;
        Debug.Log(Bar_Food.fillAmount);
    }

    public List<float> SyncDataToGM()
    {
        return new List<float>() { this.getCurrentHungerPercentage(), this.getCurrentTemperature() };
    }

    public void DoublyStatusIDSpeed(string _ID) // 특정 능력치 증가(Increase) / 감소(Decrease) 속도를 두배로 조정
    {
        if(_ID == "Increase")
        {
            
        }
        else if(_ID == "Decrease")
        {

        }
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
}
