using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PopUpController : MonoBehaviour
{
    public static PopUpController sharedInstance = null;

    public GameObject PopUpPrefab;

    GameObject m_This;

    [SerializeField]
    private Attendance m_Attendance;

    void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
            Destroy(gameObject);
        else
            sharedInstance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void PoppedUp()
    {
        initOpenItemWindow();
        m_This = Instantiate(PopUpPrefab, gameObject.transform);
        m_This.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(CallLambdaFunction);
    }

    void initOpenItemWindow()
    {
        GameObject TemporaryParentObject = gameObject.transform.parent.gameObject; // 부모정보 임시저장
        PopUpPrefab.transform.GetChild(0).GetComponent<TMP_Text>().text = "제목수정됨"; // 타이틀
        PopUpPrefab.transform.GetChild(2).GetComponent<TMP_Text>().text = TemporaryParentObject.transform.GetChild(1).GetComponent<TMP_Text>().text; // 이름
        if (TemporaryParentObject.tag == "Earth")
        {
            PopUpPrefab.transform.GetChild(1).GetComponent<Image>().sprite = TemporaryParentObject.transform.GetChild(1).GetComponent<Image>().sprite;
        }
        else
        {
            PopUpPrefab.transform.GetChild(1).GetComponent<Image>().sprite = TemporaryParentObject.transform.GetChild(0).GetComponent<Image>().sprite; // 스프라이트
        }
        PopUpPrefab.transform.GetChild(3).GetComponent<TMP_Text>().text = "설명";
    }
    public void CallLambdaFunction()
    {
        //m_MoneyManager.CheckQualification(this,PopUpPrefab.transform.GetChild(2).GetComponent<TMP_Text>().text);
    }

    public void ClosePopUp()
    {
        Destroy(m_This);
    }

    public void Attendance_Sequence(bool isNewbie = false)
    {
        m_Attendance.gameObject.SetActive(true);

        m_Attendance.Check_Attendance(isNewbie);

    }

    public Attendance GetAttendance()
    {
        return m_Attendance;
    }
}
