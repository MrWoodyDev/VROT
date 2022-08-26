using Discord.Commands;
using Discord.WebSocket;
using VROT.Services;

namespace VROT.Common
{
    public class EmbedCommand : ModuleBase<CommandContext>
    {
        private readonly ITenorService _tenorService;

        public EmbedCommand(ITenorService tenorService)
        {
            _tenorService = tenorService;
        }

        public async Task RunGifCommandAsync(SocketGuildUser user, string description, string gifSearch)
        {
            if (user == null)
            {
                var embed = new VrotEmbedBuilder()
                    .WithDescription("**Укажите пользователя**")
                    .WithCurrentTimestamp()
                    .Build();
                await ReplyAsync(embed: embed);
            }
            else
            {
                var gifUrl = await _tenorService.GetRandomGifUrlAsync(gifSearch);

                var embed = new VrotEmbedBuilder()
                    .WithDescription(description)
                    .WithImageUrl(gifUrl)
                    .Build();
                await ReplyAsync(embed: embed);
            }
        }
    }
}