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
    GameObject Store; // Coin�� +ǥ��
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


    //���⿡ �߰�

    public void Switch_UI_HideButton(bool isHideOn) // UI ����� ��ư Ȱ��ȭ
    {
        if(!isHideOn) // ���� = ��Ȱ��ȭ = UI ����� ��� ����
        {
            Heart.SetActive(true);
            Coin.SetActive(true);
            Store.SetActive(true);
            Quest.SetActive(true);
            TabBar.SetActive(true);

            Hide_UI_On.SetActive(true);
            Hide_UI_Off.SetActive(false);

            Debug.Log("UI ����� ����");
        }
        else if(isHideOn) // UI ����� ��� Ȱ��ȭ
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
            Debug.Log("UI �����");
        }
    }
}
