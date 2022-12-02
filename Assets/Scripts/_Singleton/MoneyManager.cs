using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    [SerializeField]
    GameObject heart;
    [SerializeField]
    TMP_Text heart_text;

    [SerializeField]
    GameObject coin;
    [SerializeField]
    TMP_Text coin_text;

    public int nHeart = 0;
    public int nCoin = 0;
    public bool DEBUG_update = false;

    public static MoneyManager sharedInstance = null;

    int max_Heart = 1000000000; // 10��
    int max_coin = 1000000; // 100��

    private void Start()
    {
        if (sharedInstance != null && sharedInstance != this)
            Destroy(gameObject);
        else
            sharedInstance = this;

        init();

    }
    void init()
    {
        DisplayOn();
        StartCoroutine(DisplayOn_Coroutine());
    }

    public void OnLoadDataBase()
    {

    }

    private void FixedUpdate()
    {
        DisplayOn();
    }

    public void UpdateMoney(int __heart = 0,int __coin =0,int __Amount = 1) // �� ����
    {
        if (nHeart - __heart >= 0)
        {
            nHeart -= __heart * __Amount;
        }
        if(nCoin - __coin >= 0)
        { 
            nCoin -= __coin * __Amount;
        }
        DisplayOn();
    }
    public void DisplayOn() // �ٲ� �� �ð� ����
    {
        // ���� DB����� ����
        //if (!heart || !coin)
        //{
        //    ReassignData();
        //}
        heart_text.text = SeperateMoneyTextByComma(nHeart);
        coin_text.text = SeperateMoneyTextByComma(nCoin);
    }

    public IEnumerator DisplayOn_Coroutine()
    {
        while(true)
        {
            DisplayOn();
            yield return new WaitForSeconds(10f);
            StartCoroutine(DisplayOn_Coroutine());
        }
    }

    string SeperateMoneyTextByComma(int nData) // ����ǥ��
    {
        return string.Format("{0:#,##0}", nData);
    }

    public void AddHeart(int nAmount) // ��Ʈ �߰�
    {
        if (nHeart + nAmount < max_Heart)
        {
            nHeart += nAmount;
            if (QuestHandler.sharedInstance.getEventType1() == "Money" && QuestHandler.sharedInstance.getEventType2() == "Heart")
            {
                Broadcast.sharedInstance.Notify("Money", "Heart",Penguri.sharedInstance.getAddHeartAmount());
            }
        }

        DisplayOn();
    }

    public void AddCoin(int nAmount) // ���� �߰�
    {
        if (nCoin + nAmount < max_coin)
            nCoin += nAmount;
        DisplayOn();
    }

    public void LoadData(int __heart,int __coin) // �ε�� ��ȭ �ҷ�����
    {
        nHeart = __heart;
        nCoin = __coin;
    }

    public void DEBUG_ADDMoney()
    {
        nHeart += 100000;
        nCoin += 5000;

        DisplayOn();
    }
}
