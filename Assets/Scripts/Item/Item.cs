using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : MonoBehaviour
{
    public string type = "";
    public string UnlockCondition = "";
    public float Cost_Heart = 0f;
    public float Cost_Coin = 0f;
    public float Cost_Use = 0f;
    public float TimeDuration = 0f;
    public float IncreaseTemperature = 0f;
    public float IncreaseFillings = 0f;
    public bool isPurchased = false;

    protected Dictionary<string, List<string>> m_SavedData;

    /*
     * Food
     * Unlock Condition / Unlock Cost(Heart/Coin) / Cost_Use / Increase Filling
     * 
     * Temperature
     * Unlock Condition / Unlock Cost(Heart/Coin) / Cost_Use / Increase Temerature / Time duration
     */

    public virtual void init()
    {
        m_SavedData = GameManager.sharedInstance.getTsvData();
    }

    public virtual void UpdateDataForFoods()
    {
        type = "Food";
    }

    public virtual void UpdateDataForWarmItems()
    {
        type = "WarmItem";
    }

    public virtual void printData()
    {

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
        Warm_Cushion,
        Warm_Blanket,
        Warm_Heater,
        Warm_Muffler,
        Warm_Beanie,
    }
}
