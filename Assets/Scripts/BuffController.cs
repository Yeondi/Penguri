#define __DEBUG__
//#define __Release__
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    //���ӸŴ������� �ҷ����� ���
    //Ȱ������ true -> Penguri call this
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
        m_Happiness = GameObject.Find("Buff").transform.GetChild(0).gameObject;
        m_FoodBaby = GameObject.Find("Buff").transform.GetChild(1).gameObject;
        m_PassOut = GameObject.Find("Buff").transform.GetChild(2).gameObject;
        m_Starve = GameObject.Find("Buff").transform.GetChild(3).gameObject;
        m_LowTemp = GameObject.Find("Buff").transform.GetChild(4).gameObject;
        m_Sickness = GameObject.Find("Buff").transform.GetChild(5).gameObject;

        ism = GameManager.sharedInstance.GetItemStatusManager();
    }

    float fBasic = 1.0f;

    float fStarve = 0f;
    float fPassOut = 0f;
    float fHappiness = 0f;

    private void FixedUpdate()
    {
        Debug.Log("����� : " + fStarve + "\n��ü���� : " + m_HypothermiaLevel * -0.1f + "\n���� : " + fPassOut
            + "\n�ູ�� : " + fHappiness);

        if (isOnSicknessDEBUG)
            Sickness(true);

        //���� Ư�� ������ ���� ����(��: sickness)�� �ִٸ�, ���⼭ ��� ����


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
            ism.SwitchValue_Blanket(true);
            ism.SwitchValue_Heater(true);
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
            ism.SwitchValue_Blanket(true);
            ism.SwitchValue_Heater(true);
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
        m_Weight = fBasic + fStarve + (m_HypothermiaLevel * -10f) + fPassOut + fHappiness;
    }

    public void Starve(float _Percentage) // ���ָ� :: ���� ��ġ n% ����
    {
        //Hunger 30%����
        if (_Percentage > 0.5f)
        {
            Debug.Log("��� 50%�̻�");
            m_Starve.SetActive(false);
            fStarve = 0f;
            buff();
            return;
        }
        m_Starve.SetActive(true);
        fStarve = -0.5f;
        buff();
        Debug.Log("��� 30%����");
    }//Hunger 50%�̻�� ����

    public void Hypothermia(float _Temperature) // ��ü���� :: n�� ���� ������ġ m% ����
    {
        if (_Temperature < 10.0f)
        {//����
            PassOut(true);
        }
        else if (_Temperature > 18f)
        {
            PassOut(false);
            GoingToBeNormal();
            Sickness(true);
        }
        if (_Temperature < 21.0f)
        {
            // 3�ܰ� : 21�� ���� 30% ����
            m_HypothermiaLevel = 3;
            fPassOut = 0f;
        }
        else if (_Temperature < 27.0f)
        {
            // 2�ܰ� : 27�� ���� 20% ����
            m_HypothermiaLevel = 2;
        }
        else if (_Temperature < 32.0f)
        {
            // 1�ܰ� : 32�� ���� 10% ����
            m_HypothermiaLevel = 1;
        }
        else
        {
            m_HypothermiaLevel = 0;
            fPassOut = 0f;
        }
        buff();
    }//���� ���� : 33�� �̻�

    public void Sickness(bool isOn) // ���� :: only can eat Shrimp  && �漮,���,���θ� ��밡��
    {
        // ���� 1 : ���� ���� ����
        // ���� 2 : ��/������
        if (isOn)
        {
            ism.SwitchValue_Blanket(true);
            ism.SwitchValue_Cushion(true);
            ism.SwitchValue_Heater(true);
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
    }//���� ���� : ġ���ϱ� (���� 1ȸ)

    public void PassOut(bool isTrue) // ���� :: ��Ʈ ȹ�� �Ұ�, ��ġ�� ü�� ���� n%����, ���� ��ġ �Ұ�, �漮,���,���θ� ��밡��
    {
        //ü�� 10�� ����
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
    }// ü�� 18�� �̻�

    public void Death() // ���
    {
        // �ƻ� - ������ 0 n�ð� ���� -> ���. �����û�� 2ȸ���� ��Ȱ ���� / ������ �̿��� ��Ȱ ����
        // ���� - ü�� 0 -> ���
        // ���� - ���� ���¿��� ������ ����� Ȯ�� n% -> ���
    }

    public void Sleep() // ���� :: ü�°���X, ������ ���X, �ƻ� Tick ���� / ����
    {
        //�ڼ��Ѱ� ��ȹ�� ����
    }

    public IEnumerator Happiness() // �ູ�� :: ���� ��ġ ȿ�� n%����
    {
        // ������,ü�� �ִ�ġ
        Penguri.sharedInstance.setAddedIncreaseAmountHunger(1.2f);
        Penguri.sharedInstance.setAddedIncreaseAmountTemperature(1.2f);
        Debug.Log("����");
        fHappiness = 0.2f;
        m_Happiness.SetActive(true);
        buff();
        yield return new WaitForSecondsRealtime(10f);
        m_Happiness.SetActive(false);
        fHappiness = 0f;
        buff();
        Debug.Log("��");
    }// ������ 80 % ���� or ü�� 35 ����

    public IEnumerator FoodBaby() // ��θ� :: ������ ���� X
    {
        // ������ �ִ�
        m_FoodBaby.SetActive(true);
        isFull = true;
        yield return new WaitForSecondsRealtime(m_FullTime);
        m_FoodBaby.SetActive(false);
        isFull = false;
    }// ��θ� ���� �ð� �ʰ�

    void GoingToBeNormal()
    {
        //if(ism.GetCocoa().GetComponent<Food>().isPurchased)
        ism.SwitchValue_Cocoa(ism.GetCocoa().GetComponent<WarmItem>().isPurchased ? true : false);
        ism.SwitchValue_Cushion(ism.GetCushion().GetComponent<WarmItem>().isPurchased ? true : false);
        ism.SwitchValue_Blanket(ism.GetBlanket().GetComponent<WarmItem>().isPurchased ? true : false);
        ism.SwitchValue_Heater(ism.GetHeater().GetComponent<WarmItem>().isPurchased ? true : false);
        ism.SwitchValue_Muffler(ism.GetMuffler().GetComponent<WarmItem>().isPurchased ? true : false);
        ism.SwitchValue_Beanie(ism.GetBeanie().GetComponent<WarmItem>().isPurchased ? true : false);

        ism.SwitchValue_Shrimp(ism.GetShrimp().GetComponent<Food>().isPurchased ? true : false);
        ism.SwitchValue_Sardine(ism.GetSardine().GetComponent<Food>().isPurchased ? true : false);
        ism.SwitchValue_SmallOctopus(ism.GetSmallOctopus().GetComponent<Food>().isPurchased ? true : false);
        ism.SwitchValue_Octopus(ism.GetOctopus().GetComponent<Food>().isPurchased ? true : false);
        ism.SwitchValue_Squid(ism.GetSquid().GetComponent<Food>().isPurchased ? true : false);
        ism.SwitchValue_Mackerel(ism.GetMackerel().GetComponent<Food>().isPurchased ? true : false);


    }
}
