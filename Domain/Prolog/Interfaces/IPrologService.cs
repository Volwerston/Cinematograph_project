using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Domain;

namespace Domain.Prolog.Interfaces
{
    public interface IPrologService
    {
        Task<Result> Save(PrologRule rule);

        Result<IReadOnlyList<PrologResponse>> Execute(PrologQuery query);
    }
}
