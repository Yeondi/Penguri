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

    void ResetTimer() // 혼선 방지를 위해서 시작 전 초기화
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
            //다른 효과 없음
        }
        else if (m_EventName == "Talk" || m_EventName == "Stare")
        {
            m_TypeName = "Behaviour";
            //다른 효과 없음
        }
    }

    public void OnClickEvent()
    {
        isClicked = true;

        if (m_EventName == "Rain")
        {
            float returnTime = 150f - m_CurrentTime; // 미션 성공지 이벤트 타임(150초) 중 남은시간을 리턴하여 체온하강 멈추기.  예) 120초 지난 후 눌렀을 시 30초간 체온하강 멈춤
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
