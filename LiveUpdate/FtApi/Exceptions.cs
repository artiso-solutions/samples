namespace FtApi
{
    using System;

    public class InitializationException : Exception
    {
        public InitializationException(string message)
            : base(message)
        {
        }
    }

    public class OperationException : Exception
    {
        public OperationException(string message)
            : base(message)
        {
        }
    }
}