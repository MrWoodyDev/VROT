using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NekosSharp;
using VROT.Models;
using VROT.Common;

namespace VROT.Modules
{
    public class FunCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public FunCommands(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Command("kill")]
        public async Task Kill(SocketGuildUser user = null)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetStringAsync("https://miss.perssbest.repl.co/api/v2/kill");
            var kill = Event.FromJson(response);

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
                    .WithDescription($"**{Context.Message.Author.Username}** убил **{user.Username}**")
                    .WithImageUrl(kill.Image)
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
                    .WithDescription($"**{Context.Message.Author.Username}** ударил **{user.Username}**")
                    .WithImageUrl(request.ImageUrl)
                    .WithCurrentTimestamp()
                    .Build();
                await ReplyAsync(embed: embed);
            }
        }

        [Command("kiss")]
        public async Task Kiss(SocketGuildUser user = null)
        {
            Request request = await NekoClient.Action_v3.KissGif();

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
                    .WithDescription($"**{Context.Message.Author.Username}** поцеловал **{user.Username}**")
                    .WithImageUrl(request.ImageUrl)
                    .WithCurrentTimestamp()
                    .Build();
                await ReplyAsync(embed: embed);
            }
        }

        [Command("feed")]
        public async Task Feed(SocketGuildUser user = null)
        {
            Request request = await NekoClient.Action_v3.FeedGif();

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
                    .WithDescription($"**{Context.Message.Author.Username}** покормил **{user.Username}**")
                    .WithImageUrl(request.ImageUrl)
                    .WithCurrentTimestamp()
                    .Build();
                await ReplyAsync(embed: embed);
            }
        }

        [Command("hug")]
        public async Task Hug(SocketGuildUser user = null)
        {
            Request request = await NekoClient.Action_v3.HugGif();

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
                    .WithDescription($"**{Context.Message.Author.Username}** обнял **{user.Username}**")
                    .WithImageUrl(request.ImageUrl)
                    .WithCurrentTimestamp()
                    .Build();
                await ReplyAsync(embed: embed);
            }
        }

        [Command("poke")]
        public async Task Poke(SocketGuildUser user = null)
        {
            Request request = await NekoClient.Action_v3.PokeGif();

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
                    .WithDescription($"**{Context.Message.Author.Username}** тыкнул в **{user.Username}**")
                    .WithImageUrl(request.ImageUrl)
                    .WithCurrentTimestamp()
                    .Build();
                await ReplyAsync(embed: embed);
            }
        }

        [Command("cuddle")]
        public async Task Cuddle(SocketGuildUser user = null)
        {
            Request request = await NekoClient.Action_v3.CuddleGif();

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
                    .WithDescription($"**{Context.Message.Author.Username}** прижимается к **{user.Username}**")
                    .WithImageUrl(request.ImageUrl)
                    .WithCurrentTimestamp()
                    .Build();
                await ReplyAsync(embed: embed);
            }
        }

        [Command("pat")]
        public async Task Pat(SocketGuildUser user = null)
        {
            Request request = await NekoClient.Action_v3.PatGif();

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
                    .WithDescription($"**{Context.Message.Author.Username}** погладил **{user.Username}**")
                    .WithImageUrl(request.ImageUrl)
                    .WithCurrentTimestamp()
                    .Build();
                await ReplyAsync(embed: embed);
            }
        }

        [Command("tickle")]
        public async Task Tickle(SocketGuildUser user = null)
        {
            Request request = await NekoClient.Action_v3.TickleGif();

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
                    .WithDescription($"**{Context.Message.Author.Username}** щекочет **{user.Username}**")
                    .WithImageUrl(request.ImageUrl)
                    .WithCurrentTimestamp()
                    .Build();
                await ReplyAsync(embed: embed);
            }
        }
    }
}
