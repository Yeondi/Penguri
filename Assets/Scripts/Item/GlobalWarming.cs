using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalWarming : Item
{
    [SerializeField]
    int m_level;

    float m_SpecialEffect; // [4]�� �����ϴ� ȿ��

    //int m_IncreaseHeartByTouch;
    //float m_IncreaseTemperaturByTouch;
    //float m_TemperaturePreservation;
    //float m_SatietyPreservation;
    //float m_DecreaseBlizzardChance;
    //float m_GainHeartPer10seconds;
    //float m_GainSatietyPerHour;
    //float m_GainTemperaturePerHour;

    public static readonly WaitForSeconds m_waitForSeconds = new WaitForSeconds(0.5f);

    int m_UniqueId = 1; // Key�� �ѹ���

    bool isActive = false;

    private void Start()
    {
        init();

        isActive = true;
    }

    public override void init()
    {
        base.init();
        UpdateDataForGlobalwarming();

        SetCost();



    }

    public override void UpdateDataForGlobalwarming()
    {
        base.UpdateDataForGlobalwarming();
        try
        {
            var temp = gameObject.name + m_UniqueId.ToString(); // Diet + 1
            UnlockCondition = m_savedData[temp][1];
            Cost_Heart = float.Parse(m_savedData[temp][2]);
            //m_level = int.Parse(m_savedData[temp][3]);
            m_level = 0;
            m_SpecialEffect = float.Parse(m_savedData[temp][4]);
            Cost_Use = float.Parse(m_savedData[temp][5]);

            SetStageId();
            //����� �߰�
        }
        catch(System.Exception e)
        { }
    }

    public override void checkUnlockQualify()
    {
        print(this.name + "CheckUnlockQualify");

        if(MoneyManager.sharedInstance.nHeart >= (int)Cost_Heart)
        {
            go_UnlockItem.SetActive(false);
            go_UseItem.SetActive(true);

            MoneyManager.sharedInstance.UpdateMoney((int)Cost_Heart);

            isPurchased = true;
            m_level++;
        }
    }

    public override void TurnToUnlockItem()
    {
        if(Stage_Id <= Penguri.sharedInstance.getStageId() && m_level > 0)
        {
            isUnLocked = true;
            go_Lock.SetActive(false);
            go_UnlockItem.SetActive(true);
            //go_UseItem.SetActive(true);
        }
    }

    public override void UseItem()
    {
        if(Cost_Use <= MoneyManager.sharedInstance.nHeart)
        {
            m_level++;
            m_UniqueId++;

            if (gameObject.name == "Diet")
            {
                Penguri.sharedInstance.setAddHeartAmount((int)m_SpecialEffect); // ��ġ�� ��Ʈ����
                m_SpecialEffect = int.Parse(m_savedData[gameObject.name + m_UniqueId.ToString()][4]);
            }
            else if (gameObject.name == "Recycling")
            {
                Penguri.sharedInstance.setIncreaseTemperatureByTouch(m_SpecialEffect);
                m_SpecialEffect = float.Parse(m_savedData[gameObject.name + m_UniqueId.ToString()][4]);
            }
            else if (gameObject.name == "Energy")
            {
                Penguri.sharedInstance.setPreservationOfStat(m_SpecialEffect);
                m_SpecialEffect = float.Parse(m_savedData[gameObject.name + m_UniqueId.ToString()][4]);
            }
            else if (gameObject.name == "RenewableEnergy")
            {
                Penguri.sharedInstance.setPreservationOfStat(0f, m_SpecialEffect);
                m_SpecialEffect = float.Parse(m_savedData[gameObject.name + m_UniqueId.ToString()][4]);
            }
            else if (gameObject.name == "Transport")
            {
                Debug.Log("���ڵ� Ȯ�� ����");
                m_SpecialEffect = float.Parse(m_savedData[gameObject.name + m_UniqueId.ToString()][4]);
            }
            else if (gameObject.name == "Industry")
            {
                Debug.Log("�������ν� ��Ʈ �ڵ� ȹ��");
                m_SpecialEffect = float.Parse(m_savedData[gameObject.name + m_UniqueId.ToString()][4]);
            }
            else if (gameObject.name == "MarineProtection")
            {
                Debug.Log("�������ν� �������� �ð����� �ڵ� ȸ��");
                m_SpecialEffect = float.Parse(m_savedData[gameObject.name + m_UniqueId.ToString()][4]);
            }
            else if (gameObject.name == "ForestAndSoil")
            {
                Debug.Log("�������ν� ü���� �ð����� �ڵ� ȸ��");
                m_SpecialEffect = float.Parse(m_savedData[gameObject.name + m_UniqueId.ToString()][4]);
            }

            Cost_Use = float.Parse(m_savedData[gameObject.name + m_UniqueId.ToString()][5]); // Level Up Cost

            MoneyManager.sharedInstance.UpdateMoney((int)Cost_Use, 0);

            DisplayOn_CostUse();
        }
        else
        {
            Debug.Log("���� ����");
        }
    }

    public IEnumerator LoadData(int _level)
    {
        while (true)
        {
            if (isActive)
            {
                m_UniqueId = _level;
                var temp = gameObject.name + m_UniqueId.ToString();
                UnlockCondition = m_savedData[temp][1];
                Cost_Heart = float.Parse(m_savedData[temp][2]);
                m_level = int.Parse(m_savedData[temp][3]);
                m_SpecialEffect = float.Parse(m_savedData[temp][4]);
                Cost_Use = float.Parse(m_savedData[temp][5]);

                SetCost();
                break;
            }
            else if (!isActive)
            {
                yield return m_waitForSeconds;
            }

            Debug.Log("�������µ� ����");
        }
    }

    public override void SetCost()
    {
        UnlockItem_CostText.text = Cost_Heart <= 0 ? Cost_Coin.ToString() : Cost_Heart.ToString();
        UseItem_CostText.text = Cost_Use.ToString();
    }

    public override void SetStageId()
    {
        base.SetStageId();
    }

    public void setSpecialEffect(float _data)
    {
        m_SpecialEffect = _data;
    }

    public float getSpecialEffect()
    {
        return m_SpecialEffect;
    }

    public void setLevel(int __level)
    {
        m_level = __level;
    }

    public int getLevel()
    {
        return m_level;
    }

    private void FixedUpdate()
    {
        if (!isUnLocked)
            TurnToUnlockItem();

        if (isPurchased)
        {
            go_UnlockItem.SetActive(false);
            go_UseItem.SetActive(true);
        }
    }
}
