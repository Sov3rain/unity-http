using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;
using static UnityEngine.Networking.UnityWebRequest.Result;

namespace UnityHttp.Service
{
    public class HttpResponse
    {
        public HttpResponse(
            string url,
            long statusCode,
            Result result,
            byte[] data,
            string text,
            string error,
            Texture texture,
            Dictionary<string, string> headers)
        {
            Url = url;
            StatusCode = statusCode;
            Result = result;
            Data = data;
            Text = text;
            Error = error;
            Texture = texture;
            Headers = headers;
        }

        public string Url { get; }
        public long StatusCode { get; }
        public byte[] Data { get; }
        public string Text { get; }
        public string Error { get; }
        public Texture Texture { get; }
        public Dictionary<string, string> Headers { get; }
        
        public bool IsSuccessful => Result is Success;
        public bool IsHttpError => Result is ProtocolError or DataProcessingError;
        public bool IsNetworkError => Result is ConnectionError;
        
        private Result Result { get; }
    }
}
