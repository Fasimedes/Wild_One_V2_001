using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing a trader in the game.
    /// </summary>
    public class Trader : LivingEntity
    {
        /// <summary>
        /// Gets the ID of the trader.
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trader"/> class with the specified ID and name.
        /// </summary>
        /// <param name="id">The ID of the trader.</param>
        /// <param name="name">The name of the trader.</param>
        public Trader(int id, string name) : base(name, 9999, 9999, new List<PlayerAttribute>(), 9999)
        {
            ID = id;
        }
    }
}
