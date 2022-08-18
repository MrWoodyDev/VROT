using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using VROT.Services;
using Microsoft.AspNetCore.Builder;

namespace VROT
{
    public class Program
    {
        public static async Task Main()
        {
            var builder = WebApplication.CreateBuilder();

            builder.Host.ConfigureAppConfiguration(x =>
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsetting.json", false, true)
                    .Build();

                x.AddConfiguration(configuration);
            });
            builder.Host.ConfigureLogging(x =>
            {
                x.AddConsole();
                x.SetMinimumLevel(LogLevel.Debug);
            });
                builder.Host.ConfigureDiscordHost((context, config) => 
                {
                    config.SocketConfig = new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Debug,
                        AlwaysDownloadUsers = true,
                        LogGatewayIntentWarnings = false,
                        MessageCacheSize = 200,
                        GatewayIntents = GatewayIntents.All |
                                         GatewayIntents.AllUnprivileged
                    };

                    config.Token = context.Configuration["Token"];
                });
                builder.Host.UseCommandService((context, config) =>
                {
                    config.CaseSensitiveCommands = false;
                    config.LogLevel = LogSeverity.Debug;
                    config.DefaultRunMode = RunMode.Sync;
                });
                builder.Host.UseInteractionService((context, config) =>
                {
                    config.LogLevel = LogSeverity.Info;
                    config.UseCompiledLambda = false;
                });
                builder.Host.ConfigureServices((context, services) =>
                {
                    services.AddHostedService<CommandHandler>();
                    services.AddHostedService<InteractionHandler>();
                    services.AddHostedService<BotStatusService>();
                    services.AddHttpClient();
                    services.AddControllers();
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen(); 

                });
                builder.Host.UseConsoleLifetime();

            await using var host = builder.Build();


            host.UseSwagger();
            host.UseSwaggerUI(x => x.DisplayOperationId());
            host.MapControllers();

            await host.RunAsync();
        }
    }
}