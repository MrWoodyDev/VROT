using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using VROT.Common;

namespace VROT.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Alias("p")]
        public async Task PingAsync()
        {
            await Context.Message.ReplyAsync($"Вставай заебал");
        }

        [Command("say")]
        public async Task EchoAsync([Remainder] string text)
        {
            await Context.Channel.SendMessageAsync(text);
        }

        [Command("info")]
        public async Task InfoAsync(SocketGuildUser socketGuildUser = null)
        {
            if (socketGuildUser == null)
            {
                socketGuildUser = Context.User as SocketGuildUser;
            }

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
}
