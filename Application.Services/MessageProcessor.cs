using Domain.Model;
using Domain.Prolog;
using NLP;
using Prolog;
using Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using Prolog.Test;

namespace Application
{
    public class MessageProcessor
    {
        private NLPService NLPService = new DialogFlowService();

        protected MoviesRepository movieRepository = new MoviesRepository();
        protected PrologService prologService = new PrologService(new PrologSettings());

        public string Username = String.Empty;

        public string ProcessMessage(string message)
        {
            string response = string.Empty;

            NLPResponse nlpresponse = NLPService.Ask(message);
            List<Movie> searchResult;

            switch (nlpresponse.IntentName)
            {
                case NLPIntentNames.DefaultFallbackIntent:
                    response = nlpresponse.Message;
                    break;
                case NLPIntentNames.DefaultWelcomeIntent:
                    response = nlpresponse.Message;
                    break;
                case NLPIntentNames.FavouriteGenresIntent:
                    try
                    {
                        saveUserPreferences(nlpresponse.Parameters["Genre"]);
                        response = nlpresponse.Message;
                    }
                    catch (Exception exc)
                    {
                        response = exc.Message;
                    }

                    break;
                case NLPIntentNames.GetMovieDescriptionContIntent:
                case NLPIntentNames.GetMovieDescriptionIntent:
                    searchResult = movieRepository.SearchByTitle(nlpresponse.Parameters["MovieName"][0]);
                    if (searchResult.Count == 0)
                        response = $"Sorry, I know nothing about the film {nlpresponse.Parameters["MovieName"][0]}";
                    else
                        foreach (var movie in searchResult)
                        {
                            response += $"{movie.Title}\tDescription:\n{movie.Plot}\n\n";
                        }

                    break;
                case NLPIntentNames.GetMovieRateContIntent:
                case NLPIntentNames.GetMovieRateIntent:
                    searchResult = movieRepository.SearchByTitle(nlpresponse.Parameters["MovieName"][0]);
                    if (searchResult.Count == 0)
                        response = $"Sorry, I know nothing about the film {nlpresponse.Parameters["MovieName"][0]}";
                    else
                        foreach (var movie in searchResult)
                        {
                            response +=
                                $"{movie.Title}\tMetacritic: {movie.MetacriticRate}\tTomato: {movie?.TomatoRate?.Rating.ToString() ?? "N/A"}\t " +
                                $"Imdb: {movie?.ImdbRate?.Rating.ToString() ?? "N/A"}\n";
                        }

                    break;
                case NLPIntentNames.GetMovieReleaseDateContIntent:
                case NLPIntentNames.GetMovieReleaseDateIntent:
                    searchResult = movieRepository.SearchByTitle(nlpresponse.Parameters["MovieName"][0]);
                    if (searchResult.Count == 0)
                        response = $"Sorry, I know nothing about the film {nlpresponse.Parameters["MovieName"][0]}";
                    else
                        foreach (var movie in searchResult)
                        {
                            response += $"{movie.Title}\tRelease Date: {movie.Year}\n";
                        }

                    break;
                //Obsolete
                /*case NLPIntentNames.GetMoviewReviewIntent:
                    break;
                case NLPIntentNames.GetMoviewReviewContIntent:
                    break;
                    */
                case NLPIntentNames.GetMoviesByGenreIntent:
                    var genreToFind = nlpresponse.Parameters["Genre"][0];
                    searchResult = movieRepository.SearchByGenre(new List<string>() { genreToFind },
                        new PaginationRequest() { Page = 1, Size = 1000 }).Data;
                    //remove already recommended 
                    var alreadyRecommended = prologService.Execute(PrologQueryFactory.Recommended(Username)).Value;
                    searchResult = searchResult?.Where(x =>
                        !alreadyRecommended.Any(y => y.Response.ToLower() == x.Title.ToLower()))?.ToList();
                    if (searchResult.Count > 0)
                    {
                        for (int i = 0; i < Math.Min(searchResult.Count, 5); i++)
                        {
                            response += $"{i + 1}. {searchResult[i].Title}";
                        }
                    }
                    else
                    {
                        response = "Sorry I can not find some fresh films for you :(";
                    }

                    break;
                case NLPIntentNames.GetTopRatedMoviesIntent:
                    List<Movie> movies = movieRepository.FindAll().OrderByDescending(x => x.ImdbRate.Rating)
                        .ToList();
                    //remove recommended
                    var alreadyRecommendedTOPRated =
                        prologService.Execute(PrologQueryFactory.Recommended(Username)).Value;
                    movies = movies?.Where(x =>
                        !alreadyRecommendedTOPRated.Any(y => y.Response.ToLower() == x.Title.ToLower()))?.ToList();
                    if (movies.Count == 0)
                    {
                        response = "Sorry, I have no fresh films for you :(";
                        break;
                    }

                    movies = movies.Take(Math.Min(movies.Count, 5)).ToList();
                    response = "I would recommend:\n";
                    for (int i = 0; i < movies.Count; i++)
                    {
                        response +=
                            $"{i + 1}. {movies[i].Title} - Imdb: {movies[i].ImdbRate.Rating};\tMetacritic: {movies[i].MetacriticRate};\tTomato: {movies[i].TomatoRate.Rating}\n";
                        prologService.Save(PrologRuleFactory.Recommended(Username, movies[i].Title));
                    }

                    break;
                case NLPIntentNames.ThanksIntent:
                    response = nlpresponse.Message;
                    break;
                case NLPIntentNames.WantWatchMovieIntent:
                    List<string> preferences = checkUserPreferences();
                    if (preferences.Count == 0)
                        response = "I don't know about your preferences. Please tell me what genres do you prefer.";
                    else
                    {
                        var rec = recommendRandomMovieByPreferences(preferences);

                        response = rec.Length > 0
                            ? $"I would like to recommend {rec}"
                            : "I already recommended you all films. Please name me another genres you might likes.";

                    }

                    break;
                //case NLPIntentNames.WantWatchSpecificMovieIntent:
                //    break;
                default:
                    response = "Sorry, I don't understand you :(";
                    break;
            }

            return response;
        }

        protected List<string> checkUserPreferences()
        {
            List<string> toRet = new List<string>();

            var preferences = prologService.Execute(PrologQueryFactory.Likes(Username));
            if (preferences.IsSuccess)
                toRet = preferences.Value.Select(x => x.Response).ToList();
            return toRet;
        }

        protected void saveUserPreferences(List<string> genres)
        {
            foreach (var genre in genres)
            {
                prologService.Save(PrologRuleFactory.Likes(Username, genre))
                .GetAwaiter()
                .GetResult();
            }
        }
        protected string recommendRandomMovieByPreferences(List<string> preferences)
        {
            List<Movie> toRecommend =
                    movieRepository.SearchByGenre(preferences, new PaginationRequest() { Page = 1, Size = 1000 }).Data;
            //remove already recommended
            var alreadyRecommended = prologService.Execute(PrologQueryFactory.Recommended(Username)).Value;
            toRecommend = toRecommend?.Where(x => !alreadyRecommended.Any(y => y.Response.ToLower() == x.Title.ToLower()))?.ToList();
            //randomize
            toRecommend = toRecommend?.OrderBy(x => Guid.NewGuid())?.ToList();
            if (toRecommend.Count > 0)
            {
                //write down recommended
                prologService.Save(PrologRuleFactory.Recommended(Username, toRecommend[0].Title));

                return toRecommend[0].Title;
            }

            else
                return String.Empty;
        }
    }
    class PrologSettings : IPrologSettings
    {
        public string ExecutablePath => @"C:\Program Files\swipl\bin\swipl.exe";

        public string ProgramPath => $@"{Directory.GetCurrentDirectory()}\exec.pl";

        public string KnowledgeBasePath => $@"{Directory.GetCurrentDirectory()}\db.pl";
    }
}
