namespace Solid.Core.Exceptions
{
    using System;

    public class MaximumRetryExceededException : Exception
    {
        public MaximumRetryExceededException(int maximumRetryCount)
            : base($"Maximum retry attempts of {maximumRetryCount} exceeded for operation.")
        {
        }
    }
}
