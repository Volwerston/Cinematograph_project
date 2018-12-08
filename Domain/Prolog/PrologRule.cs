using System;

namespace Domain.Prolog
{
    public class PrologRule
    {
        public string Rule { get; }

        public PrologRule(string rule)
        {
            if (string.IsNullOrWhiteSpace(rule))
            {
                throw new ArgumentException($"'{nameof(rule)}' cannot be null or white space");
            }

            Rule = rule;
        }
    }
}
