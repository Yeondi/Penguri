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
    string fileName = "안녕!_펭귄";
    string extName = "png";


    string FolderPath = "";
    string TotalPath = "";



    string lastSavedPath;

    bool _TakeScreenshotWithoutUI = false; // UI제외하고 캡쳐  테스트용

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
            TotalPath = $"{FolderPath}/{fileName}_{DateTime.Now.ToString("MMdd_HHmmss")}.{extName}"; // 스크린샷 시간 변경

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
            Debug.LogWarning($"{_folderPath} 폴더가 존재하지 않습니다.");
            return;
        }
        if (File.Exists(_totalPath) == false)
        {
            Debug.LogWarning($"{_totalPath} 파일이 존재하지 않습니다.");
            return;
        }

        // 기존의 텍스쳐 소스 제거
        if (_imageTexture != null)
            Destroy(_imageTexture);
        if (destination.sprite != null)
        {
            Destroy(destination.sprite);
            destination.sprite = null;
        }

        // 저장된 스크린샷 파일 경로로부터 읽어오기
        try
        {
            byte[] texBuffer = File.ReadAllBytes(_totalPath);

            _imageTexture = new Texture2D(1, 1, TextureFormat.RGB24, false);
            _imageTexture.LoadImage(texBuffer);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"스크린샷 파일을 읽는 데 실패하였습니다.");
            Debug.LogWarning(e);
            return;
        }

        // 이미지 스프라이트에 적용
        Rect rect = new Rect(0, 0, _imageTexture.width, _imageTexture.height);
        Sprite sprite = Sprite.Create(_imageTexture, rect, Vector2.one * 0.5f);
        destination.sprite = sprite;
    }

    public void TakeScreenshot()
    {
        Debug.Log("스크린샷 클릭 됨");
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
