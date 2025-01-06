using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSCSRPG.Models;
using System.Xml;
using System.IO;
using SOSCSRPG.Models.Shared;

namespace SOSCSRPG.Services.Factories
{
    /// <summary>
    /// Factory class for creating and managing traders.
    /// </summary>
    public static class TraderFactory
    {
        // Path to the game data file
        private const string GAME_DATA_FILENAME = ".\\GameData\\Traders.xml";

        // List to hold the traders
        private static readonly List<Trader> _traders = new List<Trader>();

        /// <summary>
        /// Static constructor to load traders from the data file.
        /// </summary>
        static TraderFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                LoadTradersFromNodes(data.SelectNodes("/Traders/Trader"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        /// <summary>
        /// Loads traders from the specified XML nodes.
        /// </summary>
        /// <param name="nodes">The XML nodes containing trader data.</param>
        private static void LoadTradersFromNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                Trader trader = new Trader(
                    node.AttributeAsInt("ID"),
                    node.SelectSingleNode("./Name")?.InnerText ?? ""
                );

                foreach (XmlNode childNode in node.SelectNodes("./InventoryItems/Item"))
                {
                    int quantity = childNode.AttributeAsInt("Quantity");
                    // Create a new GameItem object for each item we add.
                    // This is to allow for unique items, like swords with enchantments.
                    for (int i = 0; i < quantity; i++)
                    {
                        trader.AddItemToInventory(ItemFactory.CreateGameItem(childNode.AttributeAsInt("ID")));
                    }
                }

                _traders.Add(trader);
            }
        }

        /// <summary>
        /// Gets a trader by its ID.
        /// </summary>
        /// <param name="id">The ID of the trader.</param>
        /// <returns>The trader with the specified ID, or null if not found.</returns>
        public static Trader GetTraderByID(int id)
        {
            return _traders.FirstOrDefault(t => t.ID == id);
        }
    }
}
