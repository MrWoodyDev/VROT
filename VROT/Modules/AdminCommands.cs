using Discord;
using Discord.Commands;
using Discord.WebSocket;
using VROT.Common;

namespace VROT.Modules;

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
        await Task.Delay(delay);
    }

    [Command("ban")]
    [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "У вас нет прав банить участников")]
    public async Task BanMember(SocketGuildUser user = null, [Remainder] string reason = null)
    {
        if (!CommandHelpersPrefix.IsCommandValidPrefix(Context, user, out string validationMessage))
        {
            await ReplyAsync(validationMessage);
            return;
        }

        await Context.Guild.AddBanAsync(user, 0, reason);

        await ReplyAsync(embed: CommandHelpersPrefix.GetEmbedPrefix(Context, user, "Участник получил бан", reason));
    }

    [Command("random-mute")]
    [RequireOwner(ErrorMessage = "Только для владельца бота")]
    public async Task RandomBan()
    { 
        var users = Context.Guild.Users.ToList();
        var randomUser = users[new Random().Next(users.Count)];

        
        if (Context.Guild.GetUser(Context.Client.CurrentUser.Id).Hierarchy <= randomUser.Hierarchy)
        {
            await ReplyAsync($"Пошёл нахуй его я не забаню {randomUser}");
        }


        await randomUser.SetTimeOutAsync(TimeSpan.FromMinutes(1));
        await ReplyAsync($"Пользователь {randomUser.Mention} опущен");
    }

    [Command("kick")]
    [RequireUserPermission(GuildPermission.KickMembers, ErrorMessage = "У вас нет прав выгонять учатсников")]
    public async Task KickMember(SocketGuildUser user = null, [Remainder] string reason = null)
    {
        if (!CommandHelpersPrefix.IsCommandValidPrefix(Context, user, out string validationMessage))
        {
            await ReplyAsync(validationMessage);
            return;
        }

        await user.KickAsync(reason);

        await ReplyAsync(embed: CommandHelpersPrefix.GetEmbedPrefix(Context, user, "Участник получил бан", reason));
    }

    [Command("mute")]
    [RequireUserPermission(GuildPermission.ManageMessages, ErrorMessage = "Вы не имеете прав мутить участников")]
    public async Task Mute(SocketGuildUser user, string timeMute = "1m",
        [Remainder] string? reason = "Причина не указана")
    {
        var units = timeMute.Last().ToString();
        int.TryParse(timeMute.Substring(0, timeMute.Length - 1), out var time);

        var timeOutDate = DateTime.Now;

        if (!CommandHelpersPrefix.IsCommandValidPrefix(Context, user, out string validationMessage))
        {
            await ReplyAsync(validationMessage);
            return;
        }

        switch (units)
        {
            case "m":
            {
                var minutes = $"<t:{((DateTimeOffset)timeOutDate.AddMinutes(time)).ToUnixTimeSeconds()}:f>";

                var embed = new VrotEmbedBuilder()
                    .WithTitle("Участник получил наказание")
                    .AddField("**Модератор**",
                        $"**{Context.Message.Author.Username}**#{Context.Message.Author.Discriminator}", true)
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
                var hours = $"<t:{((DateTimeOffset)timeOutDate.AddHours(time)).ToUnixTimeSeconds()}:f>";

                var embed = new VrotEmbedBuilder()
                    .WithTitle("Участник получил наказание")
                    .AddField("**Модератор**",
                        $"**{Context.Message.Author.Username}**#{Context.Message.Author.Discriminator}", true)
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
                var days = $"<t:{((DateTimeOffset)timeOutDate.AddDays(time)).ToUnixTimeSeconds()}:f>";

                var embed = new VrotEmbedBuilder()
                    .WithTitle("Участник получил наказание")
                    .AddField("**Модератор**",
                        $"**{Context.Message.Author.Username}**#{Context.Message.Author.Discriminator}", true)
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

    [Command("while")]
    [RequireOwner(ErrorMessage = "Только для владельца бота")]
    public async Task WhileAsync(SocketGuildUser user, [Remainder] string text)
    {
        while (true)
        {
            await user.SendMessageAsync(text);
        }
    }

    private async Task RunAdminCommands(SocketGuildUser user = null, string title = null,
        [Remainder] string reason = null)
    {
        if (user == null)
        {
            await ReplyAsync(":x: Пользователь не указан");
            return;
        }

        if (Context.Message.Author == user)
        {
            await ReplyAsync($":x: **{Context.Message.Author.Username}**, вы не можете себе выдать наказание");
            return;
        }

        if (((Context.User as SocketGuildUser)!).Hierarchy <= user.Hierarchy)
        {
            await ReplyAsync(
                $":x: **{Context.Message.Author.Username}**, ваша роль ниже чем у пользователя **{user.Username}**");
            return;
        }

        if (Context.Guild.GetUser(Context.Client.CurrentUser.Id).Hierarchy <= user.Hierarchy)
        {
            await ReplyAsync(
                $":x: **{Context.Message.Author.Username}**, роль бота ниже чем у пользователя **{user.Username}**");
            return;
        }

        if (reason == null)
        {
            reason = "Причина не указана";
        }

        var embed = new VrotEmbedBuilder()
            .WithTitle(title)
            .AddField("**Модератор**", $"**{Context.Message.Author.Username}**#{Context.Message.Author.Discriminator}",
                true)
            .AddField("**Причина**", $"{reason}", true)
            .AddField("**Участник**", $"**{user.Username}**#{user.Discriminator} (ID {user.Id})")
            .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
            .WithCurrentTimestamp()
            .Build();

        await ReplyAsync(embed: embed);
    }

    [Command("Test")]
    public async Task Test()
    {
        var msg = await Context.Channel.SendMessageAsync(":tada:");

        // Создаем объект Emoji с кодом символа эмодзи
        var rEmote = new Emoji("\uD83C\uDF89");

        await msg.AddReactionAsync(rEmote);
    }

    [Command("giveaway")]
    [RequireOwner(ErrorMessage = "Только для владельца бота")]
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
        await msg.AddReactionAsync(new Emoji(":tada:"));

        var endTime = DateTime.UtcNow.AddMinutes(minutes);
        var users = Context.Guild.Users.ToList();

        while (DateTime.UtcNow < endTime)
        {
            var reactions = await msg.GetReactionUsersAsync(new Emoji(":tada:"), int.MaxValue).FlattenAsync();
            foreach (SocketGuildUser user in reactions)
            {
                if (!users.Contains(user))
                {
                    users.Add(user);
                }
            }

            var participants = reactions.Where(user => !user.IsBot).Distinct();
            embed.WithFooter($"Заканчивается в: {endTime} | Участники: {participants.Count()}");
            await msg.ModifyAsync(x => x.Embed = embed.Build());

            if (DateTime.UtcNow >= endTime)
            {
                break;
            }

            await Task.Delay(1000);
        }

        var winner = users[new Random().Next(users.Count)];
        embed.WithTitle("Giveaway окончен");
        embed.WithDescription($":tada: Поздравляем {winner.Mention} за победу в розыгрыше!");
        embed.WithFooter("Закончилось в: " + endTime);
        await msg.ModifyAsync(x => x.Embed = embed.Build());

        var embed1 = new VrotEmbedBuilder();
        embed1.WithDescription($":tada: Поздравялем {winner.Mention}, Вы выиграли **'{message}'**");

        await Context.Channel.SendMessageAsync(embed: embed1.Build());
        await winner.SetTimeOutAsync(TimeSpan.FromMinutes(1));
    }

}