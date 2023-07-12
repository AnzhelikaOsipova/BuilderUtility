using BuilderUtility.Common.Models.Interfaces;

namespace BuilderUtility.Common.Models
{
    public class MakeAction : IMakeAction
    {
        private readonly string _name;

        public MakeAction(string name) 
        {
            _name = name;
        }

        public async Task Run(StreamWriter writer)
        {
            await writer.WriteLineAsync($"\t{_name}");
        }
    }
}
