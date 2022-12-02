using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;

public class CloudCode : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();

        StartCoroutine(Init());

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        //var result = await CloudCode.CallEndpointAsync<ResultType>("HelloWorld", new RequestType { });
        //Debug.Log(result);
    }

    IEnumerator Init()
    {

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
