using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace UnityHttp.Service.Unity
{
    public class UnityHttpRequest : IHttpRequest, IUpdateProgress
    {
        public UnityWebRequest UnityWebRequest { get; }

        private event Action<float> _onUploadProgress;
        private event Action<float> _onDownloadProgress;
        private event Action<HttpResponse> _onSuccess;
        private event Action<HttpResponse> _onError;
        private event Action<HttpResponse> _onNetworkError;

        private readonly Dictionary<string, string> _headers;
        private float _downloadProgress;
        private float _uploadProgress;

        public UnityHttpRequest(UnityWebRequest unityWebRequest)
        {
            UnityWebRequest = unityWebRequest;
            _headers = new Dictionary<string, string>(Http.GetSuperHeaders());
        }

        public IHttpRequest RemoveSuperHeaders()
        {
            foreach (var kvp in Http.GetSuperHeaders())
            {
                _headers.Remove(kvp.Key);
            }

            return this;
        }

        public IHttpRequest SetHeader(string key, string value)
        {
            _headers[key] = value;
            return this;
        }

        public IHttpRequest SetHeaders(IEnumerable<KeyValuePair<string, string>> headers)
        {
            foreach (var kvp in headers)
            {
                SetHeader(kvp.Key, kvp.Value);
            }

            return this;
        }

        public IHttpRequest OnUploadProgress(Action<float> onProgress)
        {
            _onUploadProgress += onProgress;
            return this;
        }

        public IHttpRequest OnDownloadProgress(Action<float> onProgress)
        {
            _onDownloadProgress += onProgress;
            return this;
        }

        public IHttpRequest OnSuccess(Action<HttpResponse> onSuccess)
        {
            _onSuccess += onSuccess;
            return this;
        }

        public IHttpRequest OnError(Action<HttpResponse> onError)
        {
            _onError += onError;
            return this;
        }

        public IHttpRequest OnNetworkError(Action<HttpResponse> onNetworkError)
        {
            _onNetworkError += onNetworkError;
            return this;
        }

        public bool RemoveHeader(string key)
        {
            return _headers.Remove(key);
        }

        public IHttpRequest SetTimeout(int duration)
        {
            UnityWebRequest.timeout = duration;
            return this;
        }
        
        public IHttpRequest SetRedirectLimit(int redirectLimit)
        {
            UnityWebRequest.redirectLimit = redirectLimit;
            return this;
        }

        public void Send()
        {
            foreach (var header in _headers)
            {
                UnityWebRequest.SetRequestHeader(header.Key, header.Value);
            }

            Http.Instance.Send(this, _onSuccess, _onError, _onNetworkError);
        }

        /// <summary>
        /// Sends the request using a Task. <br/>
        /// NOTE: OnSuccess, OnError and OnNetworkError events will be overriden. <br/>
        /// Use try/await/catch to handle success and errors.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponse"/> object in a Task
        /// </returns>
        /// <exception cref="HttpException">
        /// Thrown when the response status is either ConnectionError or ProtocolError
        /// </exception>
        public Task<HttpResponse> SendAsync()
        {
            var tcs = new TaskCompletionSource<HttpResponse>();
            _onSuccess = res => tcs.TrySetResult(res);
            _onError = res => tcs.TrySetException(new HttpException(res));
            _onNetworkError = res => tcs.TrySetException(new HttpException(res));

            Send();

            return tcs.Task;
        }

        public void Abort()
        {
            Http.Instance.Abort(this);
        }
        
        public void UpdateProgress()
        {
            UpdateProgress(ref _downloadProgress, UnityWebRequest.downloadProgress, _onDownloadProgress);
            UpdateProgress(ref _uploadProgress, UnityWebRequest.uploadProgress, _onUploadProgress);
        }

        private void UpdateProgress(ref float currentProgress, float progress, Action<float> onProgress)
        {
            if (currentProgress < progress)
            {
                currentProgress = progress;
                onProgress?.Invoke(currentProgress);
            }
        }
    }
}