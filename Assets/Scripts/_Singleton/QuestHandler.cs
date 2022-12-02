using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestHandler : MonoBehaviour
{
    public static QuestHandler sharedInstance = null;


    [SerializeField]
    GameObject go_Confirm;
    [SerializeField]
    GameObject go_Shortcut;

    [SerializeField]
    TMP_Text Content_Title;
    [SerializeField]
    TMP_Text Content_Goal;

    int m_progress;
    int m_Goal;

    Dictionary<string, List<string>> m_savedData;

    [SerializeField]
    int currentQuest_Id = 1;
    [SerializeField]
    string currentQuest_Key;

    string m_EventType1;
    string m_EventType2;

    public bool isLoaded = false;

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
            Destroy(gameObject);
        else
            sharedInstance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (GameManager.sharedInstance.getTsvData() != null && !isLoaded)
        {
            m_savedData = GameManager.sharedInstance.getTsvData();
        }
    }

    public void SetData(string __QuestKey)
    {
        Content_Title.text = m_savedData[__QuestKey][2];
        Content_Goal.text = "(" + m_progress + "/" + m_savedData[__QuestKey][3] + ")";
        currentQuest_Key = __QuestKey;
        currentQuest_Id = int.Parse(__QuestKey.Substring(__QuestKey.Length - 2, 2));
        m_Goal = int.Parse(m_savedData[__QuestKey][3]);
        m_EventType1 = m_savedData[__QuestKey][6];
        m_EventType2 = m_savedData[__QuestKey][7];
    }

    public void LoadData(QuestInfo info)
    {
        Content_Title.text = m_savedData[info.Quest_Key][2];
        Content_Goal.text = info.Quest_Goal != -1 ? "(" + info.Quest_Progress + "/" + info.Quest_Goal + ")" : "";

        m_progress = info.Quest_Progress;
        m_Goal = Content_Goal.text != "-1" ? info.Quest_Goal : 0;
        currentQuest_Id = int.Parse(info.Quest_Key.Substring(info.Quest_Key.Length - 2, 2));
        m_EventType1 = m_savedData[info.Quest_Key][6];
        m_EventType2 = m_savedData[info.Quest_Key][7];

        currentQuest_Key = info.Quest_Key;

        SwitchButton();
    }

    public void CompleteQuest()
    {
        Reward.sharedInstance.GetReward(m_savedData[currentQuest_Key][4], m_savedData[currentQuest_Key][5]);

        currentQuest_Id++;

        if (currentQuest_Id <= 9)
        {
            currentQuest_Key = "Tutorial_0" + currentQuest_Id;
        }
        else
        {
            currentQuest_Key = "Tutorial_" + currentQuest_Id;
        }

        m_progress = 0;
        m_Goal = int.Parse(m_savedData[currentQuest_Key][3]);

        Content_Title.text = m_savedData[currentQuest_Key][2];
        Content_Goal.text = m_savedData[currentQuest_Key][3] != (-1).ToString() ? "(" + m_progress + "/" + m_savedData[currentQuest_Key][3] + ")" : "";
        m_EventType1 = m_savedData[currentQuest_Key][6];
        m_EventType2 = m_savedData[currentQuest_Key][7];


        go_Shortcut.SetActive(true);
        go_Confirm.SetActive(false);
    }

    void UpdateGoal()
    {
        Content_Goal.text = "(" + m_progress + "/" + m_savedData[currentQuest_Key][3] + ")";
    }

    public void ProgressOfQuest(int __Amount = 1)
    {
        if (m_progress < m_Goal)
            m_progress+= __Amount;

        UpdateGoal();

        SwitchButton();
    }

    public void SwitchButton()
    {
        if (m_progress >= m_Goal)
        {
            go_Shortcut.SetActive(false);
            go_Confirm.SetActive(true);
        }
    }

    public void setKey(string __key)
    {
        currentQuest_Key = __key;
    }

    public string getKey()
    {
        return currentQuest_Key;
    }

    public void setId(int __id)
    {
        currentQuest_Id = __id;
    }

    public int getId()
    {
        return currentQuest_Id;
    }

    public void setProgress(int __progress)
    {
        m_progress = __progress;
    }

    public int getProgress()
    {
        return m_progress;
    }

    public void setGoal(int __goal)
    {
        m_Goal = __goal;
    }

    public int getGoal()
    {
        return m_Goal;
    }

    public void setEventType1(string __Type)
    {
        m_EventType1 = __Type;
    }

    public string getEventType1()
    {
        return m_EventType1;
    }

    public void setEventType2(string __Type)
    {
        m_EventType2 = __Type;
    }

    public string getEventType2()
    {
        return m_EventType2;
    }
}
