using Discord.Commands;
using VROT.Models;

namespace VROT.Modules
{
    public class NsfwCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public NsfwCommands(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Command("boobs")]
        public async Task Boobs()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetStringAsync("https://miss.perssbest.repl.co/api/v2/real_boobs");
            var boobs = Event.FromJson(response);

            await ReplyAsync(boobs.Image);
        }

        [Command("anime-boobs")]
        public async Task AnimeBoobs()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetStringAsync("https://miss.perssbest.repl.co/api/v2/boobs");
            var boobs = Event.FromJson(response);

            await ReplyAsync(boobs.Image);
        }
    }
}
