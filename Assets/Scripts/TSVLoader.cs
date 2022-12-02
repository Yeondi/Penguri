using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TSVLoader : MonoBehaviour
{
    private Dictionary<string, List<string>> savedData;
    int nRows;

    int nTableCount = 0;

    private void Awake()
    {
        savedData = new Dictionary<string, List<string>>();
    }
    public bool setData(string data, int _nRows)
    {
        nTableCount++;
        nRows = _nRows;
        char[] seperators = new char[] { '\t', '\r', '\n' };
        string line = string.Empty;
        var a = data.Split(seperators, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < nRows; i++)
        {
            a = a.RemoveAt(0);
        }
        SplitData(a);

        //데이터 수집 끝나는 지점
        if(nTableCount == 5)
        {
            Debug.Log("데이터 수집 끝");
            return true;
        }
        return false;
    }

    void SplitData(string[] strOrigin)
    {
        string strKeyValue = "";
        for (int i = 0; i < strOrigin.Length; i++)
        {
            if (i % nRows == 0)
            {
                strKeyValue = strOrigin[i]; //키 나뉘는곳
                savedData.Add(strOrigin[i], new List<string>());
                //savedData[E_StringTable.Key.ToString()].Add(strOrigin[i]);
            }
            else
            {
                savedData[strKeyValue].Add(strOrigin[i]);
                //savedData[E_StringTable.KR.ToString()].Add(strOrigin[i]);
            }
        }
    }
    public Dictionary<string, List<string>> getSavedData()
    {
        return savedData;
    }

    public enum E_StringTable
    {
        Key,
        KR,
        EN,
    }
}

public class Tranlate : MonoBehaviour
{

}

public static class Extensions
{
    public static T[] RemoveAt<T>(this T[] source, int index)
    {
        var dest = new List<T>(source);
        dest.RemoveAt(index);
        return dest.ToArray();
    }
}