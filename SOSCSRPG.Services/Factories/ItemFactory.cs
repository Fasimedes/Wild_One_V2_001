using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using SOSCSRPG.Models.Actions;
using SOSCSRPG.Models;
using SOSCSRPG.Models.Shared;
namespace SOSCSRPG.Services.Factories
{
    /// <summary>
    /// Factory class for creating game items.
    /// </summary>
    public static class ItemFactory
    {
        // Path to the game data file
        private const string GAME_DATA_FILENAME = ".\\GameData\\GameItems.xml";

        // List to hold the standard game items
        private static readonly List<GameItem> _standardGameItems = new List<GameItem>();

        /// <summary>
        /// Static constructor to load game items from the data file.
        /// </summary>
        static ItemFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                LoadItemsFromNodes(data.SelectNodes("/GameItems/Weapons/Weapon"));
                LoadItemsFromNodes(data.SelectNodes("/GameItems/HealingItems/HealingItem"));
                LoadItemsFromNodes(data.SelectNodes("/GameItems/MiscellaneousItems/MiscellaneousItem"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        /// <summary>
        /// Creates a game item with the specified item type ID.
        /// </summary>
        /// <param name="itemTypeID">The item type ID.</param>
        /// <returns>A clone of the game item with the specified item type ID, or null if not found.</returns>
        public static GameItem CreateGameItem(int itemTypeID)
        {
            return _standardGameItems.FirstOrDefault(item => item.ItemTypeID == itemTypeID)?.Clone();
        }

        /// <summary>
        /// Loads game items from the specified XML nodes.
        /// </summary>
        /// <param name="nodes">The XML nodes containing game item data.</param>
        private static void LoadItemsFromNodes(XmlNodeList nodes)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                GameItem.ItemCategory itemCategory = DetermineItemCategory(node.Name);

                GameItem gameItem =
                    new GameItem(itemCategory,
                                 node.AttributeAsInt("ID"),
                                 node.AttributeAsString("Name"),
                                 node.AttributeAsInt("Price"),
                                 itemCategory == GameItem.ItemCategory.Weapon);

                if (itemCategory == GameItem.ItemCategory.Weapon)
                {
                    gameItem.Action =
                        new AttackWithWeapon(gameItem, node.AttributeAsString("DamageDice"));
                }
                else if (itemCategory == GameItem.ItemCategory.Consumable)
                {
                    gameItem.Action =
                        new Heal(gameItem, node.AttributeAsInt("HitPointsToHeal"));
                }

                _standardGameItems.Add(gameItem);
            }
        }

        /// <summary>
        /// Determines the item category based on the item type.
        /// </summary>
        /// <param name="itemType">The item type as a string.</param>
        /// <returns>The item category.</returns>
        private static GameItem.ItemCategory DetermineItemCategory(string itemType)
        {
            switch (itemType)
            {
                case "Weapon":
                    return GameItem.ItemCategory.Weapon;
                case "HealingItem":
                    return GameItem.ItemCategory.Consumable;
                default:
                    return GameItem.ItemCategory.Miscellaneous;
            }
        }
    }
}