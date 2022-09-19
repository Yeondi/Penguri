using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour
{
    enum Penalty
    {
        Rain,
        Snow,
        Blizzard,
        Seagull,
        Seal,
    }

    enum Request
    {
        Food,
        Heat
    }

    enum Behaviour
    {
        Talk,
        Stare,
        Sing
    }

    enum AD
    {
        Boost,
        Heart,
        Coin,
        TimeLeaf
    }

    [SerializeField]
    GameObject[] m_PenaltyPrefabs;

    [SerializeField]
    GameObject[] m_RequestPrefabs;

    [SerializeField]
    GameObject[] m_BehaviourPrefabs;

    //[SerializeField]
    //GameObject[] m_AD;

    EventType[] m_Penalty;
    EventType[] m_Request;
    EventType[] m_Behaviour;
    private void Start()
    {
        init();
    }

    void init()
    {
        m_Penalty = new EventType[m_PenaltyPrefabs.Length];
        m_Request = new EventType[m_RequestPrefabs.Length];
        m_Behaviour = new EventType[m_BehaviourPrefabs.Length];
        for (int i = 0; i < m_PenaltyPrefabs.Length; i++)
            m_Penalty[i] = Instantiate(m_PenaltyPrefabs[i], this.transform).GetComponent<EventType>();
        for(int i=0;i<m_RequestPrefabs.Length;i++)
            m_Request[i] = Instantiate(m_RequestPrefabs[i], this.transform).GetComponent<EventType>();
        for(int i=0;i<m_BehaviourPrefabs.Length;i++)
            m_Behaviour[i] = Instantiate(m_BehaviourPrefabs[i], this.transform).GetComponent<EventType>();

        //for (int i = 0; i < m_PenaltyPrefabs.Length; i++)
        //{
        //    m_PenaltyPrefabs[i].SetActive(false);
        //}
        //for (int i = 0; i < m_RequestPrefabs.Length; i++)
        //{
        //    m_RequestPrefabs[i].SetActive(false);
        //}
        //foreach (GameObject go in m_BehaviourPrefabs)
        //    go.SetActive(false);



    }

    public IEnumerator Rain()
    {
        //우산 아이콘 터치 (prefab)
        //실패시 체온 10 감소 / 70%확률로 아픔

        Debug.Log("On Rain");

        m_Penalty[0].gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(10f);
        if (m_Penalty[0].isClicked == false)
        {
            Debug.Log("페널티 받음");
            Penguri.sharedInstance.ADD_GettingWarmUp(-0.1f,1);
            //70확률로 아픔 함수 만들기
            m_Penalty[0].gameObject.SetActive(false);
        }

    }

    public IEnumerator Snow(bool isBlizzard = false)
    {
        Debug.Log("On Snow");

        if(!isBlizzard) // = Snow
        {
            m_Penalty[1].gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(10f);
            if (m_Penalty[1].isClicked == false)
            {
                Debug.Log("페널티 받음");
                //체온 감소 속도 2배
                m_Penalty[1].gameObject.SetActive(false);
            }
        }
        else if(isBlizzard)
        {
            m_Penalty[2].gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(10f);
            if (m_Penalty[2].isClicked == false)
            {
                Debug.Log("페널티 받음");
                Penguri.sharedInstance.ADD_GettingWarmUp(-0.2f, 1);
                GameManager.sharedInstance.GetBuffController().Sickness(true);
                m_Penalty[2].gameObject.SetActive(false);
            }
        }
    }

    public void Seagull()
    {

    }

    public void Seal()
    {

    }

    public void RequestFood()
    {

    }

    public void RequestKeepingWarm()
    {

    }

    public void Talk()
    {

    }

    public void Stare()
    {

    }

    public void Sing()
    {

    }

    public void Boost()
    {

    }

    public void HeartReward()
    {

    }

    public void CashReward()
    {

    }

    public void SlowDownTimeDecrease()
    {

    }
}
