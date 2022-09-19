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

    private void Start()
    {
        init();
        gameObject.GetComponent<Button>().onClick.AddListener(OnClickEvent);
        m_Penguri = Penguri.sharedInstance;
    }

    public void init()
    {
        m_EventName = name.Replace("(Clone)", "");
        if (m_EventName == "Rain" || m_EventName == "Snow" || m_EventName == "Blizzard"
            || m_EventName == "Seagull" || m_EventName == "Seal")
        {
            m_TypeName = "Penalty";
        }
        else if (m_EventName == "Request_Food" || m_EventName == "Request_Heat")
        {
            m_TypeName = "Request";
        }
        else if (m_EventName == "Talk" || m_EventName == "Stare" || m_EventName == "Sing")
        {
            m_TypeName = "Behaviour";
        }
    }

    public void OnClickEvent()
    {
        Debug.Log("OnClickEvent");
        isClicked = true;
        if (gameObject.name == "Rain")
            m_Penguri.ADD_GettingWarmUp(0.02f, 1);
        else if (gameObject.name == "Snow")
            m_Penguri.ADD_GettingWarmUp(0.03f, 1);
        else if (gameObject.name == "Blizzard")
        {
            m_Penguri.ADD_GettingWarmUp(0.04f, 1);
            //체온,포만감 감소 X
        }
        //gameObject.SetActive(false);
    }
}
