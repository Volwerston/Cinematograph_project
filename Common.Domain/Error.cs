using System;

namespace Common.Domain
{
    public class Error
    {
        public string Message { get; }

        public Error(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException($"'{nameof(message)}' cannot be null or white space");
            }

            Message = message;
        }
    }
}
