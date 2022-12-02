using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventType : EventController
{
    public string m_EventName;
    public string m_TypeName;

    public bool isClicked = false;

    Penguri m_Penguri;

    float m_CurrentTime = 0f;
    bool StartTimer = false;

    private void Start()
    {
        init();
        gameObject.GetComponent<Button>().onClick.AddListener(OnClickEvent);
        m_Penguri = Penguri.sharedInstance;
    }

    private void Update()
    {
        if (StartTimer)
        {
            m_CurrentTime += Time.deltaTime;
        }
    }

    void ResetTimer() // ȥ�� ������ ���ؼ� ���� �� �ʱ�ȭ
    {
        StartTimer = false;
        m_CurrentTime = 0f;
    }

    public void init()
    {
        m_EventName = gameObject.name.Replace("(Clone)", "");
        if (m_EventName == "Rain" || m_EventName == "Snow" || m_EventName == "Blizzard")
        {
            m_TypeName = "Penalty";
            if (m_EventName == "Rain")
            {
                ResetTimer();

                Penguri.sharedInstance.ADD_GettingWarmUp(-5f, 1);
                Penguri.sharedInstance.MultipleStatusIDSpeed("Decrease", "Heat", 1.2f, 150f);
                StartTimer = true;
            }
            else if(m_EventName == "Snow")
            {
                ResetTimer();

                Penguri.sharedInstance.ADD_GettingWarmUp(-8f, 1);
                Penguri.sharedInstance.MultipleStatusIDSpeed("Decrease", "Heat", 1.5f, 150f);
                StartTimer = true;
            }
            else if(m_EventName == "Blizzard")
            {
                ResetTimer();

                Penguri.sharedInstance.ADD_GettingWarmUp(-12f, 1);
                Penguri.sharedInstance.MultipleStatusIDSpeed("Decrease", "Heat", 2f, 250f);
                GameManager.sharedInstance.GetItemStatusManager().Lock_Unlock_WarmItem(false);
            }
        }
        else if (m_EventName == "Request_Food" || m_EventName == "Request_Heat")
        {
            m_TypeName = "Request";
            //�ٸ� ȿ�� ����
        }
        else if (m_EventName == "Talk" || m_EventName == "Stare")
        {
            m_TypeName = "Behaviour";
            //�ٸ� ȿ�� ����
        }
    }

    public void OnClickEvent()
    {
        isClicked = true;

        if (m_EventName == "Rain")
        {
            float returnTime = 150f - m_CurrentTime; // �̼� ������ �̺�Ʈ Ÿ��(150��) �� �����ð��� �����Ͽ� ü���ϰ� ���߱�.  ��) 120�� ���� �� ������ �� 30�ʰ� ü���ϰ� ����
            m_Penguri.ADD_GettingWarmUp(8f, 1);
            Penguri.sharedInstance.MultipleStatusIDSpeed("Clean");
            Penguri.sharedInstance.StartCoroutine("StopDecreaseTemperature", returnTime);
        }
        else if (m_EventName == "Snow")
        {
            float returnTime = 150f - m_CurrentTime;
            m_Penguri.ADD_GettingWarmUp(12f, 1);
            Penguri.sharedInstance.MultipleStatusIDSpeed("Clean");
            Penguri.sharedInstance.StartCoroutine("StopDecreaseTemperature", returnTime);
        }
        else if (m_EventName == "Blizzard")
        {
            float returnTime = 250f - m_CurrentTime;
            m_Penguri.ADD_GettingWarmUp(15f, 1);
            Penguri.sharedInstance.MultipleStatusIDSpeed("Clean");
            Penguri.sharedInstance.StartCoroutine("StopDecreaseTemperature", returnTime);
        }
        gameObject.SetActive(false);
    }
}
