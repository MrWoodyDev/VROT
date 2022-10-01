using Discord.Interactions;
using Discord.WebSocket;
using VROT.Common;

namespace VROT.SlashCommands;

public class InteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("info", "Info about members")]
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

            await ReplyAsync(embed: embed);
        }
    }

    [SlashCommand("ping", "command add role")]
    public async Task PingAsync()
    {
        var embed = new VrotEmbedBuilder();

        embed.WithTitle($"Pong! :ping_pong: - {Context.Client.Latency}ms");

        await ReplyAsync("", false, embed.Build());
    }
}