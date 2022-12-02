using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheatTable : MonoBehaviour
{

    public void DEBUG_BRIDGE(int nTypeNumber)
    {
        if (nTypeNumber == 0)
            MoneyManager.sharedInstance.DEBUG_ADDMoney();
        else if (nTypeNumber == 1)
            Penguri.sharedInstance.ToTheNextStep(Penguri.sharedInstance.getKey());
        else if (nTypeNumber == 2)
            UserDataManager.sharedInstance.SaveData();
    }
    public void ShowLeaderBoard_DEBUG()
    {
        Social.ShowLeaderboardUI();
    }
}
