using Discord;
using Discord.Interactions;
using VROT.Common;
using Newtonsoft.Json;
using VROT.Models;

namespace VROT.SlashCommands
{
    public class HelpCommandSlash : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("help", "команда help")]
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
                    .AddField("Модерация",$"{activity?.Mod}" , true)
                    .WithCurrentTimestamp()
                    .Build();
                await RespondAsync("", embed: embed, ephemeral: true);
            }
            if (selections.First() == "2")
            {
                var embed = new VrotEmbedBuilder()
                    .WithTitle($"Раздел с плющками")
                    .AddField("Взаимодействия", $"{activity?.Interaction}")
                    .WithCurrentTimestamp()
                    .Build();
                await RespondAsync("", embed: embed, ephemeral: true);
            }
        }    

    }
}
