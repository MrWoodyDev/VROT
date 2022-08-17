using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;
using VROT.Common;

namespace VROT.SlashCommands
{
    public class Kick : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("kick", "Кикнуть пользователя")]
        [Discord.Commands.RequireUserPermission(GuildPermission.KickMembers, ErrorMessage = "У вас нет прав выгонять учатсников")]
        public async Task KickMember(SocketGuildUser user = null, [Remainder] string reason = null)
        {
            if (Context.User is not SocketGuildUser guildUser || guildUser.Hierarchy <= user.Hierarchy)
            {
                EmbedBuilder builder = new EmbedBuilder()
                    .WithTitle($":x: Ошибка!")
                    .WithDescription("Вы не можете кикнуть\nэтого пользователя")
                    .WithCurrentTimestamp()
                    .WithColor(Color.DarkRed)
                    .WithFooter(footer =>
                    {
                        footer
                        .WithText("Module: Ban")
                        .WithIconUrl(Context.Guild.IconUrl);
                    });
                await ReplyAsync(embed: builder.Build());
                return;
            }

            if (user == null)
            {
                EmbedBuilder builder = new EmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Module: Kick")
                    .WithIconUrl(Context.Guild.IconUrl);
                })
                .WithDescription($"Бан\n/kick `[пользователь]` `[причина]`")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: builder.Build());

                return;
            }

            if (reason == null)
            {
                EmbedBuilder builder = new EmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Module: Kick")
                    .WithIconUrl(Context.Guild.IconUrl);
                })
                .WithDescription($"Бан\n/kick `[пользователь]` `[причина]`")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: builder.Build());

                return;
            }

            var embed = new VrotEmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer => footer.Text = Context.User.Username)
                .WithDescription($":white_check_mark: {user.Mention}был кикнут\n**Причина :** {reason}")
                .WithCurrentTimestamp();

            await ReplyAsync(embed: embed.Build());
            await user.KickAsync(reason);
        }
    }
}
