namespace Prolog
{
    public interface IPrologSettings
    {
        string PrologDirectory { get; }
        string PrologFileName { get; }
    }

    public class PrologService
    {
        private readonly IPrologSettings _settings;

        public PrologService(IPrologSettings settings)
        {
            _settings = settings;
        }


    }
}
