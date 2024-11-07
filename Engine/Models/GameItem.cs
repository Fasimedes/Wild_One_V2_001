using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    // Class representing a generic game item
    public class GameItem
    {
        // Unique identifier for the item type
        public int ItemTypeID { get; set; }

        // Name of the item
        public string Name { get; set; }

        // Price of the item in the game
        public int Price { get; set; }

        // Constructor for the GameItem class that initializes the item properties
        public GameItem(int itemTypeID, string name, int price)
        {
            ItemTypeID = itemTypeID; // Sets the unique item type ID
            Name = name; // Sets the name of the item
            Price = price; // Sets the price of the item
        }

        // Method for cloning the game item, creating a new instance with the same properties
        public GameItem Clone()
        {
            return new GameItem(ItemTypeID, Name, Price); // Returns a new GameItem with the same attributes
        }
    }
}
