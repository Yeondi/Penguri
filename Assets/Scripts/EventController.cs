using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour
{
    enum Penalty
    {
        Rain,
        Snow,
        Blizzard,
        Seagull,
        Seal,
    }

    enum Request
    {
        Food,
        Heat
    }

    enum Behaviour
    {
        Talk,
        Stare,
        Sing
    }

    enum AD
    {
        Boost,
        Heart,
        Coin,
        TimeLeaf
    }

    [SerializeField]
    GameObject[] m_PenaltyPrefabs;

    [SerializeField]
    GameObject[] m_RequestPrefabs;

    [SerializeField]
    GameObject[] m_BehaviourPrefabs;

    //[SerializeField]
    //GameObject[] m_AD;

    EventType[] m_Penalty;
    EventType[] m_Request;
    EventType[] m_Behaviour;

    int nCurrent_Percentage = 0;

    bool eventStatus = false;

    public static readonly WaitForSeconds m_waitForSeconds = new WaitForSeconds(600f);


    public bool DEBUG__WEATHEREVENT_ON = false;

    // 타이머 변수  테스트용
    [SerializeField]
    float _WeatherTimer = 0f;
    private void Start()
    {
        init();
    }

    void init()
    {
        m_Penalty = new EventType[m_PenaltyPrefabs.Length];
        m_Request = new EventType[m_RequestPrefabs.Length];
        m_Behaviour = new EventType[m_BehaviourPrefabs.Length];

        //Event Object하단에 추가
        for (int i = 0; i < m_PenaltyPrefabs.Length; i++)
            m_Penalty[i] = Instantiate(m_PenaltyPrefabs[i], this.transform).GetComponent<EventType>();
        for (int i = 0; i < m_RequestPrefabs.Length; i++)
            m_Request[i] = Instantiate(m_RequestPrefabs[i], this.transform).GetComponent<EventType>();
        for (int i = 0; i < m_BehaviourPrefabs.Length; i++)
            m_Behaviour[i] = Instantiate(m_BehaviourPrefabs[i], this.transform).GetComponent<EventType>();

        //디버그용  출시전 조건문 지워주시면 됩니다.
        if (DEBUG__WEATHEREVENT_ON)
        {
            StartCoroutine(WeatherEvent_Condition2());
            StartCoroutine(BehaviourEvent_Condition2());
        }
    }
    IEnumerator GameScheduler(string _eventType) // 특정 이벤트 반복 스케쥴러
    {
        if (Penguri.sharedInstance.getKey() != "EggStage")
            yield return null;


        if (_eventType == "Rain" || _eventType == "Snow")
            _WeatherTimer = 150f;
        else if (_eventType == "Blizzard")
            _WeatherTimer = 250f;

        while (_WeatherTimer > 0)
        {
            _WeatherTimer -= Time.deltaTime;
            yield return null;

            if (_WeatherTimer <= 0)
            {
                StartCoroutine(CleanEvent());
                if(_eventType == "Blizzard")
                    GameManager.sharedInstance.GetItemStatusManager().Lock_Unlock_WarmItem(true);
            }
        }
    }

    IEnumerator CleanEvent()
    {
        Debug.Log("Clean Event");
        yield return null;
        Penguri.sharedInstance.MultipleStatusIDSpeed("Clean", "", 1f);
    }

    /*
     * 로직
     * DEBUG__WEATHEREVENT_ON이 On이거나, 출시전 조건문을 없앴을경우
     * 루틴이 활성화 되고 확률에 맞추어 기상이벤트 / 행동이벤트 발생
     * 이벤트 발생시 각 열거형 인터페이스 함수를 코루틴으로 호출하고 등록되어있는 prefab을 active시킵니다.
     * Active된 오브젝트는 EventType이 실행되며 이벤트 효과가 불러와지며, 클릭 시 이벤트 효과는 사라짐과 동시에 미션 성공 보상이 들어옵니다.
     */

    public IEnumerator Rain() // 기상이벤트 - 비
    {
        yield return null;

        if (!eventStatus)
        {
            eventStatus = true;
            StartCoroutine(GameScheduler("Rain")); // 조건3
            m_Penalty[0].gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(60f); // 아이콘 활성 시간
            if (m_Penalty[0].isClicked == false)
            {
                GameManager.sharedInstance.GetBuffController().Sickness(true);
            }
            //성공조건 -> EventType - OnClickEvent()

            yield return StartCoroutine(WeatherEvent_Condition3());
            eventStatus = false;
        }
    }

    public IEnumerator Snow(bool isBlizzard = false) // 기상이벤트 - 비 / 눈보라
    {
        if (!eventStatus)
        {
            if (!isBlizzard) // = Snow
            {
                m_Penalty[1].gameObject.SetActive(true);
                Penguri.sharedInstance.ADD_GettingWarmUp(-8f, 1);
                Penguri.sharedInstance.MultipleStatusIDSpeed("Decrease", "Heat", 1.5f);
                eventStatus = true;
                StartCoroutine(GameScheduler("Snow"));
                yield return new WaitForSecondsRealtime(60f);
                if (m_Penalty[1].isClicked == false)
                {
                    Debug.Log("Snow 페널티 받음");
                    GameManager.sharedInstance.GetBuffController().Sickness(true);
                    m_Penalty[1].gameObject.SetActive(false);
                }
                else if (m_Penalty[1].isClicked == true)
                {
                    Debug.Log("Snow 클릭 됨");
                    Penguri.sharedInstance.ADD_GettingWarmUp(12f, 1);
                }
            }
            else if (isBlizzard)
            {
                m_Penalty[2].gameObject.SetActive(true);
                eventStatus = true;
                StartCoroutine(GameScheduler("Blizzard"));
                yield return new WaitForSecondsRealtime(60f);
                if (m_Penalty[2].isClicked == false)
                {
                    Debug.Log("페널티 받음");
                    GameManager.sharedInstance.GetBuffController().Sickness(true);
                    m_Penalty[2].gameObject.SetActive(false);
                }
                else if (m_Penalty[2].isClicked == true)
                {
                    Debug.Log("Blizzard 클릭 됨");
                    Penguri.sharedInstance.ADD_GettingWarmUp(15f, 1);
                }
            }
            yield return StartCoroutine(WeatherEvent_Condition3());
            eventStatus = false;
        }
    }

    public IEnumerator RequestFood() // 행동이벤트 - 먹이요청
    {
        m_Request[0].gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(20f);
        if (m_Request[0].isClicked == false)
        {
            Penguri.sharedInstance.ADD_FillHunger(-5f, 1);
        }
        else
        {
            Penguri.sharedInstance.ADD_FillHunger(5f, 1);
        }

    }

    public IEnumerator RequestKeepingWarm() // 행동이벤트 - 보온요청
    {
        m_Request[1].gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(20f);
        if (m_Request[1].isClicked == false)
        {
            Penguri.sharedInstance.ADD_GettingWarmUp(-2f, 1);
        }
        else
        {
            Penguri.sharedInstance.ADD_GettingWarmUp(2f, 1);
        }
    }

    public IEnumerator Talk() // 행동이벤트 - 말하기
    {
        m_Behaviour[0].gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(20f);
        if (m_Behaviour[0].isClicked == false)
        {
            //문구 출력
            Debug.Log("펭귄이 시무룩해졌다.");
        }
        else
        {
            MoneyManager.sharedInstance.AddHeart(30);
        }
    }

    public IEnumerator Stare() // 행동이벤트 - 바라보기
    {
        m_Behaviour[1].gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(20f);
        if (m_Behaviour[1].isClicked == false)
        {
            //문구 출력
            Debug.Log("펭귄이 시무룩해졌다.");
        }
        else
        {
            MoneyManager.sharedInstance.AddHeart(20);
            Penguri.sharedInstance.buff.setExtraCharge(1.5f); // 최종터치 배율
            yield return new WaitForSecondsRealtime(60f);
            Penguri.sharedInstance.buff.setExtraCharge(1.0f);
        }
    }

    public void Boost()
    {

    }

    public void HeartReward()
    {

    }

    public void CashReward()
    {

    }

    public void SlowDownTimeDecrease()
    {

    }

    IEnumerator WeatherEvent_Condition2()
    {
        if (Penguri.sharedInstance.getKey() != "EggStage")
        {
            float fValue = Random.Range(70f, 91f); // 70 ~ 91f
            Debug.Log("fValue : " + fValue);
            yield return new WaitForSecondsRealtime(fValue);
            if (fValue % 2 == 0)
            {
                StartCoroutine(Rain());
                Debug.Log("비가 내립니다.");
            }
            else
            {
                StartCoroutine(Snow());
                Debug.Log("눈이 내립니다.");
            }
        }
    }

    IEnumerator WeatherEvent_Condition3()
    {
        Debug.Log("Condition3 Good to go");
        yield return new WaitForSecondsRealtime(100f); // 100f
        bool isOn = GetThisChanceResult_Percentage(30f);
        if (isOn)
        {
            Debug.Log("nCurrent_Percentage : " + nCurrent_Percentage);
            if (nCurrent_Percentage <= 10)
            {
                StartCoroutine(Snow());
                Debug.Log("눈이 내립니다.");
            }
            else
            {
                StartCoroutine(Rain());
                Debug.Log("비가 내립니다.");
            }
        }

        StartCoroutine(WeatherEvent_Condition3());
    }

    IEnumerator BehaviourEvent_Condition2()
    {
        if (Penguri.sharedInstance.getKey() != "EggStage")
        {
            float fValue = Random.Range(20f, 41f);
            yield return new WaitForSecondsRealtime(fValue);
            if (fValue % 2 == 0)
            {
                StartCoroutine(Talk());
                Debug.Log("Talk");
            }
            else
            {
                StopCoroutine(Stare());
                Debug.Log("Stare");
            }

            StartCoroutine(BehaviourEvent_Condition3());
        }
    }

    IEnumerator BehaviourEvent_Condition3()
    {
        yield return new WaitForSecondsRealtime(30f);
        float fValue = Random.Range(0, 61f);
        if (fValue <= 2f)
        {
            if (Random.Range(0, 51) % 2 == 0)
                StartCoroutine(RequestFood());
            else
                StartCoroutine(RequestKeepingWarm());
        }
        else if (fValue <= 10f)
        {
            StartCoroutine(Stare());
            Debug.Log("Stare");
        }
        else
        {
            StartCoroutine(Talk());
            Debug.Log("Talk");
        }
    }

    public bool GetThisChanceResult(float Chance)
    {
        if (Chance < 0.0000001f)
        {
            Chance = 0.0000001f;
        }

        bool Success = false;
        int RandAccuracy = 10000000;
        float RandHitRange = Chance * RandAccuracy;
        int Rand = UnityEngine.Random.Range(1, RandAccuracy + 1);
        if (Rand <= RandHitRange)
        {
            Success = true;
        }
        return Success;
    }

    public bool GetThisChanceResult_Percentage(float Percentage_Chance) // 퍼센트 함수 매개변수에 퍼센트 기입
    {
        if (Percentage_Chance < 0.0000001f)
        {
            Percentage_Chance = 0.0000001f;
        }

        Percentage_Chance = Percentage_Chance / 100;

        bool Success = false;
        int RandAccuracy = 10000000;
        float RandHitRange = Percentage_Chance * RandAccuracy;
        int Rand = UnityEngine.Random.Range(1, RandAccuracy + 1);
        if (Rand <= RandHitRange)
        {
            Success = true;
            nCurrent_Percentage = Rand;
        }


        return Success;
    }

    IEnumerator PauseDecreaseAndBeginTime(float fTime) // 아이템 사용시 일정시간 온도 감소는 멈추며, 타이머는 흘러가기 시작함
    {
        Penguri.sharedInstance.setUsedWarmItem(true);
        while (fTime > 1f)
        {
            fTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Penguri.sharedInstance.setUsedWarmItem(false);
    }
}