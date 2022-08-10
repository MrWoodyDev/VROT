using Discord.Commands;
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

        [Command("hug")]
        public async Task Hug()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetStringAsync("https://miss.perssbest.repl.co/api/v2/hug");
            var hug = Event.FromJson(response);

            await ReplyAsync(hug.Image);
        }

        [Command("kiss")]
        public async Task Kiss()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetStringAsync("https://miss.perssbest.repl.co/api/v2/kiss");
            var kiss = Event.FromJson(response);

            await ReplyAsync(kiss.Image);
        }

        [Command("kill")]
        public async Task Kill()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetStringAsync("https://miss.perssbest.repl.co/api/v2/kill");
            var kill = Event.FromJson(response);

            await ReplyAsync(kill.Image);
        }
    }
}
