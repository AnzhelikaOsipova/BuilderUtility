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
        private List<TaskNode> _graph;
        private bool _reUsing = false;

        public TasksUtility(StreamWriter outputStream, ITasksReader tasksReader) 
        {
            _outputStream = outputStream;
            _tasksReader = tasksReader;
        }

        public async Task<bool> TryExecuteAsync(string startTaskName)
        {
            if (_tasks is null)
            {
                if ((await TryReadTasksAsync()) == false) return false;
            }

            _graph = _tasks!.BuildGraph(startTaskName);
            var tasksOrder = await BuildTasksOrderAsync(startTaskName);
            if (tasksOrder is null) return false;

            while (tasksOrder.Count > 0)
            {
                var task = tasksOrder.Dequeue();
                await task.Run(_outputStream);
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

        private async Task<Queue<IMakeTask>?> BuildTasksOrderAsync(string startTaskName)
        {
            if (_reUsing)
            {
                _graph.ForEach(task => task.Status = NodeStatus.NotStarted);
            }
            var startNode = _graph.Where(node => node.Item.Name == startTaskName).FirstOrDefault();
            if (startNode is null)
            {
                await _outputStream.WriteLineAsync($"Failed to create tasks order - task with name {startTaskName} does not exist.");
                return null;
            }

            _reUsing = true;
            var stack = new Stack<TaskNode>();
            var tasksOrder = new Queue<IMakeTask>();
            stack.Push(startNode);

            while (stack.Count > 0) 
            {
                var task = stack.Pop();
                switch (task.Status) 
                {
                    case NodeStatus.NotStarted:
                        if (!task.Dependencies.Any())
                        {
                            task.Status = NodeStatus.Passed;
                            tasksOrder.Enqueue(task.Item);
                            break;
                        }
                        task.Status = NodeStatus.InProgress;
                        stack.Push(task);
                        foreach (var dependency in task.Dependencies)
                        {
                            if (dependency.Status == NodeStatus.InProgress)
                            {
                                await _outputStream.WriteLineAsync("Failed to build tasks - cycle found.");
                                return null;
                            }
                            if (dependency.Status != NodeStatus.Passed)
                            {
                                stack.Push(dependency);
                            }
                        }
                        break;
                    case NodeStatus.InProgress:
                        task.Status = NodeStatus.Passed;
                        tasksOrder.Enqueue(task.Item);
                        break;
                    case NodeStatus.Passed: 
                        break;
                }
            }
            return tasksOrder;
        }
    }
}
