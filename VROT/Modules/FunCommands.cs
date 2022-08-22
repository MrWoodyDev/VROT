using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NekosSharp;
using VROT.Models;
using VROT.Common;
using VROT.Services;

namespace VROT.Modules
{
    public class FunCommands : ModuleBase<SocketCommandContext>
    {
        private readonly ITenorService _tenorService;

        public FunCommands(ITenorService tenorService)
        {
            _tenorService = tenorService;
        }

        [Command("kill")]
        public async Task Kill(SocketGuildUser user = null)
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
                var tenorGif = await _tenorService.GetRandomGifUrlAsync("kill");

                var embed = new VrotEmbedBuilder()
                    .WithDescription($"**{Context.Message.Author.Username}** убил **{user.Username}**")
                    .WithImageUrl(tenorGif)
                    .WithCurrentTimestamp()
                    .Build();
                await ReplyAsync(embed: embed);
            }
        }

        public static NekosSharp.NekoClient NekoClient = new NekoClient("VROT");

        [Command("slap")]
        public async Task Slap(SocketGuildUser user = null)
        {
            Request request = await NekoClient.Action_v3.SlapGif();

            await RunCommandAsync(user, $"**{Context.Message.Author.Username}** ударил **{user.Username}**", request);
        }

        [Command("kiss")]
        public async Task Kiss(SocketGuildUser user = null)
        {
            Request request = await NekoClient.Action_v3.KissGif();

            await RunCommandAsync(user, $"**{Context.Message.Author.Username}** поцеловал **{user.Username}**", request);
        }

        [Command("feed")]
        public async Task Feed(SocketGuildUser user = null)
        {
            Request request = await NekoClient.Action_v3.FeedGif();

            await RunCommandAsync(user, $"**{Context.Message.Author.Username}** покормил **{user.Username}**", request);
        }

        [Command("hug")]
        public async Task Hug(SocketGuildUser user = null)
        {
            Request request = await NekoClient.Action_v3.HugGif();

            await RunCommandAsync(user, $"**{Context.Message.Author.Username}** обнял **{user.Username}**", request);
        }

        [Command("poke")]
        public async Task Poke(SocketGuildUser user = null)
        {
            Request request = await NekoClient.Action_v3.PokeGif();

            await RunCommandAsync(user, $"**{Context.Message.Author.Username}** тыкнул в **{user.Username}**", request);
        }

        [Command("cuddle")]
        public async Task Cuddle(SocketGuildUser user = null)
        {
            Request request = await NekoClient.Action_v3.CuddleGif();

            await RunCommandAsync(user, $"**{Context.Message.Author.Username}** прижимается к **{user.Username}**", request);
        }

        [Command("pat")]
        public async Task Pat(SocketGuildUser user = null)
        {
            Request request = await NekoClient.Action_v3.PatGif();

            await RunCommandAsync(user, $"**{Context.Message.Author.Username}** погладил **{user.Username}**", request);
        }

        [Command("tickle")]
        public async Task Tickle(SocketGuildUser user = null)
        {
            Request request = await NekoClient.Action_v3.TickleGif();

            await RunCommandAsync(user, $"**{Context.Message.Author.Username}** щекочет **{user.Username}**", request);
        }

        private async Task RunCommandAsync(SocketGuildUser user, string description, Request request)
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
                var embed = new VrotEmbedBuilder()
                    .WithDescription(description)
                    .WithImageUrl(request.ImageUrl)
                    .WithCurrentTimestamp()
                    .Build();
                await ReplyAsync(embed: embed);
            }
        }
    }
}
