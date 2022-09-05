using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using VROT.Common;

namespace VROT.SlashCommands
{
    public class HelpCmd : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("help", "help")]
        public async Task HelpAsync()
        {

            var menuBuilder = new SelectMenuBuilder()
                .WithPlaceholder("Выберите категорию")
                .WithCustomId("menu-1")
                .WithMinValues(1)
                .WithMaxValues(1)
                .AddOption("Модерация", "opt-a", "Команды для удобной модерации сервера")
                .AddOption("Плюшки", "opt-b", "18+ контент");

            var builder = new ComponentBuilder()
                .WithSelectMenu(menuBuilder);

            var embed = new VrotEmbedBuilder()
                .WithTitle($"Команды бота")
                .WithDescription("Список команд бота")
                .WithCurrentTimestamp()
                .Build();
            await ReplyAsync("", embed: embed, components: builder.Build());

            Context.Client.SelectMenuExecuted += MyMenuHandler;

        }
        //public async Task MyMenuHandler(SocketMessageComponent arg)
        //{
        //    var text = string.Join(", ", arg.Data.Values);
        //    var embed = new VrotEmbedBuilder()
        //        .WithTitle($"Хуй")
        //        .WithDescription("Залупа")
        //        .WithCurrentTimestamp()
        //        .Build();
        //    await arg.RespondAsync(embed: embed, ephemeral: true);

        //}

        public async Task MyMenuHandler(SocketMessageComponent arg)
        {
            var text = string.Join("", arg.Data.Values);
            await arg.RespondAsync($"{text}", ephemeral: true);
        }
    }
}

