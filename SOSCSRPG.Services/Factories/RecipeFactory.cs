using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using SOSCSRPG.Models;
using SOSCSRPG.Models.Shared;
namespace SOSCSRPG.Services.Factories
{
    /// <summary>
    /// Factory class for creating and managing recipes in the game.
    /// </summary>
    public static class RecipeFactory
    {
        // Path to the game data file
        private const string GAME_DATA_FILENAME = ".\\GameData\\Recipes.xml";

        // List to hold the recipes
        private static readonly List<Recipe> _recipes = new List<Recipe>();

        /// <summary>
        /// Static constructor to load recipes from the data file.
        /// </summary>
        static RecipeFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                LoadRecipesFromNodes(data.SelectNodes("/Recipes/Recipe"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        /// <summary>
        /// Loads recipes from the specified XML nodes.
        /// </summary>
        /// <param name="nodes">The XML nodes containing recipe data.</param>
        private static void LoadRecipesFromNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                var ingredients = new List<ItemQuantity>();
                foreach (XmlNode childNode in node.SelectNodes("./Ingredients/Item"))
                {
                    GameItem item = ItemFactory.CreateGameItem(childNode.AttributeAsInt("ID"));
                    ingredients.Add(new ItemQuantity(item, childNode.AttributeAsInt("Quantity")));
                }

                var outputItems = new List<ItemQuantity>();
                foreach (XmlNode childNode in node.SelectNodes("./OutputItems/Item"))
                {
                    GameItem item = ItemFactory.CreateGameItem(childNode.AttributeAsInt("ID"));
                    outputItems.Add(new ItemQuantity(item, childNode.AttributeAsInt("Quantity")));
                }

                Recipe recipe = new Recipe(
                    node.AttributeAsInt("ID"),
                    node.SelectSingleNode("./Name")?.InnerText ?? "",
                    ingredients,
                    outputItems
                );

                _recipes.Add(recipe);
            }
        }

        /// <summary>
        /// Gets a recipe by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe.</param>
        /// <returns>The recipe with the specified ID, or null if not found.</returns>
        public static Recipe RecipeByID(int id)
        {
            return _recipes.FirstOrDefault(x => x.ID == id);
        }
    }
}