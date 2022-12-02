using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Threading.Tasks;

public class CloudSaveManager : MonoBehaviour
{
    public static CloudSaveManager sharedInstance = null;

    public ICloudSaveDataClient Data;

    public int heart => m_CachedCloudData[heartKey];
    public int coin => m_CachedCloudData[coinKey];

    public const string heartKey = "PG_PLAYER_HEART";
    public const string coinKey = "PG_PLAYER_COIN";

    Dictionary<string, int> m_CachedCloudData = new Dictionary<string, int>
    {
        { heartKey,0 },
        { coinKey,0 },
    };

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
            Destroy(gameObject);
        else
            sharedInstance = this;

        ProcessAuthenticate();
    }

    public async Task LoadAndCacheData()
    {
        try
        {
            var savedData = await CloudSaveService.Instance.Data.LoadAllAsync();

            if (this == null) return;

            var missingData = new Dictionary<string, object>();

            if (savedData.ContainsKey(heartKey))
            {
                m_CachedCloudData[heartKey] = int.Parse(savedData[heartKey]);
            }
            else
            {
                missingData.Add(heartKey, 1000);
                m_CachedCloudData[heartKey] = 1000;
            }

            if (savedData.ContainsKey(coinKey))
            {
                m_CachedCloudData[coinKey] = int.Parse(savedData[coinKey]);
            }
            else
            {
                missingData.Add(coinKey, 1000);
                m_CachedCloudData[coinKey] = 1000;
            }

            if(missingData.Count > 0)
            {
                await SaveUpdatedData(missingData);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    async Task SaveUpdatedData(Dictionary<string, object> data)
    {
        try
        {
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    async Task ProcessAuthenticate()
    {
        await UnityServices.InitializeAsync();
        await SignInAnonymously();
    }

    async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            var playerId = AuthenticationService.Instance.PlayerId;

            Debug.Log("Signed in as : " + playerId);
        };

        AuthenticationService.Instance.SignInFailed += s =>
        {
            Debug.Log(s);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        var data = new Dictionary<string, object> { { "MySaveKey", "HelloWorld" } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }
}
