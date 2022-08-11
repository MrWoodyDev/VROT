using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;
using VROT.Log;
using Microsoft.Extensions.Logging;

namespace VROT
{
    public class Purge : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; }
        private static Logger? _logger;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Purge(ConsoleLogger logger)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _logger = logger;
        }

        [SlashCommand("purge", "Очищает определенное количество сообщений в канале")]
        [Discord.Commands.RequireUserPermission(GuildPermission.ManageMessages)]
        [Discord.Commands.RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task PurgeAsync(int Amount)
        {
            try
            {
                await DeferAsync();
                SocketTextChannel channel = Context.Channel as SocketTextChannel;
                var msgs = await channel.GetMessagesAsync(limit: Amount + 1).FlattenAsync();
                await channel.DeleteMessagesAsync(msgs);
                EmbedBuilder builder = new EmbedBuilder()
                    .WithTitle($"Успешно очищено {Amount} сообщений")
                    .WithFooter(footer =>
                    {
                        footer
                        .WithText("Module: Purge")
                        .WithIconUrl(Context.Guild.IconUrl);
                    })
                    .WithColor(Color.Red);
                await ReplyAsync("", false, builder.Build());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var embed = new EmbedBuilder()
                    .WithTitle("Произошла ошибка")
                    .WithDescription($"Сообщение об ошибке: {ex.Message}")
                    .WithColor(Color.DarkRed);
                await Context.Channel.SendMessageAsync(embed: embed.Build());
            }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        await _logger.Log(new LogMessage(LogSeverity.Info, "PurgeModule : Purge", $"User: {Context.User.Username}, Command: purge", null));
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        }
    }
}
