using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    // Represents an item and the quantity of that item
    public class ItemQuantity
    {
        // Unique identifier for the item
        public int ItemID { get; set; }

        // Quantity of the item
        public int Quantity { get; set; }

        // Constructor initializes the item with an ID and quantity
        public ItemQuantity(int itemID, int quantity)
        {
            ItemID = itemID;    // Set the item's ID
            Quantity = quantity; // Set the quantity of the item
        }
    }
}
