using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace SelectiveConsumer.Utilities;

public static class Support
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}