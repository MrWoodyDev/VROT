using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using static System.Guid;

namespace VROT.Logger
{
    public abstract class Logger : ILogger
    {
        public string _guid;
        public Logger()
        {

            _guid = NewGuid().ToString()[^4..];

        }

        public LogLevel Level { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Error(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Info(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public abstract Task Log(LogMessage message);

        public void Trace(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Warning(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        internal abstract SocketTextChannel GetChannel(object channelID);
    }
}