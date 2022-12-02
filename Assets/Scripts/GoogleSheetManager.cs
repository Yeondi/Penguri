using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using static System.Net.WebRequestMethods;

public class GoogleSheetManager : MonoBehaviour
{
    const string StringSheetURL = "https://docs.google.com/spreadsheets/d/11JccWDuXeSy2QssBGu1IhmYKXMa03pda/export?format=tsv"; // StringTable
    const string GrowthStatus = "https://docs.google.com/spreadsheets/d/1_CDDGF3jIItJIUs0YjmHIZ7pDpXbeL3i/export?format=tsv"; // ��� ����ܰ�
    const string FeedingTable = "https://docs.google.com/spreadsheets/d/1I4b5zQ9cW-R1v8frCkYCBkaq6vKFNVVF/export?format=tsv"; // ����
    const string WarmingTable = "https://docs.google.com/spreadsheets/d/1PPbmHac89M99QCPpIBKK_ty1uyWagX2u/export?format=tsv"; // ���� ������

    const string Diet = "https://docs.google.com/spreadsheets/d/1E3ypFTX7M4ckULiL2XFAP7YeOWiWYnuL/export?format=tsv"; // �Ļ�Ȱ ����
    const string Recycling = "https://docs.google.com/spreadsheets/d/1WBlqaqWCLYKj9ZLHE2cftFgPrSIh-xcU/export?format=tsv"; // ģȯ�� ��Ȱ ��õ
    const string Energy = "https://docs.google.com/spreadsheets/d/1BjlEbVF2jTuSJIJsFbk7UWeZ2LJepubS/export?format=tsv"; // ������ ȿ�� ����
    const string Renewable = "https://docs.google.com/spreadsheets/d/1yQNRhZwQWFzMgSekc110VMeBVmGwrZhK/export?format=tsv"; // ��� ������ ���
    const string Transport = "https://docs.google.com/spreadsheets/d/1DrKp7bSxgG2S2Ibk4Mg5bqymgBQdPv01/export?format=tsv"; // ������� ����
    const string Industry = "https://docs.google.com/spreadsheets/d/1YHkW3fm5XK6Jht8L8JqjeSIYDLEJEaMu/export?format=tsv"; // ����о� ����
    const string MarineProtection = "https://docs.google.com/spreadsheets/d/1cTfmmQuKDvE-yzZxriNoMKKBu0ea_Uzf/export?format=tsv"; // �ؾ� ��ȣ
    const string ForestAndSoil = "https://docs.google.com/spreadsheets/d/1V5TRMv3qFxUz2V6zmi4uVfVlHnQvfXxx/export?format=tsv"; // ��,��� ����

    const string TutorialQuest = "https://docs.google.com/spreadsheets/d/1cWT9gCeUK-rIMn8yK67rBDlBCiCBFJ-7/export?format=tsv";

    public bool recall = false; // �����ͺ��̽� ��ȣ���� ���� ����  - ������ or ���� ������ ����ؼ�

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
