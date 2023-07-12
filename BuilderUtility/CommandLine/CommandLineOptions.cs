using CommandLine;

namespace BuilderUtility.CommandLine
{
    public class CommandLineOptions
    {
        [Value(index: 0, Required = true, HelpText = "Executing task name")]
        public string ExecutingTaskName { get; set; }

        [Option(shortName: 'p', longName: "path", Required = false, HelpText = "Path to the file with tasks", Default = "makefile.txt")]
        public string MakeFilePath { get; set; }
    }
}
