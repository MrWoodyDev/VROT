using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;
using VROT.Common;

namespace VROT.SlashCommands;

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
    public async Task ButtonCommandHandler(SocketGuildUser? user = null, string reason = null)
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

    [SlashCommand("set-role", "Выбрать роль для выдачи при верефикации")]
    public async Task SelectRole(SocketRole role)
    {
        await RespondAsync($" {role.Mention} выбрана!");
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

    [EnabledInDm(false)]
    [DefaultMemberPermissions(GuildPermission.Administrator)]
    [SlashCommand("role", "put buttons in channel")]
    [Discord.Commands.RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "У вас нет прав!")]
    public async Task Buttons([Remainder] SocketRole role)
    {
        var component = new ComponentBuilder();
        var button = new ButtonBuilder()
        {
            Label = "Verify",
            CustomId = "button1",
            Style = ButtonStyle.Secondary
        };

        component.WithButton(button);
        await ReplyAsync("Выбери подходящую для себя роль на сервере, пожалуйста!", components: component.Build());
    }

    [ComponentInteraction("button1")]
    public async Task MyButtonHandler()
    {
        var component = new ComponentBuilder();
        var role = Context.Guild.GetRole(1008508387388051526);
        await ((Context.User as IGuildUser)!).AddRoleAsync(role);
        await RespondAsync($"+ Роль {role} выдана", ephemeral: true);
        await RespondAsync(components: component.Build(), ephemeral: true);

    }

    [SlashCommand("giveaway", "Сделать Giveaway.")]
    public async Task StartGiveaway(string time, string message)
    {
        int minutes = 0;
        string timeFormat = "неизвестно";

        if (time.EndsWith("d"))
        {
            int days = int.Parse(time.Substring(0, time.Length - 1));
            minutes = days * 24 * 60;
            timeFormat = days == 1 ? "день" : days > 1 && days < 5 ? "дня" : "дней";
        }
        else if (time.EndsWith("h"))
        {
            int hours = int.Parse(time.Substring(0, time.Length - 1));
            minutes = hours * 60;
            timeFormat = hours == 1 ? "час" : hours > 1 && hours < 5 ? "часа" : "часов";
        }
        else if (time.EndsWith("m"))
        {
            minutes = int.Parse(time.Substring(0, time.Length - 1));
            timeFormat = minutes == 1 ? "минута" : minutes > 1 && minutes < 5 ? "минуты" : "минут";
        }

        var embed = new VrotEmbedBuilder();
        embed.WithTitle($"{message}");
        embed.AddField("От", Context.User.Mention);
        embed.AddField("Заканчивается", $"<t:{new DateTimeOffset(DateTime.UtcNow.AddMinutes(minutes)).ToUnixTimeSeconds()}:R> (<t:{new DateTimeOffset(DateTime.UtcNow.AddMinutes(minutes)).ToUnixTimeSeconds()}:F>)");
        var msg = await Context.Channel.SendMessageAsync("", false, embed.Build());
        IEmote reactionEmote = new Emoji("\uD83C\uDF89");
        await msg.AddReactionAsync(reactionEmote);

        var endTime = DateTime.UtcNow.AddMinutes(minutes);
        var users = new List<IGuildUser>().Where(user => !user.IsBot).ToList(); // Создаем пустой список для участников

        while (DateTime.UtcNow < endTime)
        {
            var reactions = await msg.GetReactionUsersAsync(reactionEmote, int.MaxValue).FlattenAsync();
            foreach (var reactionUser in reactions)
            {
                var user = await ((IGuild)Context.Guild).GetUserAsync(reactionUser.Id);
                if (user != null && !users.Contains(user))
                {
                    users.Add(user);
                }
            }

            var participants = users.Where(user => !user.IsBot).Distinct(); // Выбираем участников из списка users
            embed.WithFooter($"Заканчивается в: {endTime} | Участники: {participants.Count()}");
            await msg.ModifyAsync(x => x.Embed = embed.Build(), null);

            if (DateTime.UtcNow >= endTime)
            {
                break;
            }

            await Task.Delay(1000);
        }

        var winner = users.Where(user => user.Id != Context.Client.CurrentUser.Id).ToList()[new Random().Next(users.Count - 1)];

        embed.WithTitle("Giveaway окончен");
        embed.WithDescription($":tada: Поздравляем {winner.Mention} за победу в розыгрыше!");
        embed.WithFooter("Закончилось в: " + endTime);
        await msg.ModifyAsync(x => x.Embed = embed.Build(), null);

        //var embed1 = new VrotEmbedBuilder();
        //embed1.WithDescription($":tada: Поздравляем {winner.Mention}, Вы выиграли **'{message}'**");

        await Context.Channel.SendMessageAsync($"{winner.Mention} лови аптечку");
        await winner.SetTimeOutAsync(TimeSpan.FromMinutes(10));
        
    }

}