namespace VROT.Modules
{
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using VROT.Common;

    /// <summary>
    /// The general module containing commands like ping.
    /// </summary>
    public class General : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// A command respond with ping users.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Command("ping")]
        [Alias("p")]
        public async Task PingAsync()
        {
            await this.Context.Message.ReplyAsync($"Пнул");
            await this.Context.Channel.DeleteMessageAsync(this.Context.Message.Id);
        }

        /// <summary>
        /// A command return message user.
        /// </summary>
        /// <param name="text">An options user messages.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Command("say")]
        public async Task EchoAsync([Remainder] string text)
        {
            await this.Context.Channel.SendMessageAsync(text);
            await this.Context.Channel.DeleteMessageAsync(this.Context.Message.Id);
        }

        /// <summary>
        /// A command get the information about user.
        /// </summary>
        /// <param name="socketGuildUser">An options user get the information from.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Command("info")]
        public async Task InfoAsync(SocketGuildUser? socketGuildUser = null)
        {
            socketGuildUser ??= this.Context.User as SocketGuildUser;

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
                await ReplyAsync(embed: embed);
               await Context.Channel.DeleteMessageAsync(Context.Message.Id);


            }
        }

        /// <summary>
        /// A command for clear history chat.
        /// </summary>
        /// <param name="amount">An option count delete message.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Command("clear")]
        public async Task Clear(int amount)
        {
            IEnumerable<IMessage> messages = await this.Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
            await ((ITextChannel)this.Context.Channel).DeleteMessagesAsync(messages);
            const int delay = 3000;
            IUserMessage m = await this.ReplyAsync($"I have deleted {amount} messages for ya. :)");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }
    }
}