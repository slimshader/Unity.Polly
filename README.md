Port of [Polly](https://github.com/App-vNext/Polly) library for Unity based on UniTask

This version is based of [v7.2.4](https://github.com/App-vNext/Polly/tree/7.2.4) of the Polly library as it has no external dependancies on netstandard 2.1 which is perfect for Unity.

Example of Retry policy combined with a Fallback (by PolicyWrap):

```csharp
p.Threading.Tasks;
using Polly;
using Polly.Fallback;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class Retry : MonoBehaviour
{
    async void Start()
    {
        var retry = Policy
            .Handle<Exception>()
            .RetryAsync(3, onRetry: (Exception ex, int retryCount) =>
            {
                Debug.Log($"{ex.Message} Retry: " + retryCount);
            });

        // in real world scenario:
        //var retryWebRequest = Policy
        //    .HandleResult<UnityWebRequest>(x => x.result == UnityWebRequest.Result.ConnectionError)
        //    .RetryAsync(3, onRetry: (x, retryCount) =>
        //    {
        //        Debug.Log($"Retry: " + retryCount);
        //    });        

        var fallback = Policy<string>
            .Handle<Exception>()
            .FallbackAsync("MIT ");

        var policy = fallback.WrapAsync(retry);

        var license = await policy.ExecuteAsync(FetchLicense);

        Debug.Log(license);
    }

    async UniTask<string> FetchLicense()
    {
        if (UnityEngine.Random.value < 0.8f)
        {
            throw new Exception("Failed to fetch license");
        }

        var request = UnityWebRequest.Get("https://www.boost.org/LICENSE_1_0.txt");
        await request.SendWebRequest();
        return request.downloadHandler.text;
    }
}
```

To install in Unity project use this UPM link: https://github.com/slimshader/Unity.Polly.git?path=Packages/Unity.Polly
