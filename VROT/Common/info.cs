using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;
using VROT.Log;
using VROT.Common;

namespace VROT
{
    public class Info : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; }
        private static Logger? _logger;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Info(ConsoleLogger logger)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _logger = logger;
        }

        [SlashCommand("info", "information about bot")]
        public async Task BotAsync(SocketGuildUser? socketGuildUser = null)
        {
            socketGuildUser ??= Context.User as SocketGuildUser;

            if (socketGuildUser != null)
            {
                var embed = new VrotEmbedBuilder()
                    .WithTitle($"{socketGuildUser.Username}#{socketGuildUser.Discriminator}")
                    .AddField("ID", socketGuildUser.Id, true)
                    .AddField("Name", $"{socketGuildUser.Username}#{socketGuildUser.Discriminator}", true)
                    .AddField("Created at", socketGuildUser.CreatedAt, true)
                    .AddField("Join at", socketGuildUser.JoinedAt, true)
                    .WithThumbnailUrl(socketGuildUser.GetAvatarUrl() ?? socketGuildUser.GetDefaultAvatarUrl())
                    .WithCurrentTimestamp()
                    .Build();

                await ReplyAsync(embed: embed);
            }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        await _logger.Log(new LogMessage(LogSeverity.Info, "BallCmd : Ball", $"User: {Context.User.Username}, Command: ball", null));
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        }
    }
}
