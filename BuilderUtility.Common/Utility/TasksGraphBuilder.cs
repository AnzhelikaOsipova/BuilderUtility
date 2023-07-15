using BuilderUtility.Common.Models.Graph;
using BuilderUtility.Common.Models.Interfaces;

namespace BuilderUtility.Common.Utility
{
    public static class TasksGraphBuilder
    {
        public static async Task<TaskNode?> BuildGraph(this Dictionary<string, IMakeTask> tasks, string startTaskName, StreamWriter outputStream)
        {
            var graph = new HashSet<TaskNode>(new TaskNodeEqualityComparer());
            var stack = new Stack<string>();
            TaskNode? startNode = null;
            
            if (!tasks.ContainsKey(startTaskName))
            {
                await outputStream.WriteLineAsync($"Failed building graph - task with name {startTaskName} does not exist.");
                return null;
            }
            stack.Push(startTaskName);
            while (stack.Count > 0)
            {
                var taskName = stack.Pop();
                var task = tasks[taskName];
                var node = new TaskNode(task);
                if (taskName == startTaskName) startNode = node;
                if (graph.Add(node))
                {
                    foreach (var dependency in task.Dependencies)
                    {
                        stack.Push(dependency);
                    }
                }
            }
            int cnt = 0;
            foreach (var nodeI in graph) 
            {
                foreach (var nodeJ in graph)
                {
                    if (nodeJ.Item.Dependencies.Contains(nodeI.Item.Name))
                    {
                        nodeJ.Dependencies.Add(nodeI);
                    }
                }
                cnt++;
            }
            return startNode!;
        }
    }
}
