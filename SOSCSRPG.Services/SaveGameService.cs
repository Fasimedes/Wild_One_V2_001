using System;
using System.Collections.Generic;
using System.IO;
using SOSCSRPG.Services.Factories;
using SOSCSRPG.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace SOSCSRPG.Services
{
    /// <summary>
    /// Service class for saving and loading game states.
    /// </summary>
    public static class SaveGameService
    {
        /// <summary>
        /// Saves the game state to a file.
        /// </summary>
        /// <param name="gameState">The game state to save.</param>
        /// <param name="fileName">The name of the file to save to.</param>
        public static void Save(GameState gameState, string fileName)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(gameState, Formatting.Indented));
        }

        /// <summary>
        /// Loads the last saved game state from a file, or creates a new game state if the file does not exist.
        /// </summary>
        /// <param name="fileName">The name of the file to load from.</param>
        /// <returns>The loaded game state.</returns>
        public static GameState LoadLastSaveOrCreateNew(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"Filename: {fileName}");
            }

            // Save game file exists, so create the GameSession object from it.
            try
            {
                JObject data = JObject.Parse(File.ReadAllText(fileName));

                // Populate Player object
                Player player = CreatePlayer(data);
                int x = (int)data[nameof(GameState.XCoordinate)];
                int y = (int)data[nameof(GameState.YCoordinate)];

                // Create GameState object with saved game data
                return new GameState(player, x, y);
            }
            catch
            {
                throw new FormatException($"Error reading: {fileName}");
            }
        }

        /// <summary>
        /// Creates a player object from the saved game data.
        /// </summary>
        /// <param name="data">The saved game data.</param>
        /// <returns>A new player object.</returns>
        private static Player CreatePlayer(JObject data)
        {
            Player player = new Player(
                (string)data[nameof(GameState.Player)][nameof(Player.Name)],
                (int)data[nameof(GameState.Player)][nameof(Player.ExperiencePoints)],
                (int)data[nameof(GameState.Player)][nameof(Player.MaximumHitPoints)],
                (int)data[nameof(GameState.Player)][nameof(Player.CurrentHitPoints)],
                GetPlayerAttributes(data),
                (int)data[nameof(GameState.Player)][nameof(Player.Gold)]
            );

            PopulatePlayerInventory(data, player);
            PopulatePlayerQuests(data, player);
            PopulatePlayerRecipes(data, player);

            return player;
        }

        /// <summary>
        /// Gets the player attributes from the saved game data.
        /// </summary>
        /// <param name="data">The saved game data.</param>
        /// <returns>A list of player attributes.</returns>
        private static IEnumerable<PlayerAttribute> GetPlayerAttributes(JObject data)
        {
            List<PlayerAttribute> attributes = new List<PlayerAttribute>();

            foreach (JToken itemToken in (JArray)data[nameof(GameState.Player)][nameof(Player.Attributes)])
            {
                attributes.Add(new PlayerAttribute(
                    (string)itemToken[nameof(PlayerAttribute.Key)],
                    (string)itemToken[nameof(PlayerAttribute.DisplayName)],
                    (string)itemToken[nameof(PlayerAttribute.DiceNotation)],
                    (int)itemToken[nameof(PlayerAttribute.BaseValue)],
                    (int)itemToken[nameof(PlayerAttribute.ModifiedValue)]
                ));
            }

            return attributes;
        }

        /// <summary>
        /// Populates the player's inventory from the saved game data.
        /// </summary>
        /// <param name="data">The saved game data.</param>
        /// <param name="player">The player object to populate.</param>
        private static void PopulatePlayerInventory(JObject data, Player player)
        {
            foreach (JToken itemToken in (JArray)data[nameof(GameState.Player)][nameof(Player.Inventory)][nameof(Inventory.Items)])
            {
                int itemId = (int)itemToken[nameof(GameItem.ItemTypeID)];
                player.AddItemToInventory(ItemFactory.CreateGameItem(itemId));
            }
        }

        /// <summary>
        /// Populates the player's quests from the saved game data.
        /// </summary>
        /// <param name="data">The saved game data.</param>
        /// <param name="player">The player object to populate.</param>
        private static void PopulatePlayerQuests(JObject data, Player player)
        {
            foreach (JToken questToken in (JArray)data[nameof(GameState.Player)][nameof(Player.Quests)])
            {
                int questId = (int)questToken[nameof(QuestStatus.PlayerQuest)][nameof(QuestStatus.PlayerQuest.ID)];
                Quest quest = QuestFactory.GetQuestByID(questId);
                QuestStatus questStatus = new QuestStatus(quest)
                {
                    IsCompleted = (bool)questToken[nameof(QuestStatus.IsCompleted)]
                };
                player.Quests.Add(questStatus);
            }
        }

        /// <summary>
        /// Populates the player's recipes from the saved game data.
        /// </summary>
        /// <param name="data">The saved game data.</param>
        /// <param name="player">The player object to populate.</param>
        private static void PopulatePlayerRecipes(JObject data, Player player)
        {
            foreach (JToken recipeToken in (JArray)data[nameof(GameState.Player)][nameof(Player.Recipes)])
            {
                int recipeId = (int)recipeToken[nameof(Recipe.ID)];
                Recipe recipe = RecipeFactory.RecipeByID(recipeId);
                player.Recipes.Add(recipe);
            }
        }
    }
}