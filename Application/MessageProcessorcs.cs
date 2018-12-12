using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLP;

namespace Application
{
    public class MessageProcessor
    {
        public NLPService NLPService = new DialogFlowService();

        public string Username = String.Empty;

        public string ProcessMessage(string message)
        {
            string response = string.Empty;

            NLPResponse nlpresponse = NLPService.Ask(message);

            switch (nlpresponse.IntentName)
            {
                case NLPIntentNames.DefaultFallbackIntent:
                    response = nlpresponse.Message;
                    break;
                case NLPIntentNames.DefaultWelcomeIntent:
                    response = nlpresponse.Message;
                    break;
                case NLPIntentNames.FavouriteGenresIntent:

                    break;
                case NLPIntentNames.GetMovieDescriptionIntent:
                    break;
                case NLPIntentNames.GetMovieRateIntent:
                    break;
                case NLPIntentNames.GetMovieRateContIntent:
                    break;
                case NLPIntentNames.GetMovieReleaseDateIntent:
                    break;
                case NLPIntentNames.GetMoviewReviewIntent:
                    break;
                case NLPIntentNames.GetMoviesByGenreIntent:
                    break;
                case NLPIntentNames.GetTopRatedMoviesIntent:
                    break;
                case NLPIntentNames.ThanksIntent:
                    response = nlpresponse.Message;
                    break;
                case NLPIntentNames.WantWatchMovieIntent:

                    break;
                case NLPIntentNames.WantWatchSpecificMovieIntent:
                    break;
                default:
                    response = "Sorry, I don't understand you :(";
                    break;
            }
            response = nlpresponse.Message;
            return response;
        }

        protected List<string> checkUserPreferences()
        {
            List<string> toRet = new List<string>();
            return toRet;
        }
    }
}
