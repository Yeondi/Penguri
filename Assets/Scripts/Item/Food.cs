using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Food : Item
{
    private void Start()
    {
        init();

    }
    public override void init()
    {
        base.init();
        UpdateDataForFoods();
    }

    public override void UpdateDataForFoods()
    {
        base.UpdateDataForFoods();
        if(gameObject.name == "Shrimp")
        {
            UnlockCondition = m_SavedData[ItemList.Food_Shrimp.ToString()][1];
            Cost_Heart = float.Parse( m_SavedData[ItemList.Food_Shrimp.ToString()][2]);
            Cost_Use = float.Parse(m_SavedData[ItemList.Food_Shrimp.ToString()][4]);
            IncreaseFillings = float.Parse(m_SavedData[ItemList.Food_Shrimp.ToString()][5]);
        }
        else if(gameObject.name == "Sardine")
        {
            UnlockCondition = m_SavedData[ItemList.Food_Sardine.ToString()][1];
            Cost_Heart = float.Parse(m_SavedData[ItemList.Food_Sardine.ToString()][2]);
            Cost_Use = float.Parse(m_SavedData[ItemList.Food_Sardine.ToString()][4]);
            IncreaseFillings = float.Parse(m_SavedData[ItemList.Food_Sardine.ToString()][5]);
        }
        else if (gameObject.name == "SmallOctopus")
        {
            UnlockCondition = m_SavedData[ItemList.Food_SmallOctopus.ToString()][1];
            Cost_Heart = float.Parse(m_SavedData[ItemList.Food_SmallOctopus.ToString()][2]);
            Cost_Use = float.Parse(m_SavedData[ItemList.Food_SmallOctopus.ToString()][4]);
            IncreaseFillings = float.Parse(m_SavedData[ItemList.Food_SmallOctopus.ToString()][5]);
        }
        else if (gameObject.name == "Squid")
        {
            UnlockCondition = m_SavedData[ItemList.Food_Squid.ToString()][1];
            Cost_Coin = float.Parse(m_SavedData[ItemList.Food_Squid.ToString()][3]);
            Cost_Use = float.Parse(m_SavedData[ItemList.Food_Squid.ToString()][4]);
            IncreaseFillings = float.Parse(m_SavedData[ItemList.Food_Squid.ToString()][5]);
        }
        else if (gameObject.name == "Octopus")
        {
            UnlockCondition = m_SavedData[ItemList.Food_Octopus.ToString()][1];
            Cost_Coin = float.Parse(m_SavedData[ItemList.Food_Octopus.ToString()][3]);
            Cost_Use = float.Parse(m_SavedData[ItemList.Food_Octopus.ToString()][4]);
            IncreaseFillings = float.Parse(m_SavedData[ItemList.Food_Octopus.ToString()][5]);
        }
        else if (gameObject.name == "Mackerel")
        {
            UnlockCondition = m_SavedData[ItemList.Food_Mackerel.ToString()][1];
            Cost_Coin = float.Parse(m_SavedData[ItemList.Food_Mackerel.ToString()][3]);
            Cost_Use = float.Parse(m_SavedData[ItemList.Food_Mackerel.ToString()][4]);
            IncreaseFillings = float.Parse(m_SavedData[ItemList.Food_Mackerel.ToString()][5]);
        }
        //printData();
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
            Debug.Log("Increase Fillings : " + m_SavedData[strName][5]);
        }
        Debug.Log("==============");
    }
}
