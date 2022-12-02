using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Food : Item
{
    int amount = 1;

    int origin_price = 0;

    private void Start()
    {
        init();
        origin_price = (int)Cost_Use;
    }
    public override void init()
    {
        base.init();
        UpdateDataForFoods();

        SetCost();
    }

    public override void UpdateDataForFoods()
    {
        base.UpdateDataForFoods();
        try
        {
            UnlockCondition = m_savedData[gameObject.name][1];
            Cost_Heart = float.Parse(m_savedData[gameObject.name][2]);
            Cost_Coin = float.Parse(m_savedData[gameObject.name][3]);
            Cost_Use = float.Parse(m_savedData[gameObject.name][4]);
            IncreaseFillings = float.Parse(m_savedData[gameObject.name][5]);
            SetStageId();
        }
        catch (NullReferenceException)
        { }
        //printData();
    }

    public void changeAmount(int __Amount)
    {
        amount = __Amount;
        UseItem_CostText.text = (origin_price * __Amount).ToString();
    }

    public override void printData()
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
            Debug.Log("Increase Fillings : " + m_savedData[strName][5]);
        }
        Debug.Log("==============");
    }

    public override void UseItem()
    {
        if (Cost_Use <= MoneyManager.sharedInstance.nHeart)
        {
            Penguri.sharedInstance.ADD_FillHunger(IncreaseFillings, amount);
            MoneyManager.sharedInstance.UpdateMoney(origin_price, 0, amount);

            if (QuestHandler.sharedInstance.getEventType1() == "Food" && QuestHandler.sharedInstance.getEventType2() == (gameObject.name).Replace("Food_",""))
                QuestHandler.sharedInstance.ProgressOfQuest();
        }
        else
        {
            Debug.Log("구매 실패");
        }
    }

    public override void TurnToUnlockItem()
    {
        if (Stage_Id <= Penguri.sharedInstance.getStageId()) // Stage_Id == UnlockCondition의 정수형 값 -> 현재 아이템의 해금 조건 레벨 <= 펭귄의 현재 레벨 이 충족되어야함
        {
            isUnLocked = true;
            go_Lock.SetActive(false);
            go_UnlockItem.SetActive(true);
            Debug.Log(gameObject.name + " 이제부터 구매 가능합니다.");
        }
    }

    public override void checkUnlockQualify()
    {
        if ((int)Cost_Heart != -1)
        {
            if (MoneyManager.sharedInstance.nHeart >= (int)Cost_Heart)
            {
                go_UnlockItem.SetActive(false);
                go_UseItem.SetActive(true);

                ItemStatusManager.sharedInstance.Food_Status_Level++;

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

                ItemStatusManager.sharedInstance.Food_Status_Level++;

                MoneyManager.sharedInstance.AddCoin(-(int)Cost_Coin);

                isPurchased = true;
            }
        }
    }

    public override void SetStageId()
    {
        base.SetStageId();
    }

    public override void SetCost()
    {
        UnlockItem_CostText.text = Cost_Heart <= 0 ? Cost_Coin.ToString() : Cost_Heart.ToString();
        UseItem_CostText.text = Cost_Use.ToString();
    }
    private void FixedUpdate()
    {
        if (!isUnLocked)
            TurnToUnlockItem();

        if (isPurchased)
        {
            go_UnlockItem.SetActive(false);
            go_UseItem.SetActive(true);
        }
    }
}
