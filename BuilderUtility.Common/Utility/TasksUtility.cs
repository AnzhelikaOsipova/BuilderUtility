using BuilderUtility.Common.Interfaces;
using BuilderUtility.Common.Models.Graph;
using BuilderUtility.Common.Models.Interfaces;

namespace BuilderUtility.Common.Utility
{
    public class TasksUtility
    {
        private readonly StreamWriter _outputStream;
        private readonly ITasksReader _tasksReader;

        private List<IMakeTask>? _tasks;
        private Queue<int> _queue;

        public TasksUtility(StreamWriter outputStream, ITasksReader tasksReader) 
        {
            _outputStream = outputStream;
            _tasksReader = tasksReader;
            _queue = new Queue<int>();
        }

        public async Task<bool> ExecuteAsync(string startTaskName)
        {
            if (!(await TryBuildTasksAsync(startTaskName))) return false;

            while(_queue.Count > 0)
            {
                var taskIndex = _queue.Dequeue();
                await _tasks![taskIndex].Run(_outputStream);
            }
            return true;
        }

        private async Task<bool> TryReadTasksAsync()
        {
            var tasks = await _tasksReader.ReadAsync();
            if (tasks is null)
            {
                await _outputStream.WriteLineAsync("Failed reading tasks.");
                return false;
            }
            _tasks = tasks.ToList();
            return true;
        }

        private async Task<bool> TryBuildTasksAsync(string startTaskName)
        {
            if (_tasks is null)
            {
                if ((await TryReadTasksAsync()) == false) return false;
            }
            var taskNames = _tasks!.Select(task => task.Name).ToList();
            var graph = await BuildGraphAsync(taskNames);
            if (graph is null) return false;

            if (_queue.Count > 0)
            {
                _queue.Clear();
            }
            var startTaskIndex = taskNames.IndexOf(startTaskName);
            var startNode = graph.Where(node => node.Index == startTaskIndex).SingleOrDefault();

            return await TryMakeTasksOrderAsync(graph[startTaskIndex], graph);
        }

        private async Task<List<TaskNode>?> BuildGraphAsync(List<string> taskNames)
        {
            var graph = new List<TaskNode>();
            int counter = 0;
            foreach (var task in _tasks!)
            {
                var dependencyIndexes = new List<int>();
                foreach (var dependency in task.Dependencies)
                {
                    var index = taskNames.IndexOf(dependency);
                    if (index == -1)
                    {
                        await _outputStream.WriteLineAsync($"Failed to build the tasks' graph - the task with the name {dependency} does not exist.");
                        return null;
                    }
                    dependencyIndexes.Add(index);
                }
                graph.Add(new TaskNode(counter, dependencyIndexes));
                counter++;
            }
            return graph;
        }

        private async Task<bool> TryMakeTasksOrderAsync(TaskNode task, List<TaskNode> graph)
        {
            if (task.Status == NodeStatus.Passed) return true;
            if (task.Status == NodeStatus.InProgress)
            {
                await _outputStream.WriteLineAsync("Failed to build tasks - cycle found.");
                return false;
            }
            
            task.Status = NodeStatus.InProgress;
            foreach (var dependency in task.Dependencies)
            {
                if(!(await TryMakeTasksOrderAsync(graph[dependency], graph))) return false;
            }
            task.Status = NodeStatus.Passed;
            _queue.Enqueue(task.Index);
            return true;
        }
    }
}
