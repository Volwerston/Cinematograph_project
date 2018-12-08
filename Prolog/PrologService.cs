using Common.Domain;
using Domain.Prolog;
using Domain.Prolog.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SbsSW.SwiPlCs;

namespace Prolog
{
    public interface IPrologSettings
    {
        string PrologDirectory { get; }
        string PrologFileName { get; }
    }

    public class PrologService : IPrologService, IDisposable
    {
        private readonly IPrologSettings _settings;

        public PrologService(IPrologSettings settings)
        {
            _settings = settings;

            InitializeProlog(_settings.PrologDirectory);
        }

        public Result<IReadOnlyList<PrologResponse>> Execute(PrologQuery query)
        {
            using (var q = new PlQuery(query.Query))
            {
                return q.SolutionVariables
                    .Select(
                        variable => new PrologResponse(variable["C"].ToString()))
                    .ToList();
            }
        }

        public async Task<Result> Save(PrologRule rule)
        {
            using (var fs = new FileStream($"{_settings.PrologDirectory}/{_settings.PrologFileName}",
                FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var sw = new StreamWriter(fs))
                {
                    await sw.WriteLineAsync(rule.Rule);
                }
            }
            
            return Result.Ok();
        }

        private static void InitializeProlog(string homeDir)
        {
            Environment.SetEnvironmentVariable("SWI_HOME_DIR", homeDir);

            var param = new[] { "-q" };  // suppressing informational and banner messages
            PlEngine.Initialize(param);
        }

        public void Dispose()
        {
            PlEngine.PlCleanup();
        }
    }
}
