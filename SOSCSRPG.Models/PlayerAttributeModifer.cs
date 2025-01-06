using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing a modifier for a player attribute.
    /// </summary>
    public class PlayerAttributeModifier
    {
        /// <summary>
        /// Gets the key of the attribute to be modified.
        /// </summary>
        public string AttributeKey { get; init; }

        /// <summary>
        /// Gets the modifier value to be applied to the attribute.
        /// </summary>
        public int Modifier { get; init; }
    }
}
