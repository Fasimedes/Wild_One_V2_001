using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using SOSCSRPG.Models;
using SOSCSRPG.Models.Shared;

namespace SOSCSRPG.Services.Factories
{
    /// <summary>
    /// Factory class for creating and managing the game world.
    /// </summary>
    public static class WorldFactory
    {
        // Path to the game data file
        private const string GAME_DATA_FILENAME = ".\\GameData\\Locations.xml";

        /// <summary>
        /// Creates the game world by loading locations from the data file.
        /// </summary>
        /// <returns>A new instance of the <see cref="World"/> class.</returns>
        public static World CreateWorld()
        {
            World world = new World();
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                string rootImagePath = data.SelectSingleNode("/Locations").AttributeAsString("RootImagePath");
                LoadLocationsFromNodes(world, rootImagePath, data.SelectNodes("/Locations/Location"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
            return world;
        }

        /// <summary>
        /// Loads locations from the specified XML nodes and adds them to the world.
        /// </summary>
        /// <param name="world">The world to add the locations to.</param>
        /// <param name="rootImagePath">The root image path for location images.</param>
        /// <param name="nodes">The XML nodes containing location data.</param>
        private static void LoadLocationsFromNodes(World world, string rootImagePath, XmlNodeList nodes)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes)
            {
                Location location = new Location(
                    node.AttributeAsInt("X"),
                    node.AttributeAsInt("Y"),
                    node.AttributeAsString("Name"),
                    node.SelectSingleNode("./Description")?.InnerText ?? "",
                    $".{rootImagePath}{node.AttributeAsString("ImageName")}"
                );

                AddMonsters(location, node.SelectNodes("./Monsters/Monster"));
                AddQuests(location, node.SelectNodes("./Quests/Quest"));
                AddTrader(location, node.SelectSingleNode("./Trader"));

                world.AddLocation(location);
            }
        }

        /// <summary>
        /// Adds monsters to the specified location from the XML nodes.
        /// </summary>
        /// <param name="location">The location to add the monsters to.</param>
        /// <param name="monsters">The XML nodes containing monster data.</param>
        private static void AddMonsters(Location location, XmlNodeList monsters)
        {
            if (monsters == null)
            {
                return;
            }

            foreach (XmlNode monsterNode in monsters)
            {
                location.AddMonster(monsterNode.AttributeAsInt("ID"), monsterNode.AttributeAsInt("Percent"));
            }
        }

        /// <summary>
        /// Adds quests to the specified location from the XML nodes.
        /// </summary>
        /// <param name="location">The location to add the quests to.</param>
        /// <param name="quests">The XML nodes containing quest data.</param>
        private static void AddQuests(Location location, XmlNodeList quests)
        {
            if (quests == null)
            {
                return;
            }

            foreach (XmlNode questNode in quests)
            {
                location.QuestsAvailableHere.Add(QuestFactory.GetQuestByID(questNode.AttributeAsInt("ID")));
            }
        }

        /// <summary>
        /// Adds a trader to the specified location from the XML node.
        /// </summary>
        /// <param name="location">The location to add the trader to.</param>
        /// <param name="traderHere">The XML node containing trader data.</param>
        private static void AddTrader(Location location, XmlNode traderHere)
        {
            if (traderHere == null)
            {
                return;
            }

            location.TraderHere = TraderFactory.GetTraderByID(traderHere.AttributeAsInt("ID"));
        }
    }
}