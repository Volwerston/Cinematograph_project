using Common.Domain;
using Domain.Prolog;
using Domain.Prolog.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Prolog
{
    public interface IPrologSettings
    {
        string ExecutablePath { get; }
        string ProgramPath { get; }
        string KnowledgeBasePath { get; }
    }

    public class PrologService : IPrologService
    {
        private readonly IPrologSettings _settings;

        public PrologService(IPrologSettings settings)
        {
            _settings = settings;
        }

        public Result<IReadOnlyList<PrologResponse>> Execute(PrologQuery query)
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = _settings.ExecutablePath,
                    Arguments = GetPrologQuery(query.Query),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };

            process.Start();

            var output = process.StandardOutput.ReadToEnd();

            return output.Trim()
                .Split(new[] { Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(str => new PrologResponse(str))
                .ToArray();
        }

        public async Task<Result> Save(PrologRule rule)
        {
            using (var fs = new FileStream(_settings.KnowledgeBasePath,
                FileMode.Append, FileAccess.Write))
            {
                using (var sw = new StreamWriter(fs))
                {
                    await sw.WriteLineAsync(rule.Rule);
                }
            }
            
            return Result.Ok();
        }

        private string GetPrologQuery(string query) 
            => $@"-s ""{_settings.ProgramPath}"" -g ""{ProcessQuery(query)}"" -t halt.";

        private static string ProcessQuery(string query) 
            => string.Join(@"\""", query.Split('"'));
    }
}
