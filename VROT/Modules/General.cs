using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using VROT.Common;
using VROT.Models;

namespace VROT.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public General(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Command("ping")]
        [Alias("p")]
        public async Task PingAsync()
        {
            await Context.Message.ReplyAsync($"Вставай заебал");
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
        }

        [Command("say")]
        [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "У вас нет прав админа, идите нахуй)0))")]
        public async Task EchoAsync([Remainder] string text)
        {
            await Context.Channel.SendMessageAsync(text);
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
        }

        [Command("info")]
        public async Task InfoAsync(SocketGuildUser? socketGuildUser = null)
        {
            socketGuildUser ??= Context.User as SocketGuildUser;

            if (socketGuildUser != null)
            {
                var embed = new VrotEmbedBuilder()
                    .WithTitle($"{socketGuildUser.Username}#{socketGuildUser.Discriminator}")
                    .AddField("ID", socketGuildUser.Id, true)
                    .AddField("Name", $"{socketGuildUser.Username}#{socketGuildUser.Discriminator}", true)
                    .AddField("Created at", socketGuildUser.CreatedAt, true)
                    .AddField("Join at", socketGuildUser.JoinedAt, true)
                    .WithThumbnailUrl(socketGuildUser.GetAvatarUrl() ?? socketGuildUser.GetDefaultAvatarUrl())
                    .WithCurrentTimestamp()
                    .Build();

                await Context.Channel.DeleteMessageAsync(Context.Message.Id);
                await ReplyAsync(embed: embed);
            }
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
                reason = "Причина не указана";

            var embed = new VrotEmbedBuilder()
                .WithDescription($":white_check_mark: {user.Mention}получил банан\n**Причина :** {reason}")
                .Build();

            await ReplyAsync(embed: embed);
            await Context.Guild.AddBanAsync(user, 0, reason);
        }

        [Command("activity")]
        public async Task Activity()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetStringAsync("http://www.boredapi.com/api/activity/");
            var activity = Event.FromJson(response);

            if (activity == null)
            {
                await ReplyAsync("An error occurred, please try again later.");
                return;
            }

            await ReplyAsync($"**Activity:** {activity.Activity}\n**Participants:** {activity.Participants}\n**Type:** {activity.Type}\n**Price:** {activity.Price}\n**Accessibility:** {activity.Accessibility}");
        }
    }
}

