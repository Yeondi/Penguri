using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideUI : MonoBehaviour
{
    [SerializeField]
    GameObject Heart;
    [SerializeField]
    GameObject Coin;
    [SerializeField]
    GameObject Store; // Coin옆 +표시
    [SerializeField]
    GameObject Quest;
    [SerializeField]
    GameObject TabBar;
    [SerializeField]
    GameObject Food;
    [SerializeField]
    GameObject Temp;
    [SerializeField]
    GameObject Globarwarming;


    [SerializeField]
    GameObject Hide_UI_On;
    [SerializeField]
    GameObject Hide_UI_Off;


    //여기에 추가

    public void Switch_UI_HideButton(bool isHideOn) // UI 숨기기 버튼 활성화
    {
        if(!isHideOn) // 거짓 = 비활성화 = UI 숨기기 모드 해제
        {
            Heart.SetActive(true);
            Coin.SetActive(true);
            Store.SetActive(true);
            Quest.SetActive(true);
            TabBar.SetActive(true);

            Hide_UI_On.SetActive(true);
            Hide_UI_Off.SetActive(false);

            Debug.Log("UI 숨기기 해제");
        }
        else if(isHideOn) // UI 숨기기 모드 활성화
        {
            Heart.SetActive(false);
            Coin.SetActive(false);
            Store.SetActive(false);
            Quest.SetActive(false);
            TabBar.SetActive(false);
            Food.SetActive(false);
            Temp.SetActive(false);
            Globarwarming.SetActive(false);

            Hide_UI_On.SetActive(false);
            Hide_UI_Off.SetActive(true);
            Debug.Log("UI 숨기기");
        }
    }
}
