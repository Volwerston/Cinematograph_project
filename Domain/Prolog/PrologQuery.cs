using System;
namespace Domain.Prolog
{
    public class PrologQuery
    {
        public string Query { get; }

        public PrologQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException($"'{nameof(query)}' cannot be null or white space");
            }

            Query = query;
        }
    }
}
