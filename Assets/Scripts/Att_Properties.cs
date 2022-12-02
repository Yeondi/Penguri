using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Att_Properties : MonoBehaviour
{
    public int m_Day = 0;
    public string m_TypeName = "";
    public string m_ItemName = "";
    public int m_Amount = 0;

    /*
     * TypeName :: Money(Heart,Coin) / Buff(Boost) / Food(Shrimp 등 6종) / WarmItem(Cocoa 등 6종)  예1) Money  예2) Buff
     * ItemName :: 표기방식 -> Camel Case / 음식과 아이템 포함 기존에 붙던 Food,WarmItem,GlobalWarming등은 뺴고 기입 예) WarmItem_Cocoa (X) -> Cocoa (O)
     * Amount :: 정수 표기
     * 
     * 추후 값 변경에 대비해 데이터베이스 연결하되, 기존 방식(GoogleSheetManager)과 다르게 이곳에서 개별적으로 WebRequest통해서 불러올것.
     */

    public void Reward_Attendance() // 출석보상 처리
    {
        /*
         * Switch구문 + 함수를 고집한 이유는 차후 DB연결 될 경우에 대비해 미리 짜놓은 함수.
         * 현재 아무런 계획이 없으므로 만약 DB연결없이 고정값이 된다면 없애도 무방.
         */
        switch (m_Day)
        {
            case 1:
                Reward_Money();
                break;
            case 2:
                Reward_Buff();
                break;
            case 3:
                Reward_Items();
                break;
            case 4:
                Reward_Money();
                break;
            case 5:
                Reward_Items();
                break;
            case 6:
                Reward_Money();
                break;
            case 7:
                Reward_Money();
                break;
        }
    }

    private void Reward_Money()
    {
        if (m_ItemName == "Heart")
            MoneyManager.sharedInstance.AddHeart(m_Amount);
        else if (m_ItemName == "Coin")
            MoneyManager.sharedInstance.AddCoin(m_Amount);
    }

    private void Reward_Buff()
    {
        print("Boost 활성화");
    }

    private void Reward_Items() // Food , WarmItem 등 이하 Item.cs의 자식클래스들
    {
        // 인벤토리 만들어야할듯?
    }
}
