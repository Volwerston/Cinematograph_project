using System;
using System.Collections.Generic;

namespace NLP
{
    public class NLPResponse
    {
        public String IntentName { get; set; }
        public String Message { get; set; }
        public Dictionary<String, List<String>> Parameters { get; set; }
    }

    public static class NLPIntentNames
    {
        public const string DefaultFallbackIntent = "Default Fallback Intent";
        public const string DefaultWelcomeIntent = "Default Welcome Intent";
        public const string FavouriteGenresIntent = "FavouriteGenres";
        public const string GetMovieDescriptionIntent = "GetMovieDescription";
        public const string GetMovieRateIntent = "GetMovieRate";
        public const string GetMovieRateContIntent = "GetMovieRateCont";
        public const string GetMovieReleaseDateIntent = "GetMovieReleaseDate";
        public const string GetMoviewReviewIntent = "GetMoviewReview";
        public const string GetMoviesByGenreIntent = "GetMoviesByGenre";
        public const string GetTopRatedMoviesIntent = "GetTopRatedMovies";
        public const string ThanksIntent = "ThanksIntent";
        public const string WantWatchMovieIntent = "WantWatchMovie";
        public const string WantWatchSpecificMovieIntent = "WantWatchSpecificMovie";
    }

    public interface NLPService
    {
        NLPResponse Ask(String question);
    }
}
