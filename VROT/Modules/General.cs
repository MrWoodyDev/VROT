﻿using System.Security.Cryptography.X509Certificates;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using VROT.Common;
using VROT.Models;

namespace VROT.Modules;

public class General : ModuleBase<SocketCommandContext>
{
    private readonly IHttpClientFactory _httpClientFactory;
    public General(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [Command("ping")]
    [Alias("p")]
    public async Task PingAsync()
    {
        await Context.Message.ReplyAsync("pong");
        await Context.Channel.DeleteMessageAsync(Context.Message.Id);
    }

    [Command("info")]
    public async Task InfoAsync(SocketGuildUser? socketGuildUser = null)
    {
        socketGuildUser ??= Context.User as SocketGuildUser;

        if (socketGuildUser != null)
        {
            if (socketGuildUser.JoinedAt != null)
            {
                var embed = new VrotEmbedBuilder()
                    .WithTitle($"{socketGuildUser.Username}#{socketGuildUser.Discriminator}")
                    .AddField("ID", socketGuildUser.Id, true)
                    .AddField("Name", $"{socketGuildUser.Username}#{socketGuildUser.Discriminator}", true)
                    .AddField("Created at", $"<t:{((DateTimeOffset)socketGuildUser.CreatedAt).ToUnixTimeSeconds()}:f>", true)
                    .AddField("Join at", $"<t:{((DateTimeOffset)socketGuildUser.JoinedAt.Value).ToUnixTimeSeconds()}:f>", true)
                    .WithThumbnailUrl(socketGuildUser.GetAvatarUrl() ?? socketGuildUser.GetDefaultAvatarUrl())
                    .WithCurrentTimestamp()
                    .Build();

                await ReplyAsync(embed: embed);
            }
        }
    }

    [Command("activity")]
    public async Task Activity()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetStringAsync("http://www.boredapi.com/api/activity/");
        var activity = Event.FromJson(response);

        if (activity == null)
        {
            await ReplyAsync("An error occurred, please try again later.");
            return;
        }

        await ReplyAsync($"**Activity:** {activity.Activity}\n**Participants:** {activity.Participants}\n**Type:** {activity.Type}\n**Price:** {activity.Price}\n**Accessibility:** {activity.Accessibility}");
    }


    [Command("ball")]
    [Alias("шар")]
    public async Task Ball([Remainder] string? ask = null)
    {
        if (ask == null)
        {
            await ReplyAsync("**Напишите вопрос**");
        }
        else
        {
            int randNum = 0;
            Random random = new Random();
            randNum = random.Next(0, 6);

            switch (randNum)
            {
                case 0:
                    await Context.Message.ReplyAsync($"{Context.Message.Author.Mention}, да");
                    break;
                case 1:
                    await Context.Message.ReplyAsync($"{Context.Message.Author.Mention}, нет");
                    break;
                case 2:
                    await Context.Message.ReplyAsync($"{Context.Message.Author.Mention}, возможно да");
                    break;
                case 3:
                    await Context.Message.ReplyAsync($"{Context.Message.Author.Mention}, возможно нет");
                    break;
                case 4:
                    await Context.Message.ReplyAsync($"{Context.Message.Author.Mention}, иди нахуй");
                    break;
                case 5:
                    await Context.Message.ReplyAsync($"{Context.Message.Author.Mention}, за небольшую оплату, можешь пойти нахуй");
                    break;
            }
        }
    }

    [Command("emoji")]
    public async Task EmojiAsync(string emote)
    {
       var embed = new VrotEmbedBuilder()
           .WithTitle("Emoji")
           .AddField("Name", Emote.Parse(emote).Name, true)
           .AddField("ID", Emote.Parse(emote).Id, true)
           .AddField("Animated", Emote.Parse(emote).Animated, false)
           .AddField("URL", Emote.Parse(emote).Url, false)
           .WithThumbnailUrl(Emote.Parse(emote).Url)
           .WithCurrentTimestamp()
           .Build();

       await ReplyAsync(embed: embed);
    }
}