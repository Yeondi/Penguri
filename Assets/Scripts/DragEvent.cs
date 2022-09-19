using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragEvent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    Vector2 m_Origin;

    bool bDropToUse = false;

    Penguri m_peng;

    [SerializeField]
    int ea;

    float m_coolTime;

    [SerializeField]
    bool isOn;
    private void Start()
    {
        Debug.Log("Drag Event Awake");
        //m_peng = GameManager.sharedInstance.GetPenguri();

        ea = 1;
        m_coolTime = 0f;
        isOn = false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        m_Origin = gameObject.transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameObject.tag == "Draggable" && isOn == true)
        {
            Debug.Log("아이템 드래그 중");
            transform.position = eventData.position;
        }
        else
        {
            m_coolTime += Time.deltaTime;
            if(m_coolTime > 0.3f)
            {
                Debug.Log("드래그중 :: 클릭!");
                Penguri.sharedInstance.AddHeartByTouch();
                Penguri.sharedInstance.AddTemperatureByTouch();
                m_coolTime = 0f;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = m_Origin;

        if (bDropToUse)
        {
            if (gameObject.transform.parent.tag == "Food")
            {
                var temporaryData = gameObject.transform.parent.GetComponent<Food>();
                Debug.Log(gameObject.transform.parent.name + "이 사용되었습니다.");
                Penguri.sharedInstance.ADD_FillHunger(temporaryData.IncreaseFillings, ea);
            }
            else if(gameObject.transform.parent.tag == "WarmItem")
            {
                var temporaryData = gameObject.transform.parent.GetComponent<WarmItem>();
                Debug.Log(gameObject.transform.parent.name + "이 사용되었습니다.");
                Penguri.sharedInstance.ADD_GettingWarmUp(temporaryData.IncreaseTemperature, ea);
            }
            Debug.Log("bDropToUse : " + bDropToUse);
            bDropToUse = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Penguin")
        {
            bDropToUse = true;
            Debug.Log("bDropToUse : " + bDropToUse);
        }
    }

    public void setIsOn(bool __isOn__)
    {
        isOn = __isOn__;
    }

    public bool getIsOn()
    {
        return isOn;
    }
}
