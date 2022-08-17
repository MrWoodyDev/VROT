using Discord;
using Discord.WebSocket;

namespace VROT.Logger
{
    public class ConsoleLogger : Logger
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task Log(LogMessage message)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(() => LogToConsoleAsync(this, message));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task LogToConsoleAsync<T>(T logger, LogMessage message) where T : ILogger
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            Console.WriteLine($"Guild:{_guid} : " + message);
        }

        internal override SocketTextChannel GetChannel(object channelID)
        {
            throw new NotImplementedException();
        }
    }
}