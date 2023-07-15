using BuilderUtility.Common.Models.Interfaces;

namespace BuilderUtility.Common.Models
{
    public class MakeTask : IMakeTask
    {
        private IEnumerable<IMakeAction> _actions;
        public string Name { get; }
        public HashSet<string> Dependencies { get; }

        public MakeTask(string name, string[] dependencies)
        {
            Name = name;
            Dependencies = new HashSet<string>(dependencies);
        }

        public void AddActions(IEnumerable<IMakeAction> actions)
        {
            _actions = actions;
        }

        public async Task Run(StreamWriter writer)
        {
            await writer.WriteLineAsync(Name);
            foreach (var action in _actions)
            {
                await action.Run(writer);
            }
        }
    }
}
