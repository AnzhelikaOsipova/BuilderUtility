using BuilderUtility.Common.Models.Interfaces;

namespace BuilderUtility.Common.Models.Graph
{
    public class TaskNode
    {
        public IMakeTask Item { get; set; }

        public List<TaskNode> Dependencies { get; set; } = new List<TaskNode>();

        public NodeStatus Status { get; set; } = NodeStatus.NotStarted;

        public TaskNode(IMakeTask item)
        {
            Item = item;
        }
    }
}
