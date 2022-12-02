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
            MoneyManager.sharedInstance.AddHeart(5000); // 현재 1일차 보상 하트 5000개, 바뀔 시 대비해서 고정값이 아닌 변수값으로 대체해야함.
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
