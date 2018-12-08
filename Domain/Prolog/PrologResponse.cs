using System;

namespace Domain.Prolog
{
    public class PrologResponse
    {
        public string Response { get; }

        public PrologResponse(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
            {
                throw new ArgumentException($"'{nameof(response)}' cannot be null or white space");
            }

            Response = response;
        }
    }
}
