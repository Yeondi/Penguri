using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchToStart : MonoBehaviour
{
    [Header("StartPage")]
    [Header("Will Cover")]
    [SerializeField]
    GameObject DOS;
    [SerializeField]
    GameObject TTS;


    [Header("Will Discover")]
    [SerializeField]
    GameObject FoodAmount;
    [SerializeField]
    GameObject Temperature;
    [SerializeField]
    GameObject TabBar;
    [SerializeField]
    GameObject TopPanel;
    [SerializeField]
    GameObject QuestIcon;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            DOS.SetActive(false);
            TTS.SetActive(false);
            FoodAmount.SetActive(true);
            Temperature.SetActive(true);
            TabBar.SetActive(true);
            TopPanel.SetActive(true);
            QuestIcon.SetActive(true);
        }
    }
}
