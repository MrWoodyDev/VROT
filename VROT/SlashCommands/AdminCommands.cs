using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;
using VROT.Common;

namespace VROT.SlashCommands
{
    public class AdminCmd : InteractionModuleBase<SocketInteractionContext>
    {
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.BanMembers)]
        [SlashCommand("ban", "Выдать бан пользователю")]
        [Discord.Commands.RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "У вас нет прав банить участников")]
        public async Task BanMember(SocketGuildUser user = null, [Remainder] string? reason = null)
        {
            if (Context.User is not SocketGuildUser guildUser || guildUser.Hierarchy <= user.Hierarchy)
            {
                EmbedBuilder builder = new EmbedBuilder()
                    .WithTitle($":x: Ошибка!")
                    .WithDescription("Вы не можете выдать бан\nэтому пользователю")
                    .WithCurrentTimestamp()
                    .WithColor(Color.DarkRed)
                    .WithFooter(footer =>
                    {
                        footer
                        .WithText("Module: Ban")
                        .WithIconUrl(Context.Guild.IconUrl);
                    });
                await ReplyAsync(embed: builder.Build());
                return;
            }

            if (user == null)
            {
                EmbedBuilder builder = new EmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Module: Ban")
                    .WithIconUrl(Context.Guild.IconUrl);
                })
                .WithDescription($"Бан\n/ban `[пользователь]` `[причина]`")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: builder.Build());

                return;
            }

            if (reason == null)
            {
                EmbedBuilder builder = new EmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Module: Ban")
                    .WithIconUrl(Context.Guild.IconUrl);
                })
                .WithDescription($"Бан\n/ban `[пользователь]` `[причина]`")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: builder.Build());

                return;
            }

            var embed = new VrotEmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer => footer.Text = Context.User.Username)
                .WithDescription($":white_check_mark: {user.Mention}получил банан\n**Причина :** {reason}")
                .WithCurrentTimestamp()
                .Build();

            await ReplyAsync(embed: embed);
            await Context.Guild.AddBanAsync(user, 0, reason);
        } 
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.KickMembers)]
        [SlashCommand("kick", "Кикнуть пользователя")]
        [Discord.Commands.RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "У вас нет прав выгонять учатсников")]
        public async Task KickMember(SocketGuildUser user = null, [Remainder] string reason = null)
        {
            if (Context.User is not SocketGuildUser guildUser || guildUser.Hierarchy <= user.Hierarchy)
            {
                EmbedBuilder builder = new EmbedBuilder()
                    .WithTitle($":x: Ошибка!")
                    .WithDescription("Вы не можете кикнуть\nэтого пользователя")
                    .WithCurrentTimestamp()
                    .WithColor(Color.DarkRed)
                    .WithFooter(footer =>
                    {
                        footer
                        .WithText("Module: Ban")
                        .WithIconUrl(Context.Guild.IconUrl);
                    });
                await ReplyAsync(embed: builder.Build());
                return;
            }

            if (user == null)
            {
                EmbedBuilder builder = new EmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Module: Kick")
                    .WithIconUrl(Context.Guild.IconUrl);
                })
                .WithDescription($"Бан\n/kick `[пользователь]` `[причина]`")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: builder.Build());

                return;
            }

            if (reason == null)
            {
                EmbedBuilder builder = new EmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Module: Kick")
                    .WithIconUrl(Context.Guild.IconUrl);
                })
                .WithDescription($"Бан\n/kick `[пользователь]` `[причина]`")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: builder.Build());

                return;
            }

            var embed = new VrotEmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer => footer.Text = Context.User.Username)
                .WithDescription($":white_check_mark: {user.Mention}был кикнут\n**Причина :** {reason}")
                .WithCurrentTimestamp();

            await ReplyAsync(embed: embed.Build());
            await user.KickAsync(reason);
        }
        
        [SlashCommand("purge", "Очищает определенное количество сообщений в канале")]
        [Discord.Commands.RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "У вас нет прав!")]
        public async Task PurgeAsync(int Amount)
        {
            try
            {
                await DeferAsync();
                SocketTextChannel channel = Context.Channel as SocketTextChannel;
                var msgs = await channel.GetMessagesAsync(limit: Amount + 1).FlattenAsync();
                await channel.DeleteMessagesAsync(msgs);
                var embed = new VrotEmbedBuilder()
                    .WithTitle($"Успешно очищено {Amount} сообщений")
                    .WithCurrentTimestamp()
                    .WithFooter(footer =>
                    {
                        footer
                            .WithText("Module: Purge")
                            .WithIconUrl(Context.Guild.IconUrl);
                    });
                await ReplyAsync(embed: embed.Build());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var embed = new EmbedBuilder()
                    .WithTitle(":x: Произошла ошибка")
                    .WithDescription($"Сообщение об ошибке: {ex.Message}")
                    .WithColor(Color.DarkRed)
                    .WithCurrentTimestamp();
                await Context.Channel.SendMessageAsync(embed: embed.Build());
            }
        }
        
        [SlashCommand("report", "пожаловаться на пользователя")]
        public async Task ButtomCommandHandler(SocketGuildUser? user = null, string reason = null)
        {
            if (user == null)
            {
                EmbedBuilder builder = new EmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Module: Report")
                    .WithIconUrl(Context.Guild.IconUrl);
                })
                .WithDescription($"Репорт\n/report `[пользователь]` `[причина]`")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: builder.Build());

                return;
            }

            if (reason == null)
            {
                EmbedBuilder builder = new EmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Module: Report")
                    .WithIconUrl(Context.Guild.IconUrl);
                })
                .WithDescription($"Репорт\n/report `[пользователь]` `[причина]`")
                .WithCurrentTimestamp();
                await ReplyAsync(embed: builder.Build());

                return;
            }

            var component = new ComponentBuilder();
                var button = new ButtonBuilder()
                {
                    Label = "Бан",
                    CustomId = $"button1:{user.Id}",
                    Style = ButtonStyle.Danger
                };
                var channel = Context.Guild.Channels.SingleOrDefault(x => x.Name == $"report - {Context.User}");

            component.WithButton(button);

            var newChannel = await Context.Guild.CreateTextChannelAsync($"report - {Context.User}");
                var newChannelId = newChannel.Id;

            component.WithButton(label: "Закрыть", style: ButtonStyle.Secondary, customId: $"button2:{newChannel.Id}");


            await newChannel.SendMessageAsync($"{Context.User.Mention} | {user.Mention}");
                var embed = new VrotEmbedBuilder()
                .WithAuthor($"Жалоба от {Context.User}")
                .WithFooter(footer => footer.Text = "Module: Report")
                .WithDescription($"**Участник:** {user.Mention}\n(ID: `{Context.User.Id}`)\n**Причина:** {reason}")
                .WithCurrentTimestamp()
                .Build();

                await RespondAsync($"Репорт создан, перейдите в {newChannel.Mention}", ephemeral: true);
                await newChannel.SendMessageAsync(embed: embed, components: component.Build());
                

        }
        [ComponentInteraction("button1:*")]
        [Discord.Interactions.RequireUserPermission(GuildPermission.BanMembers)]
        public async Task HandleButton2(ulong userid)
        {
            var component = new ComponentBuilder();
            await Context.Guild.AddBanAsync(userid);
            await RespondAsync(components: component.Build(), ephemeral: true);
        }
        [ComponentInteraction("button2:*", ignoreGroupNames: true)]
        public async Task HandleButton3(ulong Id)
        {
            var component = new ComponentBuilder();
            var channel = Context.Guild.GetTextChannel(Id);
            await channel.DeleteAsync();
            await RespondAsync(components: component.Build(), ephemeral: true);
        }
        
        [SlashCommand("say", "Вывод сообщения пользователя через бота")]
        [Discord.Commands.RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "У вас нет прав администратора")]
        public async Task EchoAsync([Remainder] string text)
        {
            await Context.Channel.SendMessageAsync(text);
            await Context.Channel.DeleteMessageAsync(Context.User.Id);
        }
    }
    
}
