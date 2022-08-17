using Discord;

namespace VROT.Logger
{
    public interface ILogger
    {
        public Task Log(LogMessage message);
    }
}