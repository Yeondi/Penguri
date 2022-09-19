using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    GameObject heart;
    GameObject coin;

    public int nHeart = 0;
    public int nCoin = 0;
    public bool DEBUG_update = false;

    public static MoneyManager instance = null;

    private void Start()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        ReassignData();

        init();
    }

    void init()
    {
        DisplayOn();
    }

    public void OnLoadDataBase()
    {

    }

    void ReassignData()
    {
        heart = GameObject.FindGameObjectWithTag("money_heart");
        coin = GameObject.FindGameObjectWithTag("money_coin");
    }

    void DisplayOn()
    {
        // 차후 DB연결시 수정
        if (!heart || !coin)
        {
            ReassignData();
        }
        heart.GetComponent<TMP_Text>().text = SeperateMoneyTextByComma(nHeart);
        coin.GetComponent<TMP_Text>().text = SeperateMoneyTextByComma(nCoin);
    }

    string SeperateMoneyTextByComma(int nData)
    {
        return string.Format("{0:#,##0}", nData);
    }

    public void AddHeart(int nAmount)
    {
        nHeart += nAmount;
        Debug.Log("클릭됨");
        DisplayOn();
    }

    private void Update()
    {
        if (DEBUG_update)
        {
            DisplayOn();
            DEBUG_update = false;
        }

        Debug.Log("현재 하트 : " + nHeart);
        Debug.Log("현재 코인 : " + nCoin);
    }

    public void DEBUG_ADDMoney()
    {
        nHeart += 100000;
        nCoin += 5000;

        DisplayOn();
    }

    public void CheckQualification(PopUpController pop, string _Name)
    {
        bool isTrue = false;
        if (_Name == "크릴 새우")
        {
            if (nHeart >= 1000)
            {
                nHeart -= 1000;
                isTrue = true;
            }
            else
                Debug.Log("돈이 부족합니다. " + _Name);
        }
        else if (_Name == "정어리")
        {
            if (nHeart >= 4500)
            {
                nHeart -= 4500;
                isTrue = true;
            }
            else
                Debug.Log("돈이 부족합니다. " + _Name);
        }
        else if (_Name == "낙지")
        {
            if (nHeart >= 17000)
            {
                nHeart -= 17000;
                isTrue = true;
            }
            else
                Debug.Log("돈이 부족합니다. " + _Name);
        }
        else if (_Name == "오징어")
        {
            if (nCoin >= 300)
            {
                nCoin -= 300;
                isTrue = true;
            }
            else
                Debug.Log("돈이 부족합니다. " + _Name);
        }
        else if (_Name == "문어")
        {
            if (nCoin >= 400)
            {
                nCoin -= 400;
                isTrue = true;
            }
            else
                Debug.Log("돈이 부족합니다. " + _Name);
        }
        else if (_Name == "고등어")
        {
            if (nCoin >= 600)
            { 
                nCoin -= 600;
                isTrue = true;
            }
            else
                Debug.Log("돈이 부족합니다. " + _Name);
        }
        else if (_Name == "코코아")
        {
            if (nHeart >= 1500)
            {
                nHeart -= 1500;
                isTrue = true;
            }
            else
                Debug.Log("돈이 부족합니다. " + _Name);
        }
        else if (_Name == "방석")
        {
            if (nHeart >= 6800)
            {
                nHeart -= 6800;
                isTrue = true;
            }
            else
                Debug.Log("돈이 부족합니다. " + _Name);
        }
        else if (_Name == "담요")
        {
            if (nHeart >= 31500)
            {
                nHeart -= 31500;
                isTrue = true;
            }
            else
                Debug.Log("돈이 부족합니다. " + _Name);
        }
        else if (_Name == "난로")
        {
            if (nCoin >= 1000)
            {
                nCoin -= 1000;
                isTrue = true;
            }
            else
                Debug.Log("돈이 부족합니다. " + _Name);
        }
        else if (_Name == "목도리")
        {
            if (nCoin >= 1600)
            {
                nCoin -= 1600;
                isTrue = true;
            }
            else
                Debug.Log("돈이 부족합니다. " + _Name);
        }
        else if (_Name == "뜨개모자")
        {
            if (nCoin >= 2800)
            {
                nCoin -= 2800;
                isTrue = true;
            }
            else
                Debug.Log("돈이 부족합니다. " + _Name);
        }

        if(isTrue)
        {
            //Popup Close Code
            pop.ClosePopUp();
            pop.transform.parent.GetComponentInChildren<DragEvent>().setIsOn(true);
            pop.gameObject.SetActive(false); //해금하기 버튼 숨기기
            pop.transform.parent.GetChild(2).gameObject.SetActive(true); //능력치표 보이기
            if (pop.transform.parent.GetComponent<Food>())
                pop.transform.parent.GetComponent<Food>().isPurchased = true;
            else if (pop.transform.parent.GetComponent<WarmItem>())
                pop.transform.parent.GetComponent<WarmItem>().isPurchased = true;
        }

        DisplayOn();
    }

}
