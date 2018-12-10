using System;
using System.IO;
using Domain.Prolog;

namespace Prolog.Test
{
    public class PrologSettings : IPrologSettings
    {
        public string ExecutablePath => @"C:\Program Files\swipl\bin\swipl.exe";

        public string ProgramPath => $@"{Directory.GetCurrentDirectory()}\exec.pl";

        public string KnowledgeBasePath => $@"{Directory.GetCurrentDirectory()}\db.pl";
    }

    class Program
    {
        static void Main(string[] args)
        {
            var service = new PrologService(new PrologSettings());

            /* Preferred genres */
            service.Save(PrologRuleFactory.Likes("Yurii Stetskyi", "Horror film"))
                .GetAwaiter()
                .GetResult();

            service.Save(PrologRuleFactory.Likes("Yurii Stetskyi", "Documentary"))
                .GetAwaiter()
                .GetResult();

            var preferences = service.Execute(PrologQueryFactory.Likes("Yurii Stetskyi"));

            if (preferences.IsSuccess)
            {
                Console.WriteLine("Preferences:");
                foreach (var preference in preferences.Value)
                {
                    Console.WriteLine(preference.Response);
                }
            }
            else
            {
                Console.WriteLine($"ERROR: {preferences.Error.Message}");
            }

            /* Recommended films */
            service.Save(PrologRuleFactory.Recommended("Yurii Stetskyi", "Mickey Mouse Club House"))
                .GetAwaiter()
                .GetResult();

            service.Save(PrologRuleFactory.Recommended("Yurii Stetskyi", "Terminator"))
                .GetAwaiter()
                .GetResult();

            var recommendations = service
                .Execute(PrologQueryFactory.Recommended("Yurii Stetskyi"));

            if (recommendations.IsSuccess)
            {
                Console.WriteLine("Recommendations:");

                foreach (var recommendation in recommendations.Value)
                {
                    Console.WriteLine(recommendation.Response);
                }
            }
            else
            {
                Console.WriteLine($"ERROR: {recommendations.Error.Message}");
            }


            Console.ReadKey();
        }
    }
}
