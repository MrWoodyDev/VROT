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
    public async Task Mute(SocketGuildUser user = null, string timeMute = "1m", [Remainder] string reason = null)
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
                var minutes = timeOutDate.AddMinutes(time).ToString("f");

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
                var hours = timeOutDate.AddHours(time).ToString("f");

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
                var days = timeOutDate.AddDays(time).ToString("f");

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

    private async Task RunAdminCommands(SocketGuildUser user = null, string title = null, [Remainder] string reason = null)
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
            await ReplyAsync($":x: **{Context.Message.Author.Username}**, ваша роль ниже чем у пользователя **{user.Username}**");
            return;
        }

        if (Context.Guild.GetUser(Context.Client.CurrentUser.Id).Hierarchy <= user.Hierarchy)
        {
            await ReplyAsync($":x: **{Context.Message.Author.Username}**, роль бота ниже чем у пользователя **{user.Username}**");
            return;
        }

        if (reason == null)
        {
            reason = "Причина не указана";
        }

        var embed = new VrotEmbedBuilder()
            .WithTitle(title)
            .AddField("**Модератор**", $"**{Context.Message.Author.Username}**#{Context.Message.Author.Discriminator}", true)
            .AddField("**Причина**", $"{reason}", true)
            .AddField("**Участник**", $"**{user.Username}**#{user.Discriminator} (ID {user.Id})")
            .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
            .WithCurrentTimestamp()
            .Build();

        await ReplyAsync(embed: embed);
    }
}