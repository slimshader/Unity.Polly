using Cysharp.Threading.Tasks;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using System;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class Main : MonoBehaviour
{
    async void Start()
    {

        var policy = CreateTokenRefreshPolicy(async (newAccessToken) =>
        {
            await UniTask.Delay(1000);

            Debug.Log("New access token: " + newAccessToken);
        });

        var webRequest = await policy.ExecuteAsync(async () =>
        {
            Debug.Log("Send request");

            await UniTask.SwitchToMainThread();

            var request = UnityWebRequest.Get("https://www.boost.org/LICENSE_1_0.txt");
            request.SetRequestHeader("Authorization", "Bearer " + "access_token");

            await request.SendWebRequest();
            return request;
        });

        async UniTask<int> LongRunningOptimistic(CancellationToken cancellationToken)
        {
            await UniTask.Delay(5000, cancellationToken: cancellationToken);
            return 1;
        }

        async UniTask<int> LongRunningPessimistic()
        {
            await UniTask.Delay(5000);
            return 1;
        }

        Debug.Log(webRequest.downloadHandler.text);

        var res1 = await CreateTimeOutPolicy(TimeoutStrategy.Optimistic)
            .ExecuteAndCaptureAsync(ct => LongRunningOptimistic(ct), CancellationToken.None);

        Debug.Log(res1);

        var res2 = await CreateTimeOutPolicy(TimeoutStrategy.Pessimistic)
            .ExecuteAndCaptureAsync(() => LongRunningPessimistic());

        Debug.Log(res2);

    }

    private AsyncPolicy CreateTimeOutPolicy(TimeoutStrategy strategy)
    {
        var policy = Policy
            .TimeoutAsync(TimeSpan.FromSeconds(3), strategy,  (context, timeSpan, task) =>
            {
                Debug.Log($"TimeOut {strategy} policy");
                return UniTask.CompletedTask;
            });

        return policy;
    }

    private AsyncPolicy<UnityWebRequest> CreateTokenRefreshPolicy(Func<string, UniTask> tokenRefreshed)
    {
        var policy = Policy
            .HandleResult<UnityWebRequest>(message => message.responseCode == (long)HttpStatusCode.Unauthorized)
            .RetryAsync(1, async (result, retryCount, context) =>
            {
                if (context.ContainsKey("refresh_token"))
                {
                    var newAccessToken = await RefreshAccessToken(context["refresh_token"].ToString());
                    if (newAccessToken != null)
                    {
                        await tokenRefreshed(newAccessToken);

                        context["access_token"] = newAccessToken;
                    }
                }
            });

        return policy;
    }

    private UniTask<string> RefreshAccessToken(string v)
    {
        return UniTask.FromResult("new access token");
    }
}
