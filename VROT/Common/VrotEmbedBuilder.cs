using Discord;

namespace VROT.Common
{
    internal class VrotEmbedBuilder : EmbedBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VrotEmbedBuilder"/> class.
        /// </summary>
        public VrotEmbedBuilder()
        {
            WithColor(new Color(238, 62, 75));
        }
    }
}
