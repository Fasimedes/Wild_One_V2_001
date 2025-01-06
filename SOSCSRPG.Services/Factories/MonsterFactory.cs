using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using SOSCSRPG.Models;
using SOSCSRPG.Services;
using SOSCSRPG.Models.Shared;
using SOSCSRPG.Core;
namespace SOSCSRPG.Services.Factories
{
    /// <summary>
    /// Factory class for creating and managing monsters in the game.
    /// </summary>
    public static class MonsterFactory
    {
        // Path to the game data file
        private const string GAME_DATA_FILENAME = ".\\GameData\\Monsters.xml";

        // Game details instance
        private static readonly GameDetails s_gameDetails;

        // List to hold the base monsters
        private static readonly List<Monster> s_baseMonsters = new List<Monster>();

        /// <summary>
        /// Static constructor to load monsters from the data file.
        /// </summary>
        static MonsterFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                s_gameDetails = GameDetailsService.ReadGameDetails();

                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                string rootImagePath = data.SelectSingleNode("/Monsters").AttributeAsString("RootImagePath");
                LoadMonstersFromNodes(data.SelectNodes("/Monsters/Monster"), rootImagePath);
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        /// <summary>
        /// Gets a monster from the specified location based on encounter chances.
        /// </summary>
        /// <param name="location">The location to get the monster from.</param>
        /// <returns>A monster instance, or null if no monsters are present.</returns>
        public static Monster GetMonsterFromLocation(Location location)
        {
            if (!location.MonstersHere.Any())
            {
                return null;
            }

            // Total the percentages of all monsters at this location.
            int totalChances = location.MonstersHere.Sum(m => m.ChanceOfEncountering);

            // Select a random number between 1 and the total (in case the total chances is not 100).
            int randomNumber = DiceService.Instance.Roll(totalChances, 1).Value;

            // Loop through the monster list, adding the monster's percentage chance of appearing to the runningTotal variable.
            // When the random number is lower than the runningTotal, that is the monster to return.
            int runningTotal = 0;
            foreach (MonsterEncounter monsterEncounter in location.MonstersHere)
            {
                runningTotal += monsterEncounter.ChanceOfEncountering;
                if (randomNumber <= runningTotal)
                {
                    return GetMonster(monsterEncounter.MonsterID);
                }
            }

            // If there was a problem, return the last monster in the list.
            return GetMonster(location.MonstersHere.Last().MonsterID);
        }

        /// <summary>
        /// Loads monsters from the specified XML nodes.
        /// </summary>
        /// <param name="nodes">The XML nodes containing monster data.</param>
        /// <param name="rootImagePath">The root image path for monster images.</param>
        private static void LoadMonstersFromNodes(XmlNodeList nodes, string rootImagePath)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                var attributes = s_gameDetails.PlayerAttributes;
                attributes.First(a => a.Key.Equals("DEX")).BaseValue = Convert.ToInt32(node.SelectSingleNode("./Dexterity").InnerText);
                attributes.First(a => a.Key.Equals("DEX")).ModifiedValue = Convert.ToInt32(node.SelectSingleNode("./Dexterity").InnerText);

                Monster monster = new Monster(
                    node.AttributeAsInt("ID"),
                    node.AttributeAsString("Name"),
                    $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                    node.AttributeAsInt("MaximumHitPoints"),
                    attributes,
                    ItemFactory.CreateGameItem(node.AttributeAsInt("WeaponID")),
                    node.AttributeAsInt("RewardXP"),
                    node.AttributeAsInt("Gold")
                );

                XmlNodeList lootItemNodes = node.SelectNodes("./LootItems/LootItem");

                if (lootItemNodes != null)
                {
                    foreach (XmlNode lootItemNode in lootItemNodes)
                    {
                        monster.AddItemToLootTable(lootItemNode.AttributeAsInt("ID"), lootItemNode.AttributeAsInt("Percentage"));
                    }
                }

                s_baseMonsters.Add(monster);
            }
        }

        /// <summary>
        /// Gets a monster instance by its ID.
        /// </summary>
        /// <param name="id">The ID of the monster.</param>
        /// <returns>A new monster instance with the same properties as the base monster.</returns>
        private static Monster GetMonster(int id)
        {
            Monster newMonster = s_baseMonsters.FirstOrDefault(m => m.ID == id).Clone();
            foreach (ItemPercentage itemPercentage in newMonster.LootTable)
            {
                // Populate the new monster's inventory, using the loot table
                if (DiceService.Instance.Roll(100).Value <= itemPercentage.Percentage)
                {
                    newMonster.AddItemToInventory(ItemFactory.CreateGameItem(itemPercentage.ID));
                }
            }
            return newMonster;
        }
    }
}