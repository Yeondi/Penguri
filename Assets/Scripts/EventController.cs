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

    // Ÿ�̸� ����  �׽�Ʈ��
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

        //Event Object�ϴܿ� �߰�
        for (int i = 0; i < m_PenaltyPrefabs.Length; i++)
            m_Penalty[i] = Instantiate(m_PenaltyPrefabs[i], this.transform).GetComponent<EventType>();
        for (int i = 0; i < m_RequestPrefabs.Length; i++)
            m_Request[i] = Instantiate(m_RequestPrefabs[i], this.transform).GetComponent<EventType>();
        for (int i = 0; i < m_BehaviourPrefabs.Length; i++)
            m_Behaviour[i] = Instantiate(m_BehaviourPrefabs[i], this.transform).GetComponent<EventType>();

        //����׿�  ����� ���ǹ� �����ֽø� �˴ϴ�.
        if (DEBUG__WEATHEREVENT_ON)
        {
            StartCoroutine(WeatherEvent_Condition2());
            StartCoroutine(BehaviourEvent_Condition2());
        }
    }
    IEnumerator GameScheduler(string _eventType) // Ư�� �̺�Ʈ �ݺ� �����췯
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
     * ����
     * DEBUG__WEATHEREVENT_ON�� On�̰ų�, ����� ���ǹ��� ���������
     * ��ƾ�� Ȱ��ȭ �ǰ� Ȯ���� ���߾� ����̺�Ʈ / �ൿ�̺�Ʈ �߻�
     * �̺�Ʈ �߻��� �� ������ �������̽� �Լ��� �ڷ�ƾ���� ȣ���ϰ� ��ϵǾ��ִ� prefab�� active��ŵ�ϴ�.
     * Active�� ������Ʈ�� EventType�� ����Ǹ� �̺�Ʈ ȿ���� �ҷ�������, Ŭ�� �� �̺�Ʈ ȿ���� ������� ���ÿ� �̼� ���� ������ ���ɴϴ�.
     */

    public IEnumerator Rain() // ����̺�Ʈ - ��
    {
        yield return null;

        if (!eventStatus)
        {
            eventStatus = true;
            StartCoroutine(GameScheduler("Rain")); // ����3
            m_Penalty[0].gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(60f); // ������ Ȱ�� �ð�
            if (m_Penalty[0].isClicked == false)
            {
                GameManager.sharedInstance.GetBuffController().Sickness(true);
            }
            //�������� -> EventType - OnClickEvent()

            yield return StartCoroutine(WeatherEvent_Condition3());
            eventStatus = false;
        }
    }

    public IEnumerator Snow(bool isBlizzard = false) // ����̺�Ʈ - �� / ������
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
                    Debug.Log("Snow ���Ƽ ����");
                    GameManager.sharedInstance.GetBuffController().Sickness(true);
                    m_Penalty[1].gameObject.SetActive(false);
                }
                else if (m_Penalty[1].isClicked == true)
                {
                    Debug.Log("Snow Ŭ�� ��");
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
                    Debug.Log("���Ƽ ����");
                    GameManager.sharedInstance.GetBuffController().Sickness(true);
                    m_Penalty[2].gameObject.SetActive(false);
                }
                else if (m_Penalty[2].isClicked == true)
                {
                    Debug.Log("Blizzard Ŭ�� ��");
                    Penguri.sharedInstance.ADD_GettingWarmUp(15f, 1);
                }
            }
            yield return StartCoroutine(WeatherEvent_Condition3());
            eventStatus = false;
        }
    }

    public IEnumerator RequestFood() // �ൿ�̺�Ʈ - ���̿�û
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

    public IEnumerator RequestKeepingWarm() // �ൿ�̺�Ʈ - ���¿�û
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

    public IEnumerator Talk() // �ൿ�̺�Ʈ - ���ϱ�
    {
        m_Behaviour[0].gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(20f);
        if (m_Behaviour[0].isClicked == false)
        {
            //���� ���
            Debug.Log("����� �ù���������.");
        }
        else
        {
            MoneyManager.sharedInstance.AddHeart(30);
        }
    }

    public IEnumerator Stare() // �ൿ�̺�Ʈ - �ٶ󺸱�
    {
        m_Behaviour[1].gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(20f);
        if (m_Behaviour[1].isClicked == false)
        {
            //���� ���
            Debug.Log("����� �ù���������.");
        }
        else
        {
            MoneyManager.sharedInstance.AddHeart(20);
            Penguri.sharedInstance.buff.setExtraCharge(1.5f); // ������ġ ����
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
                Debug.Log("�� �����ϴ�.");
            }
            else
            {
                StartCoroutine(Snow());
                Debug.Log("���� �����ϴ�.");
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
                Debug.Log("���� �����ϴ�.");
            }
            else
            {
                StartCoroutine(Rain());
                Debug.Log("�� �����ϴ�.");
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

    public bool GetThisChanceResult_Percentage(float Percentage_Chance) // �ۼ�Ʈ �Լ� �Ű������� �ۼ�Ʈ ����
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

    IEnumerator PauseDecreaseAndBeginTime(float fTime) // ������ ���� �����ð� �µ� ���Ҵ� ���߸�, Ÿ�̸Ӵ� �귯���� ������
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