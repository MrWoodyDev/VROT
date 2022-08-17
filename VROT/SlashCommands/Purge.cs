using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using VROT.Common;

namespace VROT.SlashCommands
{
    public class Purge : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("purge", "Очищает определенное количество сообщений в канале")]
        [Discord.Commands.RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "У вас нет прав!")]
        public async Task PurgeAsync(int Amount)
        {
            try
            {
                await DeferAsync();
                SocketTextChannel channel = Context.Channel as SocketTextChannel;
                var msgs = await channel.GetMessagesAsync(limit: Amount + 1).FlattenAsync();
                await channel.DeleteMessagesAsync(msgs);
                var embed = new VrotEmbedBuilder()
                    .WithTitle($"Успешно очищено {Amount} сообщений")
                    .WithCurrentTimestamp()
                    .WithFooter(footer =>
                    {
                        footer
                        .WithText("Module: Purge")
                        .WithIconUrl(Context.Guild.IconUrl);
                    });
                await ReplyAsync(embed: embed.Build());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var embed = new EmbedBuilder()
                    .WithTitle(":x: Произошла ошибка")
                    .WithDescription($"Сообщение об ошибке: {ex.Message}")
                    .WithColor(Color.DarkRed)
                    .WithCurrentTimestamp();
                await Context.Channel.SendMessageAsync(embed: embed.Build());
            }
        }
    }
}