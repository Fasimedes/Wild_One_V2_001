using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing a location in the game.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Gets the X coordinate of the location.
        /// </summary>
        public int XCoordinate { get; }

        /// <summary>
        /// Gets the Y coordinate of the location.
        /// </summary>
        public int YCoordinate { get; }

        /// <summary>
        /// Gets the name of the location.
        /// </summary>
        [JsonIgnore]
        public string Name { get; }

        /// <summary>
        /// Gets the description of the location.
        /// </summary>
        [JsonIgnore]
        public string Description { get; }

        /// <summary>
        /// Gets the image name associated with the location.
        /// </summary>
        [JsonIgnore]
        public string ImageName { get; }

        public DialogueNode DialogueNode { get; set; }

        /// <summary>
        /// Gets the list of quests available at the location.
        /// </summary>
        [JsonIgnore]
        public List<Quest> QuestsAvailableHere { get; } = new List<Quest>();

        /// <summary>
        /// Gets the list of monsters that can be encountered at the location.
        /// </summary>
        [JsonIgnore]
        public List<MonsterEncounter> MonstersHere { get; } = new List<MonsterEncounter>();

        /// <summary>
        /// Gets or sets the trader present at the location.
        /// </summary>
        [JsonIgnore]
        public Trader TraderHere { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class with the specified coordinates, name, description, and image name.
        /// </summary>
        /// <param name="xCoordinate">The X coordinate of the location.</param>
        /// <param name="yCoordinate">The Y coordinate of the location.</param>
        /// <param name="name">The name of the location.</param>
        /// <param name="description">The description of the location.</param>
        /// <param name="imageName">The image name associated with the location.</param>
        public Location(int xCoordinate, int yCoordinate, string name, string description, string imageName)
        {
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            Name = name;
            Description = description;
            ImageName = imageName;
        }

        /// <summary>
        /// Adds a monster to the list of monsters that can be encountered at the location.
        /// </summary>
        /// <param name="monsterID">The ID of the monster.</param>
        /// <param name="chanceOfEncountering">The chance of encountering the monster.</param>
        public void AddMonster(int monsterID, int chanceOfEncountering)
        {
            if (MonstersHere.Exists(m => m.MonsterID == monsterID))
            {
                // This monster has already been added to this location.
                // So, overwrite the ChanceOfEncountering with the new number.
                MonstersHere.First(m => m.MonsterID == monsterID).ChanceOfEncountering = chanceOfEncountering;
            }
            else
            {
                // This monster is not already at this location, so add it.
                MonstersHere.Add(new MonsterEncounter(monsterID, chanceOfEncountering));
            }
        }
    }
}