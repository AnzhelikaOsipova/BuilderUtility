namespace BuilderUtility.Common.Models.Interfaces
{
    public interface IMakeAction
    {
        public Task Run(StreamWriter writer);
    }
}
