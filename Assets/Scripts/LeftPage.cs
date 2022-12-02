//#define debug
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class LeftPage : MonoBehaviour
//{
//    [Header("Buttons")]
//    [SerializeField]
//    Button btn_left;
//    [SerializeField]
//    Button btn_right;
//    [Space]
//    [SerializeField]
//    int nCurrentLayer = 0;
//    [SerializeField]
//    int nMaxLayer = 1;
//    [SerializeField]
//    string strSectionName;


//#if debug
//    [Space]
//    [Header("Debug Variables")]
//    [SerializeField]
//#endif
//    int nNumberOfLayers = 0;

//    GameObject[] items;

//    void Start()
//    {
//        if (gameObject.name == "Food Contents")
//            initFoods();
//        else if (gameObject.name == "Temp Contents")
//            initTemperature();
//        else if (gameObject.name == "EarthContents")
//            initEarth();
//    }

//    void initFoods()
//    {
//        nNumberOfLayers = GameObject.Find("Food Layers").transform.childCount;
//        items = new GameObject[nNumberOfLayers];

//        for(int i=0;i<nNumberOfLayers;i++)
//        {
//            items[i] = GameObject.Find("Food Layers").transform.Find("Foods P" + i).gameObject;
//        }
//    }

//    void initTemperature()
//    {
//        nNumberOfLayers = GameObject.Find("Temp Layers").transform.childCount;
//        items = new GameObject[nNumberOfLayers];

//        for(int i=0;i<nNumberOfLayers;i++)
//        {
//            items[i] = GameObject.Find("Temp Layers").transform.Find("Temp P" + i).gameObject;
//        }
//    }

//    void initEarth()
//    {
//        nNumberOfLayers = GameObject.Find("Earth Layers").transform.childCount;
//        items = new GameObject[nNumberOfLayers];

//        for (int i = 0; i < nNumberOfLayers; i++)
//        {
//            items[i] = GameObject.Find("Earth Layers").transform.Find("Earth P" + i).gameObject;
//        }
//    }

//    void updateArrows()
//    {
//        if (nCurrentLayer == 0)
//            btn_left.interactable = false;
//        else
//            btn_left.interactable = true;

//        if (nCurrentLayer == nMaxLayer)
//            btn_right.interactable = false;
//        else
//            btn_right.interactable = true;
//    }

//    public void leftToTheNextPage(string strDirection = "left")
//    {
//        if(strDirection == "left")
//        {
//            items[nCurrentLayer].gameObject.SetActive(false);
//            nCurrentLayer--;
//            items[nCurrentLayer].gameObject.SetActive(true);
//        }
//        else if(strDirection == "right")
//        {
//            items[nCurrentLayer].gameObject.SetActive(false);
//            nCurrentLayer++;
//            items[nCurrentLayer].gameObject.SetActive(true);
//        }
//        updateArrows();
//    }
//}
