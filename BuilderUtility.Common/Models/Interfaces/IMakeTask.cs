namespace BuilderUtility.Common.Models.Interfaces
{
    public interface IMakeTask
    {
        public string Name { get; }
        public HashSet<string> Dependencies { get; }

        public Task Run(StreamWriter writer);
        public void AddActions(IEnumerable<IMakeAction> actions);
    }
}
