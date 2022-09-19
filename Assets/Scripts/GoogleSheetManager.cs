using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class GoogleSheetManager : MonoBehaviour
{
    const string StreeSheetURL = "https://docs.google.com/spreadsheets/d/11rkb-Y3Os0WNMsZqZmrB9I7ynAGB_W4YwWLRnX94STk/export?format=tsv";
    const string GrowthStatus = "https://docs.google.com/spreadsheets/d/1mGQUlrMyz5jmAyQ3CrBXLH7sRyJLuxX5MgUoR9JvHlE/export?format=tsv";
    const string FeedingTable = "https://docs.google.com/spreadsheets/d/10aG_zyvmk2Fwu34t-qQoRu0EdHZs9rzFEWxTgr2IE4w/export?format=tsv";
    const string WarmingTable = "https://docs.google.com/spreadsheets/d/1slYb1F6YU9XIar6r4mK-96p7W5A5okGbInWxV_3b_wQ/export?format=tsv";
    public bool recall = false;

    int nRows;

    int nTableCount = 0;

    public bool bGoodToGo = false;
    //IEnumerator Start()
    //{
    //    UnityWebRequest www = UnityWebRequest.Get(URL);
    //    yield return www.SendWebRequest();

    //    string data = www.downloadHandler.text;
    //    print(data);
    //}

    private void Update()
    {
        if (recall)
        {
            //StartCoroutine(Recall());
            StartCoroutine(CallRoutine(StreeSheetURL));
            StartCoroutine(CallRoutine(GrowthStatus));
            StartCoroutine(CallRoutine(FeedingTable));
            StartCoroutine(CallRoutine(WarmingTable));
            recall = false;
        }
    }

    public IEnumerator Recall()
    {
        UnityWebRequest www = UnityWebRequest.Get(StreeSheetURL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        var a = data.Split('\t');
        CountRows(a);

        GameManager.sharedInstance.GetComponent<TSVLoader>().setData(data, nRows);


        UnityWebRequest GrowthStatusRequest = UnityWebRequest.Get(GrowthStatus);
        yield return GrowthStatusRequest.SendWebRequest();

        data = GrowthStatusRequest.downloadHandler.text;
        a = data.Split('\t');
        CountRows(a);

        GameManager.sharedInstance.GetComponent<TSVLoader>().setData(data, nRows);

    }

    IEnumerator CallRoutine(string __fileName__)
    {

        UnityWebRequest www = UnityWebRequest.Get(__fileName__);
        yield return www.SendWebRequest();

        nTableCount++;

        string data = www.downloadHandler.text;
        var a = data.Split('\t');
        CountRows(a);

        bGoodToGo = LoadingSceneController.sharedInstance.GetComponent<TSVLoader>().setData(data, nRows);

        if (bGoodToGo)
        {
            LoadingSceneController.sharedInstance.setGoodToGo(bGoodToGo);
            //GameManager.sharedInstance.getPenguriManager().DEBUG__Start();
        }
    }

    void CountRows(string[] data)
    {
        for(int i=0; i < data.Length; i++)
        {
            if(data[i].Contains("\r"))
            {
                nRows = i + 1;
                break;
            }
        }
    }
}
