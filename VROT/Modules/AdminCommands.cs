using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using VROT.Common;
using VROT.Models;

namespace VROT.Modules
{
    public class AdminCommands : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "У вас нет прав админа, идите нахуй)0))")]
        public async Task EchoAsync([Remainder] string text)
        {
            await Context.Channel.SendMessageAsync(text);
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
        }

        [Command("clear")]
        [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "У вас нет прав админа, идите нахуй)0))")]
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
                .WithDescription($":white_check_mark: {user.Mention}получил банан\n**Причина :** {reason}")
                .Build();

            await ReplyAsync(embed: embed);
            await Context.Guild.AddBanAsync(user, 0, reason);
        }

        [Command("kick")]
        [RequireUserPermission(GuildPermission.KickMembers, ErrorMessage = "У вас нет прав выгонять учатсников")]
        public async Task KickMember(SocketGuildUser user = null, [Remainder] string reason = null)
        {

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
    }
}
