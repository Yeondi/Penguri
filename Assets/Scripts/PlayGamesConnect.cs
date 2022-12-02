using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayGamesConnect : MonoBehaviour
{
    [SerializeField]
    GameObject Go_Google;
    [SerializeField]
    GameObject Go_Apple;
    [SerializeField]
    GameObject Go_Guest;

    string path;
    string filename;

    [SerializeField]
    TMP_Text __google__;

    bool isGuest = false;



    //LoginScene
    public GameObject Agreement;
    public GameObject TimeCheck;

    bool TermsAndConditions;
    bool PersonalInformation;


    private void Start()
    {
        path = Application.persistentDataPath + "/";
        filename = "save.atree";

#if UNITY_ANDROID
        Go_Google.SetActive(true);
        Go_Guest.SetActive(true);

        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        if (haveSaveFile())
            Login();

#elif UNITY_IOS
        Go_Apple.SetActive(true);
        Go_Guest.SetActive(true);
#endif
    }

    public bool haveSaveFile()
    {
        try
        {
            string data = File.ReadAllText(path + filename);
            print("파일을 찾았어요!");
            return true;
        }
        catch (System.Exception e)
        {
            print("파일이 없는데요?");
            return false;
        }
    }

    public void Login()
    {
        if (!isGuest)
        {
            if (!PlayGamesPlatform.Instance.localUser.authenticated)
            {
                Social.localUser.Authenticate((bool success) =>
                {
                    if (success)
                    {
                        __google__.text = Social.localUser.id + " : " + Social.localUser.userName;
                        MoveToNextScene();
                    }
                    else
                        __google__.text = "Failed";
                });
            }
        }
        else if(isGuest)
        {
            MoveToNextScene();
        }
    }

    public void LogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
    }

    public void ShowLeaderBoard()
    {
        __google__.text = "ShowLeaderBoard";

        Social.ShowLeaderboardUI();
    }

    public void AddLeaderBoard()
    {
    }

    void MoveToNextScene()
    {
        SceneManager.LoadScene("LoadingScene");
    }
    public void GuestPopup()
    {
        isGuest = true;
    }


    public void Check_Agreement()
    {
        if(TermsAndConditions && PersonalInformation)
        {
            TimeCheck.SetActive(true);
            Agreement.SetActive(false);
        }
    }

    public void SetTermsAndConditions(bool isTrue)
    {
        TermsAndConditions = isTrue;
    }

    public void SetPersonalInformation(bool isTrue)
    {
        PersonalInformation = isTrue;
    }
}
