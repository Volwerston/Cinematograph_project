using System;
using System.Linq;

namespace Domain.Prolog
{
    public static class PrologQueryFactory
    {
        public static PrologQuery Likes(string user)
        {
            var processedUser = new string(
                user
                .Trim()
                .Where(ch => char.IsLetter(ch) ||
                             char.IsWhiteSpace(ch))
                .ToArray());

            return new PrologQuery($@"list_preferences(""{processedUser}"", X).");
        }

        public static PrologQuery Recommended(string user)
        {
            var processedUser = new string(
                user
                    .Trim()
                    .Where(ch => char.IsLetter(ch) || 
                                 char.IsWhiteSpace(ch))
                    .ToArray());

            return new PrologQuery($@"list_recommended(""{processedUser}"", X).");
        }
    }


    public class PrologQuery
    {
        public string Query { get; }

        internal PrologQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException($"'{nameof(query)}' cannot be null or white space");
            }

            Query = query;
        }
    }
}
