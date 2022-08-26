using Discord.Commands;
using Discord.WebSocket;
using VROT.Common;

namespace VROT.Modules
{
    public class FunCommands : ModuleBase<SocketCommandContext>
    {
        [Command("kill")]
        public async Task Kill(SocketGuildUser user = null!, EmbedCommand embedCommand = null!)
        {
            await embedCommand.RunGifCommandAsync(user, $"**{Context.Message.Author.Username}** убил **{user.Username}**", "anime kill");
        }

        [Command("slap")]
        public async Task Slap(SocketGuildUser user = null!, EmbedCommand embedCommand = null!)
        {
            await embedCommand.RunGifCommandAsync(user, $"**{Context.Message.Author.Username}** дал пощёчину **{user.Username}**", "anime slap");
        }

        [Command("punch")]
        public async Task Punch(SocketGuildUser user = null!, EmbedCommand embedCommand = null!)
        {
            await embedCommand.RunGifCommandAsync(user, $"**{Context.Message.Author.Username}** дал пизды **{user.Username}**", "anime punch");
        }

        [Command("kiss")]
        public async Task Kiss(SocketGuildUser user = null!, EmbedCommand embedCommand = null!)
        {
            await embedCommand.RunGifCommandAsync(user, $"**{Context.Message.Author.Username}** поцеловал **{user.Username}**", "anime kiss");
        }

        [Command("feed")]
        public async Task Feed(SocketGuildUser user = null!, EmbedCommand embedCommand = null!)
        {
            await embedCommand.RunGifCommandAsync(user, $"**{Context.Message.Author.Username}** покормил **{user.Username}**", "anime feed");
        }

        [Command("hug")]
        public async Task Hug(SocketGuildUser user = null!, EmbedCommand embedCommand = null!)
        {
            await embedCommand.RunGifCommandAsync(user, $"**{Context.Message.Author.Username}** обнял **{user.Username}**", "anime hug");
        }

        [Command("poke")]
        public async Task Poke(SocketGuildUser user = null!, EmbedCommand embedCommand = null!)
        {
            await embedCommand.RunGifCommandAsync(user, $"**{Context.Message.Author.Username}** тыкнул в **{user.Username}**", "anime poke");
        }

        [Command("cuddle")]
        public async Task Cuddle(SocketGuildUser user = null!, EmbedCommand embedCommand = null!)
        {
            await embedCommand.RunGifCommandAsync(user, $"**{Context.Message.Author.Username}** прижимается к **{user.Username}**", "anime cuddle");
        }

        [Command("pat")]
        public async Task Pat(SocketGuildUser user = null!, EmbedCommand embedCommand = null!)
        {
            await embedCommand.RunGifCommandAsync(user, $"**{Context.Message.Author.Username}** погладил **{user.Username}**", "anime pat");
        }

        [Command("tickle")]
        public async Task Tickle(SocketGuildUser user = null!, EmbedCommand embedCommand = null!)
        {
            await embedCommand.RunGifCommandAsync(user, $"**{Context.Message.Author.Username}** щекочет **{user.Username}**", "anime tickle");
        }
    }
}