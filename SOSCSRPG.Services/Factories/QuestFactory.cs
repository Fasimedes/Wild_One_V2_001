using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using SOSCSRPG.Models;
using SOSCSRPG.Models.Shared;
namespace SOSCSRPG.Services.Factories
{
    /// <summary>
    /// Factory class for creating and managing quests in the game.
    /// </summary>
    internal static class QuestFactory
    {
        // Path to the game data file
        private const string GAME_DATA_FILENAME = ".\\GameData\\Quests.xml";

        // List to hold the quests
        private static readonly List<Quest> _quests = new List<Quest>();

        /// <summary>
        /// Static constructor to load quests from the data file.
        /// </summary>
        static QuestFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                LoadQuestsFromNodes(data.SelectNodes("/Quests/Quest"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        /// <summary>
        /// Loads quests from the specified XML nodes.
        /// </summary>
        /// <param name="nodes">The XML nodes containing quest data.</param>
        private static void LoadQuestsFromNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                // Declare the items needed to complete the quest, and its reward items
                List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
                List<ItemQuantity> rewardItems = new List<ItemQuantity>();

                // Load items needed to complete the quest
                foreach (XmlNode childNode in node.SelectNodes("./ItemsToComplete/Item"))
                {
                    GameItem item = ItemFactory.CreateGameItem(childNode.AttributeAsInt("ID"));
                    itemsToComplete.Add(new ItemQuantity(item, childNode.AttributeAsInt("Quantity")));
                }

                // Load reward items for the quest
                foreach (XmlNode childNode in node.SelectNodes("./RewardItems/Item"))
                {
                    GameItem item = ItemFactory.CreateGameItem(childNode.AttributeAsInt("ID"));
                    rewardItems.Add(new ItemQuantity(item, childNode.AttributeAsInt("Quantity")));
                }

                // Add the quest to the list
                _quests.Add(new Quest(node.AttributeAsInt("ID"),
                                      node.SelectSingleNode("./Name")?.InnerText ?? "",
                                      node.SelectSingleNode("./Description")?.InnerText ?? "",
                                      itemsToComplete,
                                      node.AttributeAsInt("RewardExperiencePoints"),
                                      node.AttributeAsInt("RewardGold"),
                                      rewardItems));
            }
        }

        /// <summary>
        /// Gets a quest by its ID.
        /// </summary>
        /// <param name="id">The ID of the quest.</param>
        /// <returns>The quest with the specified ID, or null if not found.</returns>
        internal static Quest GetQuestByID(int id)
        {
            return _quests.FirstOrDefault(quest => quest.ID == id);
        }
    }
}