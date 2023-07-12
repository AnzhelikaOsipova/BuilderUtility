using BuilderUtility.Common.Models.Interfaces;

namespace BuilderUtility.Common.Models
{
    public class MakeTask : IMakeTask
    {
        private IEnumerable<IMakeAction> _actions;
        public string Name { get; }
        public IEnumerable<string> Dependencies { get; }

        public MakeTask(string name, IEnumerable<string> dependencies)
        {
            Name = name;
            Dependencies = dependencies;
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
