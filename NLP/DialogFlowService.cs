using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;

namespace NLP
{
    public class DialogFlowService : NLPService
    {
        private const String CLIENT_TOKEN = "4adc566a8281443c8c852a9f45976107";
        private const String API_BASE_URL = "https://api.dialogflow.com/v1/";

        private const String DEFAULT_LANG = "en";
        private const String SESSION_ID = "88881466-112a-4349-a2f1-419177f6400c";


        public NLPResponse Ask(string question)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["query"] = question;

            queryString["sessionId"] = "SESSION_ID";
            queryString["lang"] = "DEFAULT_LANG";

            var endpointUri = API_BASE_URL + "query" + "?" + queryString;

            string responseStr = null;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {CLIENT_TOKEN}");

                var response = httpClient.GetAsync(endpointUri).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseStr = response.Content.ReadAsStringAsync().Result;
                }
            }

            if (responseStr == null)
            {
                return new NLPResponse();
            }

            return ParseResponse(responseStr);
        }

        private NLPResponse ParseResponse(String responseStr)
        {
            JObject response = JObject.Parse(responseStr);

            var parameters = new Dictionary<string, List<string>>();

            var result = response["result"];
            foreach (JProperty param in result["parameters"])
            {
                if (param.Value.GetType() == typeof(JArray))
                {
                    parameters.Add(param.Name, ((JArray)param.Value).ToObject<List<String>>());
                }
                else
                {
                    parameters.Add(param.Name, new List<string> { param.Value.ToString()});
                }
            }

            //TODO implement
            return new NLPResponse
            {
                IntentName = result["metadata"]["intentName"].ToString(),
                Parameters = parameters
            };
        }
    }
}
