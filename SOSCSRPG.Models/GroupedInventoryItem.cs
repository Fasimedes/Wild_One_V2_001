using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing a grouped inventory item.
    /// </summary>
    public class GroupedInventoryItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Event that is raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the game item.
        /// </summary>
        public GameItem Item { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the game item.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupedInventoryItem"/> class with the specified item and quantity.
        /// </summary>
        /// <param name="item">The game item.</param>
        /// <param name="quantity">The quantity of the game item.</param>
        public GroupedInventoryItem(GameItem item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}
