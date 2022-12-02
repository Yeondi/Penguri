using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    public static Reward sharedInstance = null;

    Dictionary<string, List<string>> m_savedData;

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
            Destroy(gameObject);
        else
            sharedInstance = this;

        DontDestroyOnLoad(gameObject);

        StartCoroutine(Async_Load_SavedData());
    }

    IEnumerator Async_Load_SavedData() // 비동기 데이터 로드
    {
        try
        {
            m_savedData = GameManager.sharedInstance.getTsvData();
            StopCoroutine(Async_Load_SavedData());
        }
        catch (System.Exception e)
        { }
        yield return null;
    }

    public void GetReward(string __Type, string __Amount) // 완료보상 타입 , 수
    {
        int amount = 0;
        try
        {
            try
            {
                amount = int.Parse(__Amount);
            }
            catch(System.Exception e)
            {
                amount = (int)float.Parse(__Amount);
            }
        }
        catch(System.Exception e)
        {
            amount = 9999;
        }

        //if (__Amount.GetType().Name == "Int32")
        //    amount = int.Parse(__Amount);
        //else if (__Amount.GetType().Name == "Float")
        //    amount = (int)float.Parse(__Amount);
        //else if (__Amount == "Max")
        //    amount = 9999;
        //else
        //    Debug.LogWarning("GetReward Else Value");

        switch(__Type)
        {
            case "Temperature":
                Penguri.sharedInstance.ADD_GettingWarmUp(amount, 1);
                break;
            case "DEBUG__HEART":
                MoneyManager.sharedInstance.AddHeart(amount);
                break;
            case "DEBUG__COIN":
                MoneyManager.sharedInstance.AddCoin(amount);
                break;
            case "DEBUG_HUNGER":
                Penguri.sharedInstance.ADD_FillHunger(amount, 1);
                break;
            case "* 튜토리얼 상태에만 추가되는 날씨이벤트":
                Debug.Log("비가 내립니다. Reward보상효과");
                break;
            default:
                Debug.Log("Reward.cs - GetReward() - default");
                break;
        }
    }
}
