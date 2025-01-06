using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSCSRPG.Models.Actions;
using Newtonsoft.Json;

namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing a game item.
    /// </summary>
    public class GameItem
    {
        /// <summary>
        /// Enum representing the category of the item.
        /// </summary>
        public enum ItemCategory
        {
            Miscellaneous,
            Weapon,
            Consumable
        }

        /// <summary>
        /// Gets the category of the item.
        /// </summary>
        [JsonIgnore]
        public ItemCategory Category { get; }

        /// <summary>
        /// Gets the item type ID.
        /// </summary>
        public int ItemTypeID { get; }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        [JsonIgnore]
        public string Name { get; }

        /// <summary>
        /// Gets the price of the item.
        /// </summary>
        [JsonIgnore]
        public int Price { get; }

        /// <summary>
        /// Gets a value indicating whether the item is unique.
        /// </summary>
        [JsonIgnore]
        public bool IsUnique { get; }

        /// <summary>
        /// Gets or sets the action associated with the item.
        /// </summary>
        [JsonIgnore]
        public IAction Action { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameItem"/> class with the specified properties.
        /// </summary>
        /// <param name="category">The category of the item.</param>
        /// <param name="itemTypeID">The item type ID.</param>
        /// <param name="name">The name of the item.</param>
        /// <param name="price">The price of the item.</param>
        /// <param name="isUnique">Indicates whether the item is unique.</param>
        /// <param name="action">The action associated with the item.</param>
        public GameItem(ItemCategory category, int itemTypeID, string name, int price,
            bool isUnique = false, IAction action = null)
        {
            Category = category;
            ItemTypeID = itemTypeID;
            Name = name;
            Price = price;
            IsUnique = isUnique;
            Action = action;
        }

        /// <summary>
        /// Performs the action associated with the item.
        /// </summary>
        /// <param name="actor">The entity performing the action.</param>
        /// <param name="target">The entity being targeted by the action.</param>
        public void PerformAction(LivingEntity actor, LivingEntity target)
        {
            Action?.Execute(actor, target);
        }

        /// <summary>
        /// Creates a clone of the current game item.
        /// </summary>
        /// <returns>A new <see cref="GameItem"/> instance with the same properties as the current item.</returns>
        public GameItem Clone()
        {
            return new GameItem(Category, ItemTypeID, Name, Price, IsUnique, Action);
        }
    }
}
