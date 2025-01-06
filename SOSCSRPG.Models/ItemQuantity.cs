namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing an item and its quantity.
    /// </summary>
    public class ItemQuantity
    {
        // The game item associated with this quantity
        private readonly GameItem _gameItem;

        /// <summary>
        /// Gets the item type ID of the game item.
        /// </summary>
        public int ItemID => _gameItem.ItemTypeID;

        /// <summary>
        /// Gets the quantity of the game item.
        /// </summary>
        public int Quantity { get; }

        /// <summary>
        /// Gets the description of the quantity and item name.
        /// </summary>
        public string QuantityItemDescription => $"{Quantity} {_gameItem.Name}";

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemQuantity"/> class with the specified item and quantity.
        /// </summary>
        /// <param name="item">The game item.</param>
        /// <param name="quantity">The quantity of the game item.</param>
        public ItemQuantity(GameItem item, int quantity)
        {
            _gameItem = item;
            Quantity = quantity;
        }
    }
}