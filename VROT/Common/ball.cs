using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VROT.Modules
{
    public class PrefixModule : ModuleBase<SocketCommandContext>
    {
        [Command("ball")]
        [Alias("шар")]
        public async Task Ball([Remainder] string ask = null)
        {
            int randNum = 0;
            Random random = new Random();
            randNum = random.Next(1, 7);

            switch (randNum)
            {
                case 1:
                    await Context.Message.ReplyAsync("Да");
                    break;
                case 2:
                    await Context.Message.ReplyAsync("Нет");
                    break;
                case 3:
                    await Context.Message.ReplyAsync("Возможно да");
                    break;
                case 4:
                    await Context.Message.ReplyAsync("Возможно нет");
                    break;
                case 5:
                    await Context.Message.ReplyAsync("Иди нахуй");
                    break;
                case 6:
                    await Context.Message.ReplyAsync("За небольшую оплату, можешь пойти нахуй");
                    break;
            }
        }
    }
}
