using Discord;

namespace VROT.Log
{
    public interface ILogger
    {
        public Task Log(LogMessage message);
    }
}