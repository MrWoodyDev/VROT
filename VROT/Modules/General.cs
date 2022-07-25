namespace VROT.Modules
{
    using Discord;
    using Discord.Commands;
    using System.Threading.Tasks;
    using Discord.WebSocket;
    using VROT.Common;

    public class General : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Alias("p")]
        public async Task PingAsync()
        {
            await Context.Message.ReplyAsync($"Вставай заебал");
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
        }

        [Command("say")]
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
                    .WithThumbnailUrl(socketGuildUser.GetAvatarUrl() ?? socketGuildUser.GetDefaultAvatarUrl())
                    .WithCurrentTimestamp()
                    .Build();

                await ReplyAsync(embed: embed);
            }
        }

        [Command("clear")]
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
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task banUser(string banre, [Remainder] SocketGuildUser usertobehammered)
        {
            var rUser = Context.User as SocketGuildUser;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Bot Admin");

            if (rUser != null && rUser.Roles.Contains(role))
            {
                await ReplyAsync($"User {usertobehammered.Mention} has been banned.");
                await Context.Guild.AddBanAsync(usertobehammered, 0, banre);
            }
            else
            {
                await ReplyAsync(":no_entry_sign: You need the Bot Admin role to do that!");
            }
        }
    }
}

