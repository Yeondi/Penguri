using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarmItem : Item
{
    [SerializeField]
    Image go_Clock;
    [SerializeField]
    TMP_Text ClockText;
    [SerializeField]
    float fTime;

    bool isInUse = false;


    void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();
        UpdateDataForWarmItems();

        //transform.GetChild(3).GetComponent<Button>().onClick.AddListener
            //(delegate { checkUnlockQualify(); });

        SetCost();

        ClockText.text = "체온 유지 시간 <color=#FFE400>" + TimeDuration + "</color>, 초당 제온 감소 <color=#FFE400>" + 0 + "</color> 완화";

        isUnLocked = false;
        fTime = TimeDuration;

        go_InUse.GetComponent<Button>().onClick.AddListener(delegate { PoppedUp_InUse(); });
    }

    public override void UpdateDataForWarmItems() // 받아온 데이터를 기반으로 값 배치
    {
        base.UpdateDataForWarmItems();
        string strObjectName = gameObject.name;
        UnlockCondition = m_savedData[strObjectName][1];
        Cost_Heart = float.Parse(m_savedData[strObjectName][2]);
        Cost_Coin = float.Parse(m_savedData[strObjectName][3]);
        Cost_Use = float.Parse(m_savedData[strObjectName][4]);
        TimeDuration = float.Parse(m_savedData[strObjectName][5]);
        IncreaseTemperature = float.Parse(m_savedData[strObjectName][6]);
        SoftenTemperature = float.Parse(m_savedData[strObjectName][7]);

        SetStageId();
    }
    public override void printData() // 유니티 디버그용
    {
        base.printData();
        Debug.Log("==============");
        int nCount = Enum.GetValues(typeof(ItemList)).Length;
        for (int i = 0; i < nCount; i++)
        {
            string strName = Enum.GetName(typeof(ItemList), i);
            Debug.Log("Name : " + m_savedData[strName]);
            Debug.Log("Unlock Condition : " + m_savedData[strName][1]);
            Debug.Log("Cost Heart : " + m_savedData[strName][2]);
            Debug.Log("Cost Coin : " + m_savedData[strName][3]);
            Debug.Log("Cost Use : " + m_savedData[strName][4]);
            Debug.Log("Time Duration : " + m_savedData[strName][5]);
            Debug.Log("Increase Temperature : " + m_savedData[strName][6]);
        }
        Debug.Log("==============");
    }

    public override void checkUnlockQualify() // 해금조건 확인 , 아이템 사용 준비
    {
        if ((int)Cost_Heart != -1)
        {
            if (MoneyManager.sharedInstance.nHeart >= (int)Cost_Heart)
            {
                go_UnlockItem.SetActive(false);
                go_UseItem.SetActive(true);

                ItemStatusManager.sharedInstance.WarmItem_Status_Level++;

                MoneyManager.sharedInstance.AddHeart(-(int)Cost_Heart);

                isPurchased = true;
            }
        }
        else
        {
            if (MoneyManager.sharedInstance.nHeart >= (int)Cost_Coin)
            {
                go_UnlockItem.SetActive(false);
                go_UseItem.SetActive(true);

                ItemStatusManager.sharedInstance.WarmItem_Status_Level++;

                MoneyManager.sharedInstance.AddCoin(-(int)Cost_Coin);

                isPurchased = true;
            }
        }
    }

    public override void TurnToUnlockItem() // "잠금" 풀기 조건
    {
        if (Stage_Id <= Penguri.sharedInstance.getStageId()) // Stage_Id == UnlockCondition의 정수형 값 -> 현재 아이템의 해금 조건 레벨 <= 펭귄의 현재 레벨 이 충족되어야함
        {
            isUnLocked = true;
            go_Lock.SetActive(false);
            go_UnlockItem.SetActive(true);
        }
    }

    public override void SetCost()
    {
        UnlockItem_CostText.text = Cost_Heart <= 0 ? Cost_Coin.ToString() : Cost_Heart.ToString();
        UseItem_CostText.text = Cost_Use.ToString();
    }

    public override void SetStageId()
    {
        base.SetStageId();
    }

    IEnumerator PauseDecreaseAndBeginTime() // 아이템 사용시 일정시간 온도 감소는 멈추며, 타이머는 흘러가기 시작함
    {
        Penguri.sharedInstance.setUsedWarmItem(true);
        while (fTime > 1f)
        {
            fTime -= Time.deltaTime;
            //go_Clock.fillAmount = 1-(1f / fTime);
            go_Clock.fillAmount = fTime / TimeDuration;
            ClockText.text = fTime.ToString("F0");
            ClockText.text = "체온 유지 시간 <color=#FFE400>" + fTime.ToString("F0") + "</color>, 초당 제온 감소 <color=#FFE400>" + Penguri.sharedInstance.getDecreaseTemperatureAmountPerSec() + "</color> 완화";
            yield return new WaitForFixedUpdate();
        }
        go_InUse.SetActive(false);
        go_UseItem.SetActive(true);
        isInUse = false;
        fTime = TimeDuration;
    }

    public override void UseItem()
    {
        if(Cost_Use <= MoneyManager.sharedInstance.nHeart)
        {
            Penguri.sharedInstance.ADD_GettingWarmUp(IncreaseTemperature, 1);
            MoneyManager.sharedInstance.UpdateMoney((int)Cost_Use,0);
            go_UseItem.SetActive(false);
            go_InUse.SetActive(true);
            isInUse = true;
            QuestHandler.sharedInstance.ProgressOfQuest();
            StartCoroutine(PauseDecreaseAndBeginTime());
        }
        else
        {
            Debug.Log("구매 실패");
        }
    }

    public void PoppedUp_InUse()
    {
        Instantiate(go_InUse_Popup, GameObject.Find("PopUp").transform);
    }

    private void FixedUpdate()
    {
        if (!isUnLocked)
            TurnToUnlockItem();

        if (isPurchased && !isInUse)
        {
            go_UnlockItem.SetActive(false);
            go_UseItem.SetActive(true);
        }
    }
}
