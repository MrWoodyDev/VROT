using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;

namespace VROT.Common
{
    internal class CommandHelpersPrefix
    {
        public static bool IsCommandValidPrefix(SocketCommandContext context, SocketGuildUser user, out string validationMessage)
        {
            validationMessage = string.Empty;

            if (user == null)
            {
                validationMessage = $":x: **{context.User.Username}**, пользователь не указан";
                return false;
            }

            if (context.User == user)
            {
                validationMessage = $":x: **{context.User.Username}**, вы не можете себе выдать наказание";
                return false;
            }

            if (((context.User as SocketGuildUser)!).Hierarchy <= user.Hierarchy)
            {
                validationMessage = $":x: **{context.User.Username}**, ваша роль ниже чем у пользователя **{user.Username}**";
                return false;
            }

            if (context.Guild.GetUser(context.Client.CurrentUser.Id).Hierarchy <= user.Hierarchy)
            {
                validationMessage = $":x: **{context.User.Username}**, роль бота ниже чем у пользователя **{user.Username}**";
                return false;
            }

            return true;
        }

        public static Embed GetEmbedPrefix(SocketCommandContext context, SocketGuildUser user, string title, [Remainder] string reason = "Причина не указана")
        {
            var embed = new VrotEmbedBuilder()
                .WithTitle(title)
                .AddField("**Модератор**", $"**{context.User.Username}**#{context.User.Discriminator}", true)
                .AddField("**Причина**", $"{reason}", true)
                .AddField("**Участник**", $"**{user.Username}**#{user.Discriminator} (ID {user.Id})")
                .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .WithCurrentTimestamp()
                .Build();

            return embed;
        }
    }
}
