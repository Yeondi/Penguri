using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchToStart : MonoBehaviour
{
    [Header("StartPage")]
    [Header("Will Cover")]
    [SerializeField]
    GameObject Title_image;
    [SerializeField]
    GameObject Touch_To_Start_text;
    [SerializeField]
    GameObject Copyright_text;
    [SerializeField]
    GameObject touchIcon;


    [Header("Will Discover")]
    [SerializeField]
    GameObject Status;
    [SerializeField]
    GameObject TabBar; // 하단바
    [SerializeField]
    GameObject TopPanel; // 탑 패널
    [SerializeField]
    GameObject QuestIcon; // 퀘스트아이콘


    [SerializeField]
    GameObject moneyManager;

#if UNITY_EDITOR
    [SerializeField]
    GameObject Buff;
    [SerializeField]
    GameObject Cheat;
#endif

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Title_image.SetActive(false);
            Touch_To_Start_text.SetActive(false);
            Copyright_text.SetActive(false);
            touchIcon.SetActive(false);

            Status.SetActive(true);
            TabBar.SetActive(true);
            TopPanel.SetActive(true);
            QuestIcon.SetActive(true);
#if UNITY_EDITOR
            Buff.SetActive(true);
            Cheat.SetActive(true);
#endif
            UserDataManager.sharedInstance.bGoodToLoad = true;
            gameObject.SetActive(false);
        }
    }
}
