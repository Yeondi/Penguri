//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
//using GooglePlayGames.BasicApi.SavedGame;
//using Newtonsoft.Json;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using UnityEngine;
//using UnityEngine.Networking;
//using UnityEngine.Windows;

//public class GoogleCloudSave : MonoBehaviour
//{
//    public Action<SavedGameRequestStatus, byte[]> OnSavedGameDataReadComplete;
//    public Action<SavedGameRequestStatus, ISavedGameMetadata> OnSavedGameDataWrittenComplete;
//    public bool CloudConnected;
//    bool isSaving;

//    const string SAVE_FILE_NAME = "hiiix_pgpg_Info.bin";




//    // App -> 리더보드 관련한애 https://smilejsu.tistory.com/1416

//    void Start()
//    {
        
//    }

//    public void Init()
//    {
//        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
//        PlayGamesPlatform.InitializeInstance(config);
//        PlayGamesPlatform.DebugLogEnabled = true;
//        PlayGamesPlatform.Activate();
//    }

//    public void SignIn(System.Action<bool> OnComplete)
//    {
//        Social.localUser.Authenticate(OnComplete);
//    }

//    public void SaveData()
//    {
//        if(Social.localUser.authenticated)
//        {
//            this.isSaving = true;
//            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
//            savedGameClient.OpenWithAutomaticConflictResolution(SAVE_FILE_NAME, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
//        }
//    }

//    private void SaveLocal()
//    {
//        var path = Application.persistentDataPath + "/hiiix_pgpg_Info.bin";
//        var json = JsonConvert.SerializeObject(App.Instance.gameInfo);
//        byte[] bytes = Encoding.UTF8.GetBytes(json);
//        File.WriteAllBytes(path, bytes);
//    }

//    private void SaveGame(ISavedGameMetadata data)
//    {
//        this.SaveLocal();

//        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
//        SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
//        var stringToSave = this.GameInfoToString();
//        byte[] bytes = Encoding.UTF8.GetBytes(stringToSave);
//        savedGameClient.CommitUpdate(data, update, bytes, OnSavedGameDataWrittenComplete);

//    }

//    public void LoadData()
//    {
//        if (Social.localUser.authenticated)
//        {
//            this.isSaving = false;
//            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(SAVE_FILE_NAME, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, this.OnSavedGameOpened);
//        }
//        else
//        {
//            this.LoadLocal();
//        }
//    }

//    private void LoadLocal()
//    {
//        var path = Application.persistentDataPath + "/hiiix_pgpg_Info.bin";
//        byte[] bytes = File.ReadAllBytes(path);
//        var json = Encoding.UTF8.GetString(bytes);

//        this.StringToGameInfo(json);
//    }

//    private void LoadGame(ISavedGameMetadata data)
//    {
//        ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(data, OnSavedGameDataReadComplete);
//    }

//    private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
//    {
//        Debug.LogFormat("OnSavedGameOpened : {0}, {1}", status, isSaving);

//        if (status == SavedGameRequestStatus.Success)
//        {
//            if (!isSaving)
//            {
//                this.LoadGame(game);
//            }
//            else
//            {
//                this.SaveGame(game);
//            }
//        }
//        else
//        {
//            if (!isSaving)
//            {
//                this.LoadLocal();
//            }
//            else
//            {
//                this.SaveLocal();
//            }
//        }
//    }

//    public void StringToGameInfo(string localData)
//    {
//        if (localData != string.Empty)
//        {
//            App.Instance.gameInfo = JsonConvert.DeserializeObject<GameInfo>(localData);
//        }
//    }

//    private string GameInfoToString()
//    {
//        return JsonConvert.SerializeObject(App.Instance.gameInfo);
//    }
//}
