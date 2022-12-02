using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tettt : MonoBehaviour
{
    public float m_DecreaseTemperatureAmountPerSec = 1f;
    public float bak_value_temp = 0f;

    private void Start()
    {
        StartCoroutine(StopDecreaseTemperature(10f));
    }
    public IEnumerator StopDecreaseTemperature(float __duration) // 초당 체온하강 멈춤
    {
        Debug.Log("체온 하강 멈춤");
        bak_value_temp = m_DecreaseTemperatureAmountPerSec;
        m_DecreaseTemperatureAmountPerSec = 0;
        yield return new WaitForSecondsRealtime(__duration);

        Debug.Log("체온 하강 재개");
        m_DecreaseTemperatureAmountPerSec = bak_value_temp;
    }
}
