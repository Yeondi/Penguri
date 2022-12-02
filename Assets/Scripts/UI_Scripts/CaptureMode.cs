using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using TMPro;

public class CaptureMode : MonoBehaviour
{
    private Texture2D _imageTexture;
    string fileName = "�ȳ�!_���";
    string extName = "png";


    string FolderPath = "";
    string TotalPath = "";



    string lastSavedPath;

    bool _TakeScreenshotWithoutUI = false; // UI�����ϰ� ĸ��  �׽�Ʈ��

    //[SerializeField]
    //TMP_Text testTExt;

    private void Awake()
    {
#if UNITY_EDITOR
        FolderPath = $"{Application.dataPath}/ScreenShots/";
        TotalPath = $"{FolderPath}/{fileName}_{DateTime.Now.ToString("MMdd_HHmmss")}.{extName}";
#elif UNITY_ANDROID
        FolderPath = $"/storage/emulated/0/DCIM/{Application.productName}/";
        TotalPath = $"{FolderPath}/{fileName}_{DateTime.Now.ToString("MMdd_HHmmss")}.{extName}";
#endif
    }

    void OnClickCaptureButton()
    {
        string _totalPath = TotalPath;
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }

        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);

        Rect area = new Rect(0f, 0f, Screen.width, Screen.height);
        screenTexture.ReadPixels(area, 0, 0);

        bool succeeded = true;
        try
        {
            if (Directory.Exists(FolderPath) == false)
            {
                Directory.CreateDirectory(FolderPath);
            }

            File.WriteAllBytes(TotalPath, screenTexture.EncodeToPNG());
        }
        catch (Exception e)
        {
            succeeded = false;
            Debug.LogWarning($"Screenshot Save Failed : {_totalPath}");
            //testTExt.text = _totalPath;
            Debug.LogWarning(e);
        }
        Destroy(screenTexture);

        if (succeeded)
        {
            Debug.Log($"Screenshot Saved in : {_totalPath}");
            lastSavedPath = _totalPath;
            //testTExt.text = _totalPath;
            TotalPath = $"{FolderPath}/{fileName}_{DateTime.Now.ToString("MMdd_HHmmss")}.{extName}"; // ��ũ���� �ð� ����

            if (QuestHandler.sharedInstance.getEventType1() == "Function" && QuestHandler.sharedInstance.getEventType2() == "Take_A_Pic")
            {
                QuestHandler.sharedInstance.CompleteQuest();
                Broadcast.sharedInstance.Notify("Function", "Take_A_Pic");
            }
        }

        RefreshAndroidGallery(_totalPath);

    }

    [System.Diagnostics.Conditional("UNITY_ANDROID")]
    void RefreshAndroidGallery(string imageFilePath)
    {
#if !UNITY_EDITOR
        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2]
            {"android.intent.action.MEDIA_SCANNER_SCAN_FILE",classUri.CallStatic<AndroidJavaObject>("parse", "file://" + imageFilePath)});
        objActivity.Call("sendBroadcast", objIntent);
#endif
    }

    void ShowRecentSavedFile(Image destination)
    {
        string _folderPath = FolderPath;
        string _totalPath = lastSavedPath;

        if (Directory.Exists(_folderPath) == false)
        {
            Debug.LogWarning($"{_folderPath} ������ �������� �ʽ��ϴ�.");
            return;
        }
        if (File.Exists(_totalPath) == false)
        {
            Debug.LogWarning($"{_totalPath} ������ �������� �ʽ��ϴ�.");
            return;
        }

        // ������ �ؽ��� �ҽ� ����
        if (_imageTexture != null)
            Destroy(_imageTexture);
        if (destination.sprite != null)
        {
            Destroy(destination.sprite);
            destination.sprite = null;
        }

        // ����� ��ũ���� ���� ��ηκ��� �о����
        try
        {
            byte[] texBuffer = File.ReadAllBytes(_totalPath);

            _imageTexture = new Texture2D(1, 1, TextureFormat.RGB24, false);
            _imageTexture.LoadImage(texBuffer);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"��ũ���� ������ �д� �� �����Ͽ����ϴ�.");
            Debug.LogWarning(e);
            return;
        }

        // �̹��� ��������Ʈ�� ����
        Rect rect = new Rect(0, 0, _imageTexture.width, _imageTexture.height);
        Sprite sprite = Sprite.Create(_imageTexture, rect, Vector2.one * 0.5f);
        destination.sprite = sprite;
    }

    public void TakeScreenshot()
    {
        Debug.Log("��ũ���� Ŭ�� ��");
        StartCoroutine(TakeScreenshotRoutine());
    }

    public IEnumerator TakeScreenshotRoutine()
    {
        yield return new WaitForEndOfFrame();

        OnClickCaptureButton();
    }

    private void OnPostRender()
    {
        if (_TakeScreenshotWithoutUI)
        {
            _TakeScreenshotWithoutUI = false;

            OnClickCaptureButton();
        }
    }
}
