using BuilderUtility.Common.Interfaces;
using BuilderUtility.Common.Models;
using BuilderUtility.Common.Models.Interfaces;

namespace BuilderUtility.Common.FileReader
{
    public class FileReader : ITasksReader
    {
        private readonly StreamWriter _outputStream;
        private readonly string _makefilePath;

        public FileReader(StreamWriter outputStream, string makefilePath) 
        {
            _outputStream = outputStream;
            _makefilePath = makefilePath;
        }

        public async Task<IEnumerable<IMakeTask>?> ReadAsync() 
        {
            if (!File.Exists(_makefilePath))
            {
                await _outputStream.WriteLineAsync("Unable to read the file.");
                return null;
            }

            var makeTasks = new List<MakeTask>();
            var tasksNames = new List<string>();
            var makeActions = new List<IMakeAction>();
            using var reader = new StreamReader(_makefilePath);

            var line = await reader.ReadLineAsync();
            if (line is not null && line.StartsWith(' '))
            {
                await _outputStream.WriteLineAsync("File cannot start with an action.");
                return null;
            }
            while (line is not null)
            {
                if (line.StartsWith(' ') || line.StartsWith('\t'))
                {
                    makeActions.Add(new MakeAction(line.Trim()));
                }
                else
                {
                    if (makeTasks.Count > 0)
                    {
                        makeTasks[makeTasks.Count - 1].AddActions(makeActions);
                        makeActions = new List<IMakeAction>();
                    }
                    var names = line.Split(':');
                    if ((names.Length > 2))
                    {
                        await _outputStream.WriteLineAsync($"Error while parsing the line {line} - expected only one \":\".");
                        return null;
                    }
                    var name = names[0].Trim();
                    if (string.IsNullOrEmpty(name))
                    {
                        await _outputStream.WriteLineAsync($"Error while parsing the line {line} - task's name cannot be empty.");
                        return null;
                    }
                    if (tasksNames.Contains(name))
                    {
                        await _outputStream.WriteLineAsync($"Error while parsing the line {line} - task's names must be unique.");
                        return null;
                    }
                    var dependencies = names.Length == 2 ?
                        names[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        : Array.Empty<string>();
                    makeTasks.Add(new MakeTask(name, dependencies));
                    tasksNames.Add(name);
                }
                line = reader.ReadLine();
            }
            if (makeTasks.Count > 0)
            {
                makeTasks[makeTasks.Count - 1].AddActions(makeActions);
            }
            return makeTasks;
        }
    }
}
