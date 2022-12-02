using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attendance : MonoBehaviour
{
    [SerializeField]
    GameObject[] Att_List;

    public int Day_Attendance;

    public void Check_Attendance(bool isNewbie = false)
    {
        Day_Attendance++;
        if (isNewbie)
        {
            //SetActive_CheckMark(Att_List[0]);
            MoneyManager.sharedInstance.AddHeart(5000); // ���� 1���� ���� ��Ʈ 5000��, �ٲ� �� ����ؼ� �������� �ƴ� ���������� ��ü�ؾ���.
        }
        else
        {
            Att_List[Day_Attendance-1].GetComponent<Att_Properties>().Reward_Attendance();
        }


        SetActive_CheckMark(Att_List, Day_Attendance);
    }

    void SetActive_CheckMark(GameObject[] gos, int __Day_Attendance)
    {
        for (int i = 0; i < __Day_Attendance; i++)
        {
            gos[i].transform.GetChild(2).gameObject.SetActive(true);
        }
    }
}
