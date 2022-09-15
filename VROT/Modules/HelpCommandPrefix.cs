using Discord.Interactions;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using VROT.Common;
using VROT.Models;

namespace VROT.Modules
{
    public class HelpCommandPrefix : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task MenuInput()
        {
            var components = new ComponentBuilder();
            var select = new SelectMenuBuilder()
            {
                CustomId = "menu1",
                Placeholder = "Выберите категорию..."
            };

            select.AddOption("Модерация", "1");
            select.AddOption("Взаимодействия", "2");
            select.AddOption("Развлекательные", "3");

            components.WithSelectMenu(select);

            var embed = new VrotEmbedBuilder()
                .WithTitle($"Команды бота")
                .WithDescription("Список команд бота")
                .WithCurrentTimestamp()
                .Build();
            await ReplyAsync("", embed: embed, components: components.Build());
        }

        [ComponentInteraction("menu1")]
        public async Task MenuHandler(string[] selections)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "helpConfig.json");
            var json = await File.ReadAllTextAsync(path);
            var activity = JsonConvert.DeserializeObject<Event>(json);

            if (selections.First() == "1")
            {
                var embed = new VrotEmbedBuilder()
                    .WithTitle($"Раздел с модерацией")
                    .AddField("Модерация", $"{activity?.Mod}", true)
                    .WithCurrentTimestamp()
                    .Build();
                await ReplyAsync("", embed: embed);
            }

            if (selections.First() == "2")
            {
                var embed = new VrotEmbedBuilder()
                    .WithTitle($"Раздел с плющками")
                    .AddField("Взаимодействия", $"{activity?.Interaction}")
                    .WithCurrentTimestamp()
                    .Build();
                await ReplyAsync("", embed: embed);
            }

            if (selections.First() == "3")
            {
                var embed = new VrotEmbedBuilder()
                    .WithTitle("Раздел равлекательных команд")
                    .AddField("Развлечения", $"{activity?.Fun}")
                    .WithCurrentTimestamp()
                    .Build();
                await ReplyAsync("", embed: embed);
            }
        }

    }
}
