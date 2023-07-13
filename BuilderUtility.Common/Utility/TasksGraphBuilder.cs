using BuilderUtility.Common.Models.Graph;
using BuilderUtility.Common.Models.Interfaces;
using System.Runtime.CompilerServices;

namespace BuilderUtility.Common.Utility
{
    public static class TasksGraphBuilder
    {
        public static List<TaskNode> BuildGraph(this List<IMakeTask> tasks, string startTaskName)
        {
            var graph = new List<TaskNode>();
            var stack = new Stack<string>();
            
            stack.Push(startTaskName);
            while (stack.Count > 0) 
            {
                var taskName = stack.Pop();
                if (!graph.Any(node => node.Item.Name == taskName))
                {
                    var task = tasks.First(x => x.Name == taskName);
                    graph.Add(new TaskNode(task));
                    foreach(var dependency in task.Dependencies)
                    {
                        stack.Push(dependency);
                    }
                }
            }
            for (int i = 0; i < graph.Count; i++) 
            {
                for (int j = 0; j < graph.Count; j++)
                {
                    if (graph[j].Item.Dependencies.Contains(graph[i].Item.Name))
                    {
                        graph[j].Dependencies.Add(graph[i]);
                    }
                }
            }
            return graph;
        }
    }
}
