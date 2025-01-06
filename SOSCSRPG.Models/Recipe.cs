using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing a recipe in the game.
    /// </summary>
    public class Recipe
    {
        /// <summary>
        /// Gets the ID of the recipe.
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Gets the name of the recipe.
        /// </summary>
        [JsonIgnore]
        public string Name { get; }

        /// <summary>
        /// Gets the list of ingredients required for the recipe.
        /// </summary>
        [JsonIgnore]
        public List<ItemQuantity> Ingredients { get; }

        /// <summary>
        /// Gets the list of items produced by the recipe.
        /// </summary>
        [JsonIgnore]
        public List<ItemQuantity> OutputItems { get; }

        /// <summary>
        /// Gets the tooltip contents for the recipe.
        /// </summary>
        [JsonIgnore]
        public string ToolTipContents =>
            "Ingredients" + Environment.NewLine +
            "===========" + Environment.NewLine +
            string.Join(Environment.NewLine, Ingredients.Select(i => i.QuantityItemDescription)) +
            Environment.NewLine + Environment.NewLine +
            "Creates" + Environment.NewLine +
            "===========" + Environment.NewLine +
            string.Join(Environment.NewLine, OutputItems.Select(i => i.QuantityItemDescription));

        /// <summary>
        /// Initializes a new instance of the <see cref="Recipe"/> class with the specified properties.
        /// </summary>
        /// <param name="id">The ID of the recipe.</param>
        /// <param name="name">The name of the recipe.</param>
        /// <param name="ingredients">The list of ingredients required for the recipe.</param>
        /// <param name="outputItems">The list of items produced by the recipe.</param>
        public Recipe(int id, string name, List<ItemQuantity> ingredients, List<ItemQuantity> outputItems)
        {
            ID = id;
            Name = name;
            Ingredients = ingredients;
            OutputItems = outputItems;
        }
    }
}