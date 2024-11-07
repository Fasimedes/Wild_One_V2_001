using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    // Class representing a weapon, inheriting from GameItem
    public class Weapon : GameItem
    {
        // Minimum and maximum damage that the weapon can deal
        public int MinimumDamage { get; set; }
        public int MaximumDamage { get; set; }

        // Constructor for the Weapon class that initializes weapon properties
        public Weapon(int itemTypeID, string name, int price, int minDamage, int maxDamage)
            : base(itemTypeID, name, price) // Calls the constructor of the base class GameItem
        {
            MinimumDamage = minDamage; // Sets the minimum damage
            MaximumDamage = maxDamage; // Sets the maximum damage
        }

        // Method for cloning the weapon and returning a new instance
        public new Weapon Clone()
        {
            return new Weapon(ItemTypeID, Name, Price, MinimumDamage, MaximumDamage);
        }
    }
}
