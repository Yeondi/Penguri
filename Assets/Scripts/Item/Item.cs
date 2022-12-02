using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Item : MonoBehaviour
{
    public string type = ""; // Food or WarmItem
    public string UnlockCondition = ""; // 잠금해제 조건
    public float Cost_Heart = 0f; // 잠금해제시 드는 하트
    public float Cost_Coin = 0f; // 잠금해제시 드는 코인
    public float Cost_Use = 0f; // 사용시 드는 하트
    public float TimeDuration = 0f; // 쿨타임
    public float IncreaseTemperature = 0f; // 온도 증감값
    public float IncreaseFillings = 0f; // 허기 증감값
    public float SoftenTemperature = 0f; // 온도완화값

    public bool isUnLocked = false;   // 잠김여부
    public bool isPurchased = false; // 해금여부 -> true는 구매가능상태

    [SerializeField]
    public GameObject go_UnlockItem;
    [SerializeField]
    public TMP_Text UnlockItem_CostText;

    [SerializeField]
    public GameObject go_Lock;
    [SerializeField]
    public GameObject go_UseItem;
    [SerializeField]
    public TMP_Text UseItem_CostText;
    [SerializeField]
    public GameObject go_InUse;

    [SerializeField]
    public GameObject go_InUse_Popup;

    protected Dictionary<string, List<string>> m_savedData;

    protected int Stage_Id = 0;

    /*
     * Food
     * Unlock Condition / Unlock Cost(Heart/Coin) / Cost_Use / Increase Filling
     * 
     * Temperature
     * Unlock Condition / Unlock Cost(Heart/Coin) / Cost_Use / Increase Temerature / Time duration
     */

    public virtual void init()
    {
        m_savedData = GameManager.sharedInstance.getTsvData();
    }

    public virtual void UpdateDataForFoods()
    {
        type = "Food";
    }

    public virtual void UpdateDataForWarmItems()
    {
        type = "WarmItem";
    }

    public virtual void UpdateDataForGlobalwarming()
    {
        type = "Globalwarming";
    }

    public virtual void printData()
    {

    }

    public virtual void UseItem()
    {
    }

    public virtual void checkUnlockQualify() // UseItem 활성화 조건체크
    {

    }

    public virtual void TurnToUnlockItem() // 잠김 해제 -> 구매가능한 상태로 전환
    {

    }

    public virtual void SetCost() // 가격 세팅
    {

    }

    public virtual void DisplayOn_CostUse()
    {
        UseItem_CostText.text = Cost_Use.ToString();
    }

    public virtual void SetStageId() // 언제 해금되는가?
    {
        switch (UnlockCondition)
        {
            case "EggStage":
                Stage_Id = 0;
                break;
            case "BabyStage1":
                Stage_Id = 1;
                break;
            case "BabyStage2":
                Stage_Id = 2;
                break;
            case "GrowthStage1":
                Stage_Id = 3;
                break;
            case "GrowthStage2":
                Stage_Id = 4;
                break;
            case "AdultStage":
                Stage_Id = 5;
                break;
        }
    }

    protected enum ItemList
    {
        Food_Shrimp,
        Food_Sardine,
        Food_SmallOctopus,
        Food_Squid,
        Food_Octopus,
        Food_Mackerel,
        Warm_Cocoa,
        Warm_Handwarmer,
        Warm_Cushion,
        Warm_Beanie,
        Warm_Muffler,
        Warm_Cape,
        Diet,
        Recycling,
        Energy,
        RenewableEnergy,
        Transport,
        Industry,
        MarineProtection,
        ForestAndSoil
    }

    //protected enum StageList
    //{
    //    EggStage,
    //    BabyStage1,
    //    BabyStage2,
    //    GrowthStage1,
    //    GrowthStage2,
    //    AdultStage,
    //}

}
