using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Models.Factories
{
    internal static class QuestFactory // Static factory class responsible for creating quests
    {
        private static readonly List<Quest> _quests = new List<Quest>(); // List holding all quests available in the game
        static QuestFactory() // Static constructor to initialize the quest list
        {

            List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();// Declare the items need to complete the quest, and its reward items
            List<ItemQuantity> rewardItems = new List<ItemQuantity>(); // Declare the reward items for completing the quest

            itemsToComplete.Add(new ItemQuantity(9002, 3)); // Add specific item requirements (item ID: 9001, quantity: 5)
            rewardItems.Add(new ItemQuantity(1002, 1));      // Define the rewards (item ID: 1002, quantity: 1)

            // Create and add the quest to the quest list
            _quests.Add(new Quest(1,                                                // Unique quest ID
                                  "Clear the herb garden",                          // Quest name
                                  "Defeat the snakes in the Herbalist's garden",    // Description
                                  itemsToComplete,                                  // Required items for completion
                                  25,                                               // Experience points rewarded
                                  10,                                               // Gold rewarded
                                  rewardItems));                                    // Reward items
        }
        internal static Quest GetQuestByID(int id) // Method to retrieve a quest by its unique ID
        {
            return _quests.FirstOrDefault(quest => quest.ID == id); // Return quest or null if not found
        }
    }
}
