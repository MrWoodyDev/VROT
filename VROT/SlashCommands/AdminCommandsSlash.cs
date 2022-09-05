using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;
using VROT.Common;

namespace VROT.SlashCommands
{
    public class AdminCommandsSlash : InteractionModuleBase<SocketInteractionContext>
    {
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.BanMembers)]
        [SlashCommand("say", "Выдать бан пользователю")]
        [Discord.Commands.RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "У вас нет прав администратора")]
        public async Task EchoAsync([Remainder] string text)
        {
            await Context.Channel.SendMessageAsync(text);
            await Context.Channel.DeleteMessageAsync(Context.User.Id);
        }

        [SlashCommand("clear", "clear")]
        [Discord.Commands.RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "У вас нет прав администратора")]
        public async Task Clear(int amount)
        {
            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
            await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
            const int delay = 3000;
            await Task.Delay(delay);
        }

        [SlashCommand("ban", "ban")]
        [Discord.Commands.RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "У вас нет прав банить участников")]
        public async Task BanMember(SocketGuildUser user = null, [Remainder] string reason = null)
        {
            if (!CommandHelpersSlash.IsCommandValidSlash(Context, user, out string validationMessage))
            {
                await ReplyAsync(validationMessage);
                return;
            }

            await Context.Guild.AddBanAsync(user, 0, reason);

            await ReplyAsync(embed: CommandHelpersSlash.GetEmbedSlash(Context, user, "Участник получил бан", reason));
        }

        [SlashCommand("kick", "kick")]
        [Discord.Commands.RequireUserPermission(GuildPermission.KickMembers, ErrorMessage = "У вас нет прав выгонять учатсников")]
        public async Task KickMember(SocketGuildUser user = null, [Remainder] string reason = null)
        {
            if (!CommandHelpersSlash.IsCommandValidSlash(Context, user, out string validationMessage))
            {
                await ReplyAsync(validationMessage);
                return;
            }

            await user.KickAsync(reason);

            await ReplyAsync(embed: CommandHelpersSlash.GetEmbedSlash(Context, user, "Участник получил наказание", reason));
        }

        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.BanMembers)]
        [SlashCommand("mute", "Выдать мут пользователю")]
        public async Task Mute(SocketGuildUser user = null, string timeMute = "1m", [Remainder] string reason = null)
        {
            var units = timeMute.Last().ToString();
            int.TryParse(timeMute.Substring(0, timeMute.Length - 1), out var time);

            var timeOutDate = DateTime.Now;

            if (!CommandHelpersSlash.IsCommandValidSlash(Context, user, out string validationMessage))
            {
                await ReplyAsync(validationMessage);
                return;
            }

            switch (units)
            {
                case "m":
                    {
                        var minutes = timeOutDate.AddMinutes(time).ToString("f");

                        var embed = new VrotEmbedBuilder()
                            .WithTitle("Участник получил наказание")
                            .AddField("**Модератор**", $"**{Context.User.Username}**#{Context.User.Discriminator}", true)
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
                        var hours = timeOutDate.AddHours(time).ToString("f");

                        var embed = new VrotEmbedBuilder()
                            .WithTitle("Участник получил наказание")
                            .AddField("**Модератор**", $"**{Context.User.Username}**#{Context.User.Discriminator}", true)
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
                        var days = timeOutDate.AddDays(time).ToString("f");

                        var embed = new VrotEmbedBuilder()
                            .WithTitle("Участник получил наказание")
                            .AddField("**Модератор**", $"**{Context.User.Username}**#{Context.User.Discriminator}", true)
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
        
        [SlashCommand("report", "пожаловаться на пользователя")]
        public async Task ButtomCommandHandler(SocketGuildUser? user = null, string reason = null)
        {
            if (user == null)
            {
                EmbedBuilder builder = new EmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Module: Report")
                    .WithIconUrl(Context.Guild.IconUrl);
                })
                .WithDescription($"Репорт\n/report `[пользователь]` `[причина]`")
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
                    .WithText("Module: Report")
                    .WithIconUrl(Context.Guild.IconUrl);
                })
                .WithDescription($"Репорт\n/report `[пользователь]` `[причина]`")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: builder.Build());

                return;
            }

            var component = new ComponentBuilder();
                var button = new ButtonBuilder()
                {
                    Label = "Бан",
                    CustomId = $"button1:{user.Id}",
                    Style = ButtonStyle.Danger
                };
                var channel = Context.Guild.Channels.SingleOrDefault(x => x.Name == $"report - {Context.User}");

            component.WithButton(button);

            var newChannel = await Context.Guild.CreateTextChannelAsync($"report - {Context.User}");
                var newChannelId = newChannel.Id;

            component.WithButton(label: "Закрыть", style: ButtonStyle.Secondary, customId: $"button2:{newChannel.Id}");


            await newChannel.SendMessageAsync($"{Context.User.Mention} | {user.Mention}");
                var embed = new VrotEmbedBuilder()
                .WithAuthor($"Жалоба от {Context.User}")
                .WithFooter(footer => footer.Text = "Module: Report")
                .WithDescription($"**Участник:** {user.Mention}\n(ID: `{Context.User.Id}`)\n**Причина:** {reason}")
                .WithCurrentTimestamp()
                .Build();

                await RespondAsync($"Репорт создан, перейдите в {newChannel.Mention}", ephemeral: true);
                await newChannel.SendMessageAsync(embed: embed, components: component.Build());
                

        }
        [ComponentInteraction("button1:*")]
        [Discord.Interactions.RequireUserPermission(GuildPermission.BanMembers)]
        public async Task HandleButton2(ulong userid)
        {
            var component = new ComponentBuilder();
            await Context.Guild.AddBanAsync(userid);
            await RespondAsync(components: component.Build(), ephemeral: true);
        }
        [ComponentInteraction("button2:*", ignoreGroupNames: true)]
        public async Task HandleButton3(ulong Id)
        {
            var component = new ComponentBuilder();
            var channel = Context.Guild.GetTextChannel(Id);
            await channel.DeleteAsync();
            await RespondAsync(components: component.Build(), ephemeral: true);
        }
    }
    
}
