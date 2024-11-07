using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Models.Factories
{
    internal class ItemFactory     // Factory for creating game items
    {
        private static readonly List<GameItem> _standardGameItems = new List<GameItem>(); // list of our game items // Static constructor to initialize the item list when the factory is first used
        static ItemFactory() // Upon first run of the static function, the constructor will run and will create some game items
        {                    
            _standardGameItems.Add(new Weapon(1001, "Pointy Stick", 1, 1, 2));
            _standardGameItems.Add(new Weapon(1002, "Rusty Sword", 5, 1, 3));
            _standardGameItems.Add(new GameItem(9001, "Snake fang", 1));
            _standardGameItems.Add(new GameItem(9002, "Snakeskin", 2));
            _standardGameItems.Add(new GameItem(9003, "Rat tail", 1));
            _standardGameItems.Add(new GameItem(9004, "Rat fur", 2));
            _standardGameItems.Add(new GameItem(9005, "Spider fang", 1));
            _standardGameItems.Add(new GameItem(9006, "Spider silk", 2));
        }
        public static GameItem CreateGameItem(int itemTypeID)         // Method to create a new instance of a game item by its ID
        {
            GameItem standardItem = _standardGameItems.FirstOrDefault(item => item.ItemTypeID == itemTypeID); // Finds the first matching item by ID from the list
            if (standardItem != null) // Returns a cloned instance if found, otherwise returns null
            {
                if (standardItem is Weapon)
                {
                    return (standardItem as Weapon).Clone();
                }
                return standardItem.Clone();
            }
            return null;
        }
    }
}
