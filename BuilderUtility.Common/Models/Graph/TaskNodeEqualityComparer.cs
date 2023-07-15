using System.Diagnostics.CodeAnalysis;

namespace BuilderUtility.Common.Models.Graph
{
    public class TaskNodeEqualityComparer : IEqualityComparer<TaskNode>
    {
        public bool Equals(TaskNode? x, TaskNode? y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;
            if (x.Item.Name == x.Item.Name) return true;
            return false;
        }

        public int GetHashCode([DisallowNull] TaskNode obj)
        {
            return obj.Item.Name.GetHashCode();
        }
    }
}
