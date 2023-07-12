using BuilderUtility.Common.Models.Interfaces;

namespace BuilderUtility.Common.Interfaces
{
    public interface ITasksReader
    {
        public Task<IEnumerable<IMakeTask>?> ReadAsync();
    }
}
