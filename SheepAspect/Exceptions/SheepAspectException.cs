using System;

namespace SheepAspect.Exceptions
{
    public class SheepAspectException: ApplicationException
    {
        public SheepAspectException(string message): base(message)
        {
        }

        protected SheepAspectException(string message, Exception innerException): base(message, innerException)
        {
        }
    }
}