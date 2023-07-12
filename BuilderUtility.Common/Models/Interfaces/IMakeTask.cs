namespace BuilderUtility.Common.Models.Interfaces
{
    public interface IMakeTask
    {
        public string Name { get; }
        public IEnumerable<string> Dependencies { get; }

        public Task Run(StreamWriter writer);
    }
}
