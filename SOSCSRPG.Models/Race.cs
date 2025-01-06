using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing a race in the game.
    /// </summary>
    public class Race
    {
        /// <summary>
        /// Gets or sets the key of the race.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the display name of the race.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets the list of attribute modifiers for the race.
        /// </summary>
        public List<PlayerAttributeModifier> PlayerAttributeModifiers { get; } = new List<PlayerAttributeModifier>();
    }
}
