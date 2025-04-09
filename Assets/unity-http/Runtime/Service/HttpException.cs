using System;
using UnityHttp.Service;

namespace UnityHttp
{
    public class HttpException : Exception
    {
        public HttpException(HttpResponse response) : base(response.Error)
        {
            Response = response;
        }
        
        public HttpResponse Response { get; }
    }
}