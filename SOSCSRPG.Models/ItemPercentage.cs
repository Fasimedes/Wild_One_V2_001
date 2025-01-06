using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing an item with a percentage chance.
    /// </summary>
    public class ItemPercentage
    {
        /// <summary>
        /// Gets the ID of the item.
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Gets the percentage chance of the item.
        /// </summary>
        public int Percentage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemPercentage"/> class with the specified ID and percentage.
        /// </summary>
        /// <param name="id">The ID of the item.</param>
        /// <param name="percentage">The percentage chance of the item.</param>
        public ItemPercentage(int id, int percentage)
        {
            ID = id;
            Percentage = percentage;
        }
    }
}
