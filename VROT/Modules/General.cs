using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace VROT.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("Вставай заебал");
        }
    }
}
