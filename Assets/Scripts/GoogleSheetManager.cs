using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using static System.Net.WebRequestMethods;

public class GoogleSheetManager : MonoBehaviour
{
    const string StringSheetURL = "https://docs.google.com/spreadsheets/d/11JccWDuXeSy2QssBGu1IhmYKXMa03pda/export?format=tsv"; // StringTable
    const string GrowthStatus = "https://docs.google.com/spreadsheets/d/1_CDDGF3jIItJIUs0YjmHIZ7pDpXbeL3i/export?format=tsv"; // 펭귄 성장단계
    const string FeedingTable = "https://docs.google.com/spreadsheets/d/1I4b5zQ9cW-R1v8frCkYCBkaq6vKFNVVF/export?format=tsv"; // 먹이
    const string WarmingTable = "https://docs.google.com/spreadsheets/d/1PPbmHac89M99QCPpIBKK_ty1uyWagX2u/export?format=tsv"; // 보온 아이템

    const string Diet = "https://docs.google.com/spreadsheets/d/1E3ypFTX7M4ckULiL2XFAP7YeOWiWYnuL/export?format=tsv"; // 식생활 개선
    const string Recycling = "https://docs.google.com/spreadsheets/d/1WBlqaqWCLYKj9ZLHE2cftFgPrSIh-xcU/export?format=tsv"; // 친환경 생활 실천
    const string Energy = "https://docs.google.com/spreadsheets/d/1BjlEbVF2jTuSJIJsFbk7UWeZ2LJepubS/export?format=tsv"; // 에너지 효율 증가
    const string Renewable = "https://docs.google.com/spreadsheets/d/1yQNRhZwQWFzMgSekc110VMeBVmGwrZhK/export?format=tsv"; // 재생 에너지 사용
    const string Transport = "https://docs.google.com/spreadsheets/d/1DrKp7bSxgG2S2Ibk4Mg5bqymgBQdPv01/export?format=tsv"; // 교통수단 개선
    const string Industry = "https://docs.google.com/spreadsheets/d/1YHkW3fm5XK6Jht8L8JqjeSIYDLEJEaMu/export?format=tsv"; // 산업분야 개선
    const string MarineProtection = "https://docs.google.com/spreadsheets/d/1cTfmmQuKDvE-yzZxriNoMKKBu0ea_Uzf/export?format=tsv"; // 해양 보호
    const string ForestAndSoil = "https://docs.google.com/spreadsheets/d/1V5TRMv3qFxUz2V6zmi4uVfVlHnQvfXxx/export?format=tsv"; // 숲,토양 관리

    const string TutorialQuest = "https://docs.google.com/spreadsheets/d/1cWT9gCeUK-rIMn8yK67rBDlBCiCBFJ-7/export?format=tsv";

    public bool recall = false; // 데이터베이스 재호출을 위한 변수  - 디버깅용 or 차후 쓸곳에 대비해서

    int nRows;

    int nTableCount = 0;

    public bool bGoodToGo = false;

    private void Update()
    {
        if (recall)
        {
            //StartCoroutine(Recall());
            StartCoroutine(CallRoutine(StringSheetURL));
            StartCoroutine(CallRoutine(GrowthStatus));
            StartCoroutine(CallRoutine(FeedingTable));
            StartCoroutine(CallRoutine(WarmingTable));

            StartCoroutine(CallRoutine(Diet));
            StartCoroutine(CallRoutine(Recycling));
            StartCoroutine(CallRoutine(Energy));
            StartCoroutine(CallRoutine(Renewable));
            StartCoroutine(CallRoutine(Transport));
            StartCoroutine(CallRoutine(Industry));
            StartCoroutine(CallRoutine(MarineProtection));
            StartCoroutine(CallRoutine(ForestAndSoil));

            StartCoroutine(CallRoutine(TutorialQuest));
            recall = false;
        }
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

    public IEnumerator Recall()
    {
        UnityWebRequest www = UnityWebRequest.Get(StringSheetURL);
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
}
