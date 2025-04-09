using System;

namespace Duck.Http.Service.Unity
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