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
     * TypeName :: Money(Heart,Coin) / Buff(Boost) / Food(Shrimp �� 6��) / WarmItem(Cocoa �� 6��)  ��1) Money  ��2) Buff
     * ItemName :: ǥ���� -> Camel Case / ���İ� ������ ���� ������ �ٴ� Food,WarmItem,GlobalWarming���� ���� ���� ��) WarmItem_Cocoa (X) -> Cocoa (O)
     * Amount :: ���� ǥ��
     * 
     * ���� �� ���濡 ����� �����ͺ��̽� �����ϵ�, ���� ���(GoogleSheetManager)�� �ٸ��� �̰����� ���������� WebRequest���ؼ� �ҷ��ð�.
     */

    public void Reward_Attendance() // �⼮���� ó��
    {
        /*
         * Switch���� + �Լ��� ������ ������ ���� DB���� �� ��쿡 ����� �̸� ¥���� �Լ�.
         * ���� �ƹ��� ��ȹ�� �����Ƿ� ���� DB������� �������� �ȴٸ� ���ֵ� ����.
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
        print("Boost Ȱ��ȭ");
    }

    private void Reward_Items() // Food , WarmItem �� ���� Item.cs�� �ڽ�Ŭ������
    {
        // �κ��丮 �������ҵ�?
    }
}
