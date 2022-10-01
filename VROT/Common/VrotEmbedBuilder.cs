using Discord;

namespace VROT.Common;

internal class VrotEmbedBuilder : EmbedBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VrotEmbedBuilder"/> class.
    /// </summary>
    public VrotEmbedBuilder()
    {
        WithColor(new Color(44, 47, 51));
    }
}