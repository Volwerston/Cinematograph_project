using System;
using System.Collections.Generic;

namespace NLP
{
    public class NLPResponse
    {
        public String IntentName { get; set; }
        public Dictionary<String, List<String>> Parameters { get; set; }
    }

    public interface NLPService
    {
        NLPResponse Ask(String question);
    }
}
