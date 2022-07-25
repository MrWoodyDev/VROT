namespace VROT.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord;

    internal class VrotEmbedBuilder : EmbedBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WatermelonEmbedBuilder"/> class.
        /// </summary>
        public VrotEmbedBuilder()
        {
            WithColor(new Color(238, 62, 75));
        }
    }
}
