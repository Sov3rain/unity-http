using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Networking.UnityWebRequest.Result;

namespace UnityHttp.Service.Unity
{
    public class UnityHttpService : IHttpService
    {
        public IHttpRequest Get(string uri)
        {
            return new UnityHttpRequest(UnityWebRequest.Get(uri));
        }

        public IHttpRequest GetTexture(string uri)
        {
            return new UnityHttpRequest(UnityWebRequestTexture.GetTexture(uri));
        }

        public IHttpRequest Post(string uri, string postData)
        {
            return new UnityHttpRequest(UnityWebRequest.PostWwwForm(uri, postData));
        }

        public IHttpRequest Post(string uri, WWWForm formData)
        {
            return new UnityHttpRequest(UnityWebRequest.Post(uri, formData));
        }

        public IHttpRequest Post(string uri, Dictionary<string, string> formData)
        {
            return new UnityHttpRequest(UnityWebRequest.Post(uri, formData));
        }

        public IHttpRequest Post(string uri, List<IMultipartFormSection> multipartForm)
        {
            return new UnityHttpRequest(UnityWebRequest.Post(uri, multipartForm));
        }

        public IHttpRequest Post(string uri, byte[] bytes, string contentType)
        {
            var unityWebRequest = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST)
            {
                uploadHandler = new UploadHandlerRaw(bytes)
                {
                    contentType = contentType
                },
                downloadHandler = new DownloadHandlerBuffer()
            };
            return new UnityHttpRequest(unityWebRequest);
        }

        public IHttpRequest PostJson(string uri, string json)
        {
            return Post(uri, Encoding.UTF8.GetBytes(json), "application/json");
        }

        public IHttpRequest Put(string uri, byte[] bodyData)
        {
            return new UnityHttpRequest(UnityWebRequest.Put(uri, bodyData));
        }

        public IHttpRequest Put(string uri, string bodyData)
        {
            return new UnityHttpRequest(UnityWebRequest.Put(uri, bodyData));
        }

        public IHttpRequest Delete(string uri)
        {
            return new UnityHttpRequest(UnityWebRequest.Delete(uri));
        }

        public IHttpRequest Head(string uri)
        {
            return new UnityHttpRequest(UnityWebRequest.Head(uri));
        }

        public IEnumerator Send(
            IHttpRequest request, 
            Action<HttpResponse> onSuccess = null,
            Action<HttpResponse> onError = null, 
            Action<HttpResponse> onNetworkError = null)
        {
            var unityHttpRequest = (UnityHttpRequest)request;
            var unityWebRequest = unityHttpRequest.UnityWebRequest;

            yield return unityWebRequest.SendWebRequest();

            var response = CreateResponse(unityWebRequest);

            if (unityWebRequest.result is ConnectionError)
            {
                onNetworkError?.Invoke(response);
            }
            else if (unityWebRequest.result is ProtocolError or DataProcessingError)
            {
                onError?.Invoke(response);
            }
            else
            {
                onSuccess?.Invoke(response);
            }
        }

        public void Abort(IHttpRequest request)
        {
            var unityHttpRequest = request as UnityHttpRequest;
            if (unityHttpRequest?.UnityWebRequest is { isDone: false })
            {
                unityHttpRequest.UnityWebRequest.Abort();
            }
        }

        private static HttpResponse CreateResponse(UnityWebRequest req) => new(
            url: req.url,
            statusCode: req.responseCode,
            result: req.result,
            data: req.downloadHandler?.data,
            text: req.downloadHandler?.text,
            error: req.error,
            headers: req.GetResponseHeaders(),
            texture: (req.downloadHandler as DownloadHandlerTexture)?.texture
        );
    }
}