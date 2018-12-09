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

            service.Save(new PrologRule("loves(yura, it).")).GetAwaiter().GetResult();
            service.Save(new PrologRule("loves(yura, cinematograph).")).GetAwaiter().GetResult();
            
            var res = service.Execute(new PrologQuery("list_preferences(yura, X)."));

            Console.ReadKey();
        }
    }
}
