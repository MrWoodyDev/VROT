using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace VROT.Services
{
    public class BotStatusService : DiscordClientService
    {
        public BotStatusService(DiscordSocketClient client, ILogger<DiscordClientService> logger) : base(client, logger)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Client.WaitForReadyAsync(stoppingToken);
            Logger.LogInformation("Client is ready!");

            while (true)
            {
                await Client.SetGameAsync("I love Discord");
                Thread.Sleep(5000);
                await Client.SetGameAsync("I love Discord.NET");
                Thread.Sleep(5000);

            }
        }
    }
}
