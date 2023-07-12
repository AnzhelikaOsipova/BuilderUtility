namespace BuilderUtility.Common.Models.Graph
{
    public class TaskNode
    {
        public int Index { get; set; }

        public IEnumerable<int> Dependencies { get; set; }

        public NodeStatus Status { get; set; } = NodeStatus.NotStarted;

        public TaskNode(int index, IEnumerable<int> dependencies)
        {
            Index = index;
            Dependencies = dependencies;
        }
    }
}
