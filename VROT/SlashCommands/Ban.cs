using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;
using VROT.Common;

namespace VROT.SlashCommands
{
    public class Ban : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ban", "Выдать бан пользователю")]
        [Discord.Commands.RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "У вас нет прав банить участников")]
        public async Task BanMember(SocketGuildUser user = null, [Remainder] string? reason = null)
        {
            if (Context.User is not SocketGuildUser guildUser || guildUser.Hierarchy <= user.Hierarchy)
            {
                EmbedBuilder builder = new EmbedBuilder()
                    .WithTitle($":x: Ошибка!")
                    .WithDescription("Вы не можете выдать бан\nэтому пользователю")
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
                    .WithText("Module: Ban")
                    .WithIconUrl(Context.Guild.IconUrl);
                })
                .WithDescription($"Бан\n/ban `[пользователь]` `[причина]`")
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
                    .WithText("Module: Ban")
                    .WithIconUrl(Context.Guild.IconUrl);
                })
                .WithDescription($"Бан\n/ban `[пользователь]` `[причина]`")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: builder.Build());

                return;
            }

            var embed = new VrotEmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer => footer.Text = Context.User.Username)
                .WithDescription($":white_check_mark: {user.Mention}получил банан\n**Причина :** {reason}")
                .WithCurrentTimestamp()
                .Build();

            await ReplyAsync(embed: embed);
            await Context.Guild.AddBanAsync(user, 0, reason);
        }
    }
}
