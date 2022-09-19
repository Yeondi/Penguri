using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PopUpController : MonoBehaviour
{
    public GameObject PopUpPrefab;
    private GameObject PopUpWindow;

    private MoneyManager m_MoneyManager;

    GameObject m_This;

    void Start()
    {
        PopUpWindow = GameObject.FindGameObjectWithTag("PopUpWindow");
        m_MoneyManager = GameObject.Find("MoneyManager(Clone)").GetComponent<MoneyManager>();
    }

    public void PoppedUp()
    {
        initOpenItemWindow();
        m_This = Instantiate(PopUpPrefab, PopUpWindow.transform);
        m_This.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(CallLambdaFunction);
    }

    void initOpenItemWindow()
    {
        GameObject TemporaryParentObject = gameObject.transform.parent.gameObject; // �θ����� �ӽ�����
        PopUpPrefab.transform.GetChild(0).GetComponent<TMP_Text>().text = "���������"; // Ÿ��Ʋ
        PopUpPrefab.transform.GetChild(2).GetComponent<TMP_Text>().text = TemporaryParentObject.transform.GetChild(1).GetComponent<TMP_Text>().text; // �̸�
        if (TemporaryParentObject.tag == "Earth")
        {
            PopUpPrefab.transform.GetChild(1).GetComponent<Image>().sprite = TemporaryParentObject.transform.GetChild(1).GetComponent<Image>().sprite;
        }
        else
        {
            PopUpPrefab.transform.GetChild(1).GetComponent<Image>().sprite = TemporaryParentObject.transform.GetChild(0).GetComponent<Image>().sprite; // ��������Ʈ
        }
        PopUpPrefab.transform.GetChild(3).GetComponent<TMP_Text>().text = "����";


    }

    public void CallLambdaFunction()
    {
        m_MoneyManager.CheckQualification(this,PopUpPrefab.transform.GetChild(2).GetComponent<TMP_Text>().text);
    }

    public void ClosePopUp()
    { 
        Destroy(m_This);
    }
}
