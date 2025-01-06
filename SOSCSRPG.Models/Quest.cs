using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing a quest in the game.
    /// </summary>
    public class Quest
    {
        /// <summary>
        /// Gets the ID of the quest.
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Gets the name of the quest.
        /// </summary>
        [JsonIgnore]
        public string Name { get; }

        /// <summary>
        /// Gets the description of the quest.
        /// </summary>
        [JsonIgnore]
        public string Description { get; }

        /// <summary>
        /// Gets the list of items required to complete the quest.
        /// </summary>
        [JsonIgnore]
        public List<ItemQuantity> ItemsToComplete { get; }

        /// <summary>
        /// Gets the experience points rewarded for completing the quest.
        /// </summary>
        [JsonIgnore]
        public int RewardExperiencePoints { get; }

        /// <summary>
        /// Gets the gold rewarded for completing the quest.
        /// </summary>
        [JsonIgnore]
        public int RewardGold { get; }

        /// <summary>
        /// Gets the list of items rewarded for completing the quest.
        /// </summary>
        [JsonIgnore]
        public List<ItemQuantity> RewardItems { get; }

        /// <summary>
        /// Gets the tooltip contents for the quest.
        /// </summary>
        [JsonIgnore]
        public string ToolTipContents =>
            Description + Environment.NewLine + Environment.NewLine +
            "Items to complete the quest" + Environment.NewLine +
            "===========================" + Environment.NewLine +
            string.Join(Environment.NewLine, ItemsToComplete.Select(i => i.QuantityItemDescription)) +
            Environment.NewLine + Environment.NewLine +
            "Rewards\r\n" +
            "===========================" + Environment.NewLine +
            $"{RewardExperiencePoints} experience points" + Environment.NewLine +
            $"{RewardGold} gold pieces" + Environment.NewLine +
            string.Join(Environment.NewLine, RewardItems.Select(i => i.QuantityItemDescription));

        /// <summary>
        /// Initializes a new instance of the <see cref="Quest"/> class with the specified properties.
        /// </summary>
        /// <param name="id">The ID of the quest.</param>
        /// <param name="name">The name of the quest.</param>
        /// <param name="description">The description of the quest.</param>
        /// <param name="itemsToComplete">The list of items required to complete the quest.</param>
        /// <param name="rewardExperiencePoints">The experience points rewarded for completing the quest.</param>
        /// <param name="rewardGold">The gold rewarded for completing the quest.</param>
        /// <param name="rewardItems">The list of items rewarded for completing the quest.</param>
        public Quest(int id, string name, string description, List<ItemQuantity> itemsToComplete,
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
