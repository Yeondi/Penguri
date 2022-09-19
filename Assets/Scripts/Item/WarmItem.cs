using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarmItem : Item
{
    void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();
        UpdateDataForWarmItems();
    }

    /*
 * Food
 * Unlock Condition / Unlock Cost(Heart/Coin) / Cost_Use / Increase Filling
 * 
 * Temperature
 * Unlock Condition / Unlock Cost(Heart/Coin) / Cost_Use / Increase Temerature / Time duration
 */

    public override void UpdateDataForWarmItems()
    {
        base.UpdateDataForWarmItems();
        string strObjectName = gameObject.name;
        UnlockCondition = m_SavedData[strObjectName][1];
        Cost_Heart = float.Parse(m_SavedData[strObjectName][2]);
        Cost_Coin = float.Parse(m_SavedData[strObjectName][3]);
        Cost_Use = float.Parse(m_SavedData[strObjectName][4]);
        TimeDuration = float.Parse(m_SavedData[strObjectName][5]);
        IncreaseTemperature = float.Parse(m_SavedData[strObjectName][6]);
        //if(gameObject.name == "HotCoco")
        //{
        //    UnlockCondition = m_SavedData[ItemList.Warm_Cocoa.ToString()][1];
        //    Cost_Heart = float.Parse(m_SavedData[ItemList.Warm_Cocoa.ToString()][2]);
        //    Cost_Coin = float.Parse(m_SavedData[ItemList.Warm_Cocoa.ToString()][3]);
        //    Cost_Use = float.Parse(m_SavedData[ItemList.Warm_Cocoa.ToString()][4]);
        //    TimeDuration = float.Parse(m_SavedData[ItemList.Warm_Cocoa.ToString()][5]);
        //    IncreaseTemperature = float.Parse(m_SavedData[ItemList.Warm_Cocoa.ToString()][6]);
        //}
        //else if(gameObject.name == "Cushion")
        //{

        //}
        //else if (gameObject.name == "Blanket")
        //{

        //}
        //else if (gameObject.name == "Heater")
        //{

        //}
        //else if (gameObject.name == "Muffler")
        //{

        //}
        //else if (gameObject.name == "Beanie")
        //{

        //}
    }
    public override void printData()
    {
        base.printData();
        Debug.Log("==============");
        int nCount = Enum.GetValues(typeof(ItemList)).Length;
        for (int i = 0; i < nCount; i++)
        {
            string strName = Enum.GetName(typeof(ItemList), i);
            Debug.Log("Name : " + m_SavedData[strName]);
            Debug.Log("Unlock Condition : " + m_SavedData[strName][1]);
            Debug.Log("Cost Heart : " + m_SavedData[strName][2]);
            Debug.Log("Cost Coin : " + m_SavedData[strName][3]);
            Debug.Log("Cost Use : " + m_SavedData[strName][4]);
            Debug.Log("Time Duration : " + m_SavedData[strName][5]);
            Debug.Log("Increase Temperature : " + m_SavedData[strName][6]);
        }
        Debug.Log("==============");
    }

}
