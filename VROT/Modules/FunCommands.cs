using Discord.Commands;
using NekosSharp;
using VROT.Models;

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
        public async Task Kill()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetStringAsync("https://miss.perssbest.repl.co/api/v2/kill");
            var kill = Event.FromJson(response);

            await ReplyAsync(kill.Image);
        }

        public static NekosSharp.NekoClient NekoClient = new NekoClient("VROT");

        [Command("slap")]
        public async Task Slap()
        {
            Request requesteq = await NekoClient.Action_v3.SlapGif();
            await ReplyAsync(requesteq.ImageUrl);

        }

        [Command("kiss")]
        public async Task Kiss()
        {
            Request request = await NekoClient.Action_v3.KissGif();
            await ReplyAsync(request.ImageUrl);
        }

        [Command("feed")]
        public async Task Feed()
        {
            Request request = await NekoClient.Action_v3.FeedGif();
            await ReplyAsync(request.ImageUrl);
        }

        [Command("neko")]
        public async Task Neko()
        {
            Request request = await NekoClient.Image_v3.NekoGif();
            await ReplyAsync(request.ImageUrl);
        }

        [Command("fox")]
        public async Task Fox()
        {
            Request request = await NekoClient.Image_v3.Fox();
            await ReplyAsync(request.ImageUrl);
        }

        [Command("hug")]
        public async Task Hug()
        {
            Request request = await NekoClient.Action_v3.HugGif();
            await ReplyAsync(request.ImageUrl);
        }

        [Command("poke")]
        public async Task Poke()
        {
            Request request = await NekoClient.Action_v3.PokeGif();
            await ReplyAsync(request.ImageUrl);
        }
    }
}
