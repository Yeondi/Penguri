using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Item : MonoBehaviour
{
    public string type = ""; // Food or WarmItem
    public string UnlockCondition = ""; // ������� ����
    public float Cost_Heart = 0f; // ��������� ��� ��Ʈ
    public float Cost_Coin = 0f; // ��������� ��� ����
    public float Cost_Use = 0f; // ���� ��� ��Ʈ
    public float TimeDuration = 0f; // ��Ÿ��
    public float IncreaseTemperature = 0f; // �µ� ������
    public float IncreaseFillings = 0f; // ��� ������
    public float SoftenTemperature = 0f; // �µ���ȭ��

    public bool isUnLocked = false;   // ��迩��
    public bool isPurchased = false; // �رݿ��� -> true�� ���Ű��ɻ���

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

    public virtual void checkUnlockQualify() // UseItem Ȱ��ȭ ����üũ
    {

    }

    public virtual void TurnToUnlockItem() // ��� ���� -> ���Ű����� ���·� ��ȯ
    {

    }

    public virtual void SetCost() // ���� ����
    {

    }

    public virtual void DisplayOn_CostUse()
    {
        UseItem_CostText.text = Cost_Use.ToString();
    }

    public virtual void SetStageId() // ���� �رݵǴ°�?
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
