using Discord;
using Discord.Commands;
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
            await Context.Message.ReplyAsync($"Вставай");
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


        [Command("ball")]
        [Alias("шар")]
        public async Task Ball([Remainder] string? ask = null)
        {
            if (ask == null)
            {
                await Context.Message.ReplyAsync("**Напишите вопрос**");
            }
            else
            {
                int randNum = 0;
                Random random = new Random();
                randNum = random.Next(1, 7);

                switch (randNum)
                {
                    case 1:
                        await Context.Message.ReplyAsync("Да");
                        break;
                    case 2:
                        await Context.Message.ReplyAsync("Нет");
                        break;
                    case 3:
                        await Context.Message.ReplyAsync("Возможно да");
                        break;
                    case 4:
                        await Context.Message.ReplyAsync("Возможно нет");
                        break;
                    case 5:
                        await Context.Message.ReplyAsync("Иди нахуй");
                        break;
                    case 6:
                        await Context.Message.ReplyAsync("За небольшую оплату, можешь пойти нахуй");
                        break;
                }
            }
        }
    }
}

