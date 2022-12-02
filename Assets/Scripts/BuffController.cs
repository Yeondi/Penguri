#define __DEBUG__
//#define __Release__
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    //게임매니저에서 불러오는 방식
    //활성조건 true -> Penguri call this
    string m_Name; // starve
    string m_NameToBeMarked;

    //Anime?
    [SerializeField]
    GameObject m_Happiness;
    [SerializeField]
    GameObject m_FoodBaby;
    [SerializeField]
    GameObject m_PassOut;
    [SerializeField]
    GameObject m_Starve;
    [SerializeField]
    GameObject m_LowTemp;
    [SerializeField]
    GameObject m_Sickness;

    [SerializeField]
    int m_HypothermiaLevel = 0;

    public float m_Weight = 1f;

    public bool isStarve = false;
    public bool isPassOut = false;
    public bool isHappiness = false;
    public bool isSickness = false;
    public bool isDebug = false;
    public bool isFull = false;
    public float fDebug = 10.0f;

    public bool isOnSicknessDEBUG = false;

    ItemStatusManager ism;

    public float m_FullTime = 10f;

    private void Start()
    {
        ism = ItemStatusManager.sharedInstance;
    }

    float fBasic = 1.0f;

    float fStarve = 0f;
    float fPassOut = 0f;
    float fHappiness = 0f;

    float extraCharge = 0f;

    private void FixedUpdate()
    {
        Debug.Log("배고픔 : " + fStarve + "\n저체온증 : " + m_HypothermiaLevel * -0.1f + "\n기절 : " + fPassOut
            + "\n행복함 : " + fHappiness);

        if (isOnSicknessDEBUG)
            Sickness(true);

        //만약 특정 아이템 제한 조건(예: sickness)가 있다면, 여기서 계속 제한


#if __DEBUG__
        if (isHappiness)
            fHappiness = 0.2f;
        if (isPassOut)
            fPassOut = -0.4f;
        if (isStarve)
            fStarve = -0.5f;
        if (isDebug)
            fHappiness = fDebug;
#endif

        if (isSickness)
        {
            ism.SwitchValue_Cocoa();
            ism.SwitchValue_Cushion(true);
            ism.SwitchValue_Handwarmer(true);
            ism.SwitchValue_Cape(true);
            ism.SwitchValue_Muffler();
            ism.SwitchValue_Beanie();

            ism.SwitchValue_Shrimp(true);
            ism.SwitchValue_Sardine();
            ism.SwitchValue_SmallOctopus();
            ism.SwitchValue_Octopus();
            ism.SwitchValue_Squid();
            ism.SwitchValue_Mackerel();
        }

        if (isPassOut)
        {
            ism.SwitchValue_Cocoa();
            ism.SwitchValue_Cushion(true);
            ism.SwitchValue_Handwarmer(true);
            ism.SwitchValue_Cape(true);
            ism.SwitchValue_Muffler();
            ism.SwitchValue_Beanie();

            ism.SwitchValue_Shrimp();
            ism.SwitchValue_Sardine();
            ism.SwitchValue_SmallOctopus();
            ism.SwitchValue_Octopus();
            ism.SwitchValue_Squid();
            ism.SwitchValue_Mackerel();
        }
    }

    public void buff()
    {
        m_Weight = fBasic + (fStarve + (m_HypothermiaLevel * -10f) + fPassOut + fHappiness) * extraCharge;
        //최대 증가/감소 효과값 조절 장치 필요
    }

    public void Starve(float _Percentage) // 굶주림 :: 최종 터치 n% 감소
    {
        //Hunger 30%이하
        if (_Percentage > 0.5f)
        {
            m_Starve.SetActive(false);
            fStarve = 0f;
            buff();
            return;
        }
        m_Starve.SetActive(true);
        fStarve = -0.5f;
        buff();
    }//Hunger 50%이상시 해제

    public void Hypothermia(float _Temperature) // 저체온증 :: n도 이하 최종터치 m% 감소
    {
        if (_Temperature <= 10.0f)
        {//기절
            PassOut(true);
        }
        else if (_Temperature > 18f && isPassOut)
        {
            PassOut(false);
            GoingToBeNormal();
            Sickness(true);
        }
        if (_Temperature < 21.0f)
        {
            // 3단계 : 21도 이하 30% 감소
            m_HypothermiaLevel = 3;
            fPassOut = 0f;
        }
        else if (_Temperature < 27.0f)
        {
            // 2단계 : 27도 이하 20% 감소
            m_HypothermiaLevel = 2;
        }
        else if (_Temperature < 32.0f)
        {
            // 1단계 : 32도 이하 10% 감소
            m_HypothermiaLevel = 1;
        }
        else
        {
            m_HypothermiaLevel = 0;
            fPassOut = 0f;
        }
        buff();
    }//해제 조건 : 33도 이상

    public void Sickness(bool isOn) // 질병 :: only can eat Shrimp  && 방석,담요,난로만 사용가능
    {
        // 조건 1 : 기절 상태 해제
        // 조건 2 : 비/눈보라
        if (isOn)
        {
            ism.SwitchValue_Handwarmer(true);
            ism.SwitchValue_Cushion(true);
            ism.SwitchValue_Cape(true);
            ism.SwitchValue_Shrimp(true);
            m_Sickness.SetActive(true);
            isSickness = true;
        }
        else if (!isOn)
        {
            m_Sickness.SetActive(false);
            isSickness = false;
            GoingToBeNormal();
        }
    }//해제 조건 : 치료하기 (광고 1회)

    public void PassOut(bool isTrue) // 기절 :: 하트 획득 불가, 터치당 체온 증가 n%감소, 먹이 섭치 불가, 방석,담요,난로만 사용가능
    {
        //체온 10도 이하
        if (isTrue)
        {
            fPassOut = -0.4f;
            isPassOut = true;
            m_PassOut.SetActive(true);
            buff();
        }
        else
        {
            fPassOut = 0f;
            isPassOut = false;
            m_PassOut.SetActive(false);
            buff();
        }
    }// 체온 18도 이상

    public void Death() // 사망
    {
        // 아사 - 포만감 0 n시간 지속 -> 사망. 광고시청시 2회까지 부활 가능 / 코인을 이용한 부활 가능
        // 동사 - 체온 0 -> 사망
        // 병사 - 아픔 상태에서 다음날 사망할 확률 n% -> 사망

        //광고시청 기능 필요
    }

    public void Sleep() // 수면 :: 체온감소X, 아이템 사용X, 아사 Tick 멈춤 / 졸림
    {
        // 재접속시 페널티 계산하는 스크립트 필요
    }

    public IEnumerator Happiness() // 행복함 :: 최종 터치 효율 n%증가
    {
        // 포만감,체온 최대치
        Penguri.sharedInstance.setAddedIncreaseAmountHunger(1.2f);
        Penguri.sharedInstance.setAddedIncreaseAmountTemperature(1.2f);
        Debug.Log("시작");
        fHappiness = 0.2f;
        m_Happiness.SetActive(true);
        buff();
        yield return new WaitForSecondsRealtime(10f);
        m_Happiness.SetActive(false);
        fHappiness = 0f;
        buff();
        Debug.Log("끝");
    }// 포만감 80 % 이하 or 체온 35 이하

    public IEnumerator FoodBaby() // 배부름 :: 포만감 감소 X
    {
        // 포만감 최대
        m_FoodBaby.SetActive(true);
        isFull = true;
        yield return new WaitForSecondsRealtime(m_FullTime);
        m_FoodBaby.SetActive(false);
        isFull = false;
    }// 배부름 유지 시간 초과

    void GoingToBeNormal()
    {
        //if(ism.GetCocoa().GetComponent<Food>().isPurchased)
        ism.SwitchValue_Cocoa(ism.GetCocoa().GetComponent<WarmItem>().isPurchased ? true : false);
        ism.SwitchValue_Cushion(ism.GetCushion().GetComponent<WarmItem>().isPurchased ? true : false);
        ism.SwitchValue_Handwarmer(ism.GetHandwarmer().GetComponent<WarmItem>().isPurchased ? true : false);
        ism.SwitchValue_Cape(ism.GetCape().GetComponent<WarmItem>().isPurchased ? true : false);
        ism.SwitchValue_Muffler(ism.GetMuffler().GetComponent<WarmItem>().isPurchased ? true : false);
        ism.SwitchValue_Beanie(ism.GetBeanie().GetComponent<WarmItem>().isPurchased ? true : false);

        ism.SwitchValue_Shrimp(ism.GetShrimp().GetComponent<Food>().isPurchased ? true : false);
        ism.SwitchValue_Sardine(ism.GetSardine().GetComponent<Food>().isPurchased ? true : false);
        ism.SwitchValue_SmallOctopus(ism.GetSmallOctopus().GetComponent<Food>().isPurchased ? true : false);
        ism.SwitchValue_Octopus(ism.GetOctopus().GetComponent<Food>().isPurchased ? true : false);
        ism.SwitchValue_Squid(ism.GetSquid().GetComponent<Food>().isPurchased ? true : false);
        ism.SwitchValue_Mackerel(ism.GetMackerel().GetComponent<Food>().isPurchased ? true : false);


    }

    public void setExtraCharge(float _charge) // 배율
    {
        extraCharge = _charge;
    }

    public float getExtraCharge()
    {
        return extraCharge;
    }
}
