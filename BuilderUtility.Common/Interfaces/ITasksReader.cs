using BuilderUtility.Common.Models.Interfaces;

namespace BuilderUtility.Common.Interfaces
{
    public interface ITasksReader
    {
        public Task<Dictionary<string, IMakeTask>?> ReadAsync();
    }
}
