namespace VROT.Services
{
    using System;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Addons.Hosting;
    using Discord.Commands;
    using Discord.WebSocket;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The class responsible for handling the commands and various events.
    /// </summary>
    public class CommandHandler : DiscordClientService
    {
        private readonly IServiceProvider provider;
        private readonly CommandService commandService;
        private readonly IConfiguration config;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler"/> class.
        /// </summary>
        /// <param name="client">The <see cref="DiscordSocketClient"/> that should be injected.</param>
        /// <param name="logger">The <see cref="ILogger"/> that should be injected.</param>
        /// <param name="provider">The <see cref="IServiceProvider"/> that should be injected.</param>
        /// <param name="commandService">The <see cref="CommandService"/> that should be injected.</param>
        /// <param name="config">The <see cref="IConfiguration"/> that should be injected.</param>
        public CommandHandler(DiscordSocketClient client, ILogger<CommandHandler> logger, IServiceProvider provider, CommandService commandService, IConfiguration config)
            : base(client, logger)
        {
            this.provider = provider;
            this.commandService = commandService;
            this.config = config;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.Client.MessageReceived += this.HandleMessage;
            this.commandService.CommandExecuted += this.CommandExecutedAsync;
            await this.commandService.AddModulesAsync(Assembly.GetEntryAssembly(), this.provider);
        }

        private async Task HandleMessage(SocketMessage incomingMessage)
        {
            if (incomingMessage is not SocketUserMessage message)
            {
                return;
            }

            if (message.Source != MessageSource.User)
            {
                return;
            }

            var argPos = 0;
            if (!message.HasStringPrefix(this.config["Prefix"], ref argPos) && !message.HasMentionPrefix(this.Client.CurrentUser, ref argPos))
            {
                return;
            }

            var context = new SocketCommandContext(this.Client, message);
            await this.commandService.ExecuteAsync(context, argPos, this.provider);
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            this.Logger.LogInformation("User {user} attempted to use command {command}", context.User, command.Value.Name);

            if (!command.IsSpecified || result.IsSuccess)
            {
                return;
            }

            await context.Channel.SendMessageAsync($"Error: {result}");
        }
    }
}