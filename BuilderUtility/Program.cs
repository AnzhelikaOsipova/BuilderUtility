using BuilderUtility.CommandLine;
using BuilderUtility.Common.FileReader;
using BuilderUtility.Common.Utility;

using var stream = new StreamWriter(Console.OpenStandardOutput());
var parser = new CommandLineParser(stream);
var parsedArgs = parser.Parse(args);
if (parsedArgs is null)
{
    return;
}

var utility = new TasksUtility(stream, new FileReader(stream, parsedArgs.MakeFilePath));
await utility.ExecuteAsync(parsedArgs.ExecutingTaskName);