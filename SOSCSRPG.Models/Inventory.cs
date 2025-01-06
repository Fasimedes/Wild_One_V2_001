using System.Collections.Generic;
using System.Linq;
using SOSCSRPG.Models.Shared;
using Newtonsoft.Json;
namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing an inventory of game items.
    /// </summary>
    public class Inventory
    {
        #region Backing variables
        // List to hold the actual inventory items
        private readonly List<GameItem> _backingInventory = new List<GameItem>();

        // List to hold grouped inventory items
        private readonly List<GroupedInventoryItem> _backingGroupedInventoryItems = new List<GroupedInventoryItem>();
        #endregion

        #region Properties
        /// <summary>
        /// Gets the list of items in the inventory.
        /// </summary>
        public IReadOnlyList<GameItem> Items => _backingInventory.AsReadOnly();

        /// <summary>
        /// Gets the list of grouped inventory items.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<GroupedInventoryItem> GroupedInventory => _backingGroupedInventoryItems.AsReadOnly();

        /// <summary>
        /// Gets the list of weapons in the inventory.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<GameItem> Weapons => _backingInventory.ItemsThatAre(GameItem.ItemCategory.Weapon).AsReadOnly();

        /// <summary>
        /// Gets the list of consumables in the inventory.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<GameItem> Consumables => _backingInventory.ItemsThatAre(GameItem.ItemCategory.Consumable).AsReadOnly();

        /// <summary>
        /// Gets a value indicating whether the inventory has any consumables.
        /// </summary>
        [JsonIgnore]
        public bool HasConsumable => Consumables.Any();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Inventory"/> class with the specified items.
        /// </summary>
        /// <param name="items">The items to add to the inventory.</param>
        public Inventory(IEnumerable<GameItem> items = null)
        {
            if (items == null)
            {
                return;
            }

            foreach (GameItem item in items)
            {
                _backingInventory.Add(item);
                AddItemToGroupedInventory(item);
            }
        }
        #endregion

        #region Public functions
        /// <summary>
        /// Checks if the inventory has all the specified items.
        /// </summary>
        /// <param name="items">The items to check for.</param>
        /// <returns>True if the inventory has all the specified items, otherwise false.</returns>
        public bool HasAllTheseItems(IEnumerable<ItemQuantity> items)
        {
            return items.All(item => Items.Count(i => i.ItemTypeID == item.ItemID) >= item.Quantity);
        }

        /// <summary>
        /// Adds an item to the inventory.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>A new inventory instance with the added item.</returns>
        public Inventory AddItem(GameItem item)
        {
            return AddItems(new List<GameItem> { item });
        }

        /// <summary>
        /// Adds multiple items to the inventory.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <returns>A new inventory instance with the added items.</returns>
        public Inventory AddItems(IEnumerable<GameItem> items)
        {
            return new Inventory(Items.Concat(items));
        }

        /// <summary>
        /// Removes an item from the inventory.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>A new inventory instance with the removed item.</returns>
        public Inventory RemoveItem(GameItem item)
        {
            return RemoveItems(new List<GameItem> { item });
        }

        /// <summary>
        /// Removes multiple items from the inventory.
        /// </summary>
        /// <param name="items">The items to remove.</param>
        /// <returns>A new inventory instance with the removed items.</returns>
        public Inventory RemoveItems(IEnumerable<GameItem> items)
        {
            // REFACTOR: Look for a cleaner solution, with fewer temporary variables.
            List<GameItem> workingInventory = Items.ToList();
            IEnumerable<GameItem> itemsToRemove = items.ToList();

            foreach (GameItem item in itemsToRemove)
            {
                workingInventory.Remove(item);
            }

            return new Inventory(workingInventory);
        }

        /// <summary>
        /// Removes items from the inventory based on item quantities.
        /// </summary>
        /// <param name="itemQuantities">The item quantities to remove.</param>
        /// <returns>A new inventory instance with the removed items.</returns>
        public Inventory RemoveItems(IEnumerable<ItemQuantity> itemQuantities)
        {
            // REFACTOR
            Inventory workingInventory = new Inventory(Items);

            foreach (ItemQuantity itemQuantity in itemQuantities)
            {
                for (int i = 0; i < itemQuantity.Quantity; i++)
                {
                    workingInventory = workingInventory.RemoveItem(workingInventory.Items.First(item => item.ItemTypeID == itemQuantity.ItemID));
                }
            }

            return workingInventory;
        }
        #endregion

        #region Private functions
        /// <summary>
        /// Adds an item to the grouped inventory.
        /// </summary>
        /// <param name="item">The item to add.</param>
        private void AddItemToGroupedInventory(GameItem item)
        {
            if (item.IsUnique)
            {
                _backingGroupedInventoryItems.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                if (_backingGroupedInventoryItems.All(gi => gi.Item.ItemTypeID != item.ItemTypeID))
                {
                    _backingGroupedInventoryItems.Add(new GroupedInventoryItem(item, 0));
                }

                _backingGroupedInventoryItems.First(gi => gi.Item.ItemTypeID == item.ItemTypeID).Quantity++;
            }
        }
        #endregion
    }
}