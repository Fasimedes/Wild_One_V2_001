using System.Collections.Generic;
namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing a monster in the game.
    /// </summary>
    public class Monster : LivingEntity
    {
        /// <summary>
        /// Gets the ID of the monster.
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Gets the image name associated with the monster.
        /// </summary>
        public string ImageName { get; }

        /// <summary>
        /// Gets the experience points rewarded for defeating the monster.
        /// </summary>
        public int RewardExperiencePoints { get; }

        /// <summary>
        /// Gets the loot table for the monster.
        /// </summary>
        public List<ItemPercentage> LootTable { get; } = new List<ItemPercentage>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Monster"/> class with the specified properties.
        /// </summary>
        /// <param name="id">The ID of the monster.</param>
        /// <param name="name">The name of the monster.</param>
        /// <param name="imageName">The image name associated with the monster.</param>
        /// <param name="maximumHitPoints">The maximum hit points of the monster.</param>
        /// <param name="attributes">The attributes of the monster.</param>
        /// <param name="currentWeapon">The current weapon of the monster.</param>
        /// <param name="rewardExperiencePoints">The experience points rewarded for defeating the monster.</param>
        /// <param name="gold">The gold amount of the monster.</param>
        public Monster(int id, string name, string imageName,
                       int maximumHitPoints, IEnumerable<PlayerAttribute> attributes,
                       GameItem currentWeapon, int rewardExperiencePoints, int gold)
            : base(name, maximumHitPoints, maximumHitPoints, attributes, gold)
        {
            ID = id;
            ImageName = imageName;
            CurrentWeapon = currentWeapon;
            RewardExperiencePoints = rewardExperiencePoints;
        }

        /// <summary>
        /// Adds an item to the loot table with the specified ID and percentage chance.
        /// </summary>
        /// <param name="id">The ID of the item.</param>
        /// <param name="percentage">The percentage chance of the item.</param>
        public void AddItemToLootTable(int id, int percentage)
        {
            // Remove the entry from the loot table if it already contains an entry with this ID
            LootTable.RemoveAll(ip => ip.ID == id);
            LootTable.Add(new ItemPercentage(id, percentage));
        }

        /// <summary>
        /// Creates a clone of the current monster.
        /// </summary>
        /// <returns>A new <see cref="Monster"/> instance with the same properties as the current monster.</returns>
        public Monster Clone()
        {
            Monster newMonster = new Monster(ID, Name, ImageName, MaximumHitPoints, Attributes,
                                             CurrentWeapon, RewardExperiencePoints, Gold);
            newMonster.LootTable.AddRange(LootTable);
            return newMonster;
        }
    }
}