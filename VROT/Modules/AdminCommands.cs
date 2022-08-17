using Discord;
using Discord.Commands;
using Discord.WebSocket;
using VROT.Common;

namespace VROT.Modules
{
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "У вас нет прав администратора")]
        public async Task EchoAsync([Remainder] string text)
        {
            await Context.Channel.SendMessageAsync(text);
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
        }

        [Command("clear")]
        [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "У вас нет прав администратора")]
        public async Task Clear(int amount)
        {
            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
            await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
            const int delay = 3000;
            IUserMessage m = await ReplyAsync($"I have deleted {amount} messages for ya. :)");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "У вас нет прав банить участников")]
        public async Task BanMember(SocketGuildUser user = null, [Remainder] string reason = null)
        {
            if (Context.User is not SocketGuildUser guildUser || guildUser.Hierarchy <= user.Hierarchy)
            {
                await ReplyAsync("Вы не можете забанить этого пользователя");
                return;
            }

            if (user == null)
            {
                await ReplyAsync("Пользователь не указан");
                return;
            }

            if (reason == null)
            {
                reason = "Причина не указана";
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

        [Command("kick")]
        [RequireUserPermission(GuildPermission.KickMembers, ErrorMessage = "У вас нет прав выгонять учатсников")]
        public async Task KickMember(SocketGuildUser user = null, [Remainder] string reason = null)
        {
            if (Context.User is not SocketGuildUser guildUser || guildUser.Hierarchy <= user.Hierarchy)
            {
                await ReplyAsync("Вы не можете кикнуть этого пользователя");
                return;
            }

            if (user == null)
            {
                await ReplyAsync("Пользователь не указан");
                return;
            }

            if (reason == null)
            {
                reason = "Причина не указана";
            }

            var embed = new VrotEmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer => footer.Text = Context.User.Username)
                .WithDescription($":white_check_mark: {user.Mention}был кикнут\n**Причина :** {reason}")
                .WithCurrentTimestamp();

            await ReplyAsync(embed: embed.Build());
            await user.KickAsync(reason);
        }

        [Command("mute")]
        [RequireBotPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.ManageMessages, ErrorMessage = "Вы не имеете прав мутить участников")]
        public async Task Mute(SocketGuildUser user = null, int time = 0, string units = null, [Remainder] string reason = null)
        {
            var timeOutDate = DateTime.Now;
            var minutes = timeOutDate.AddMinutes(time).ToString("f");
            var hours = timeOutDate.AddHours(time).ToString("f");
            var days = timeOutDate.AddDays(time).ToString("f");

            if (Context.User is not SocketGuildUser guildUser || guildUser.Hierarchy <= user.Hierarchy)
            {
                await ReplyAsync("Вы не можете замутить этого пользователя");
                return;
            }

            if (user == null)
            {
                await ReplyAsync("Пользователь не указан");
                return;
            }

            if (reason == null)
            {
                reason = "Причина не указана";
            }

            switch (units)
            {
                case "min":
                    {
                        var embed = new VrotEmbedBuilder()
                            .WithTitle("Участник получил наказание")
                            .AddField("**Модератор**", $"**{Context.Message.Author.Username}**#{Context.Message.Author.Discriminator}", true)
                            .AddField("**Причина**", $"{reason}", true)
                            .AddField("**Наказание будет дейстовать**", $"{time} минут")
                            .AddField("Ограничения будут сняты", minutes)
                            .AddField("**Участник**", $"**{user.Username}**#{user.Discriminator} (ID {user.Id})")
                            .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                            .WithCurrentTimestamp()
                            .Build();

                        await ReplyAsync(embed: embed);
                        await user.SetTimeOutAsync(TimeSpan.FromMinutes(time));
                        break;
                    }
                case "h":
                    {
                        var embed = new VrotEmbedBuilder()
                            .WithTitle("Участник получил наказание")
                            .AddField("**Модератор**", $"**{Context.Message.Author.Username}**#{Context.Message.Author.Discriminator}", true)
                            .AddField("**Причина**", $"{reason}", true)
                            .AddField("**Наказание будет дейстовать**", $"{time} час")
                            .AddField("Ограничения будут сняты", hours)
                            .AddField("**Участник**", $"**{user.Username}**#{user.Discriminator} (ID {user.Id})")
                            .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                            .WithCurrentTimestamp()
                            .Build();

                        await ReplyAsync(embed: embed);
                        await user.SetTimeOutAsync(TimeSpan.FromHours(time));
                        break;
                    }
                case "d":
                    {
                        var embed = new VrotEmbedBuilder()
                            .WithTitle("Участник получил наказание")
                            .AddField("**Модератор**", $"**{Context.Message.Author.Username}**#{Context.Message.Author.Discriminator}", true)
                            .AddField("**Причина**", $"{reason}", true)
                            .AddField("**Наказание будет дейстовать**", $"{time} дней")
                            .AddField("Ограничения будут сняты", days)
                            .AddField("**Участник**", $"**{user.Username}**#{user.Discriminator} (ID {user.Id})")
                            .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                            .WithCurrentTimestamp()
                            .Build();

                        await ReplyAsync(embed: embed);
                        await user.SetTimeOutAsync(TimeSpan.FromDays(time));
                        break;
                    }
            }
        }
    }
}
