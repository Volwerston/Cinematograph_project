using System;
using System.Linq;

namespace Domain.Prolog
{
    public static class PrologRuleFactory
    {
        public static PrologRule Likes(string user, string genre)
        {
            var processedUser = new string(
                user
                    .Trim()
                    .Where(ch => char.IsLetter(ch) ||
                                 char.IsWhiteSpace(ch))
                    .ToArray());

            var processedGenre = new string(
                 genre
                     .Trim()
                     .Where(ch => char.IsLetterOrDigit(ch) ||
                                  char.IsWhiteSpace(ch))
                     .ToArray());
            
            return new PrologRule($@"likes(""{processedUser}"", ""{processedGenre}"").");
        }

        public static PrologRule Recommended(string user, string genre)
        {
            var processedUser = new string(
                user
                    .Trim()
                    .Where(ch => char.IsLetter(ch) ||
                                 char.IsWhiteSpace(ch))
                    .ToArray());

            var processedGenre = new string(
                genre
                    .Trim()
                    .Where(ch => char.IsLetterOrDigit(ch) ||
                                 char.IsWhiteSpace(ch))
                    .ToArray());

            return new PrologRule($@"recommended(""{processedUser}"", ""{processedGenre}"").");
        }
    }

    public class PrologRule
    {
        public string Rule { get; }

        internal PrologRule(string rule)
        {
            if (string.IsNullOrWhiteSpace(rule))
            {
                throw new ArgumentException($"'{nameof(rule)}' cannot be null or white space");
            }

            Rule = rule;
        }
    }
}
