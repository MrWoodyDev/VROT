using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using Microsoft.AspNetCore.DataProtection;
using VROT.Common;

namespace VROT.SlashCommands
{
    public class HelpCmd : InteractionModuleBase<SocketInteractionContext>
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

            select.AddOption("test1", "1");
            select.AddOption("test2", "2");

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
            if (selections.First() == "1")
            {            
                var embed = new VrotEmbedBuilder()
                    .WithTitle($"Раздел с модерацией")
                    .WithDescription("Список команд бота")
                    .WithCurrentTimestamp()
                    .Build();
                await RespondAsync("", embed: embed, ephemeral: true);
            }
            if (selections.First() == "2")
            {
                var embed = new VrotEmbedBuilder()
                    .WithTitle($"Раздел с плющками")
                    .WithDescription("Список команд бота")
                    .WithCurrentTimestamp()
                    .Build();
                await RespondAsync("", embed: embed, ephemeral: true);
            }
        }    

    }
}
