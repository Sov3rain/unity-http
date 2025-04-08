using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duck.Http.Service
{
    public interface IHttpRequest
    {
        IHttpRequest RemoveSuperHeaders();
        IHttpRequest SetHeader(string key, string value);
        IHttpRequest SetHeaders(IEnumerable<KeyValuePair<string, string>> headers);
        IHttpRequest OnUploadProgress(Action<float> onProgress);
        IHttpRequest OnDownloadProgress(Action<float> onProgress);
        IHttpRequest OnSuccess(Action<HttpResponse> onSuccess);
        IHttpRequest OnError(Action<HttpResponse> onError);
        IHttpRequest OnNetworkError(Action<HttpResponse> onNetworkError);
        bool RemoveHeader(string key);
        IHttpRequest SetTimeout(int duration);
        void Send();
        Task<HttpResponse> SendAsync();
        IHttpRequest SetRedirectLimit(int redirectLimit);
        void Abort();
    }
}