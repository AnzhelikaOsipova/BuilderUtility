using CommandLine;

namespace BuilderUtility.CommandLine
{
    public class CommandLineParser
    {
        private readonly StreamWriter _outputStream;

        public CommandLineParser(StreamWriter outputStream)
        {
            _outputStream = outputStream;
        }

        public CommandLineOptions? Parse(string[] args) 
        {
            try
            {
                return Parser.Default.ParseArguments<CommandLineOptions>(args).Value;
            }
            catch (Exception ex)
            {
                _outputStream.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
