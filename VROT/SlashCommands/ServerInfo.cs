using Discord;
using Discord.Interactions;
using VROT.Common;

namespace VROT.SlashCommands;

public class Info : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("serverinfo", "Информация о сервере.")]
    public async Task ServerInfoAsync()
    {
        // Collect guild information.
        var guild = Context.Guild;
        var textChannels = guild.TextChannels;
        var voiceChannels = guild.VoiceChannels;
        var emotes = guild.Emotes;
        var roles = guild.Roles;
        string guildIconUrl = guild.IconUrl;
        int staticEmotesCount = emotes.Count(emote => !emote.Animated);
        int animatedEmotesCount = emotes.Count(emote => emote.Animated);

        // Sort collection of roles to print alphabetically.
        var rolesList = roles.Select(role => role.Name).ToList();
        rolesList.Sort();

        var regionsList = await guild.GetVoiceRegionsAsync();
        var regions = regionsList.Select(region => region.Name);
        var embed = new VrotEmbedBuilder()
            .WithAuthor(guild.Name, guildIconUrl)
            .WithImageUrl(guild.BannerUrl)
            .WithThumbnailUrl(guildIconUrl)
            .AddField("Участники", guild.MemberCount.ToString(), true)
            .AddField("Владелец", $"{Context.Guild.Owner.Mention}", true)
            .AddField("Создан", $"{guild.CreatedAt.ToString()}", true)
            .AddField("Каналы", $"Teкстовых: {textChannels.Count.ToString()}\nГолосовых: {voiceChannels.Count.ToString()}", true)
            .AddField("Эмодзи", $"Обычные: {staticEmotesCount.ToString()}\nАнимированные: {animatedEmotesCount.ToString()}", true)
            .AddField("Бусты", $"Уровень: {((int)guild.PremiumTier).ToString()}\nБуст: {guild.PremiumSubscriptionCount.ToString()}", true)
            .AddField("Роли", string.Join(", ", roles.Count))
            .Build();

        var components = new ComponentBuilder()
            .WithButton("Фото логотипа", style: ButtonStyle.Link, url: guildIconUrl)
            .Build();

        await RespondAsync(embed: embed, components: components);
    }
}