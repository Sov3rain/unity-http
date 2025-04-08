using System;

namespace Duck.Http.Service
{
    public class ProtocolException : Exception
    {
        public HttpResponse Error { get; }

        public ProtocolException(HttpResponse error)
        {
            Error = error;
        }
    }
}