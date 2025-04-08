using System;

namespace Duck.Http.Service
{
    public class ConnectionException : Exception
    {
        public HttpResponse Error { get; }

        public ConnectionException(HttpResponse error)
        {
            Error = error;
        }
    }
}