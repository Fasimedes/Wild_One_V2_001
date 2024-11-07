using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Quest
    {
        public int ID { get; set; } // Unique identifier for the quest
        public string Name { get; set; } // Display name of the quest
        public string Description { get; set; } // Brief description of the quest and its story
        public List<ItemQuantity> ItemsToComplete { get; set; } // List of required items with quantities to complete the quest
        public int RewardExperiencePoints { get; set; }  // Experience points awarded to the player upon quest completion
        public int RewardGold { get; set; } // Gold awarded to the player upon quest completion
        public List<ItemQuantity> RewardItems { get; set; } // List of item rewards with quantities given to the player upon quest completion
        public Quest(int id, string name, string description, List<ItemQuantity> itemsToComplete, // Constructor to initialize a new quest with all details
                     int rewardExperiencePoints, int rewardGold, List<ItemQuantity> rewardItems)
        {
            ID = id;
            Name = name;
            Description = description;
            ItemsToComplete = itemsToComplete;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            RewardItems = rewardItems;
        }
    }
}
