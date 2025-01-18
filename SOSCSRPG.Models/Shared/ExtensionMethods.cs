using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using SOSCSRPG.Models;
using Newtonsoft.Json.Linq;

namespace SOSCSRPG.Models.Shared
{
    /// <summary>
    /// Static class containing extension methods for various types.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Gets the value of the specified attribute as an integer.
        /// </summary>
        /// <param name="node">The XML node.</param>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The attribute value as an integer.</returns>
        public static int AttributeAsInt(this XmlNode node, string attributeName)
        {
            return Convert.ToInt32(node.AttributeAsString(attributeName));
        }

        /// <summary>
        /// Gets the value of the specified attribute as a string.
        /// </summary>
        /// <param name="node">The XML node.</param>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The attribute value as a string.</returns>
        public static string AttributeAsString(this XmlNode node, string attributeName)
        {
            XmlAttribute attribute = node.Attributes?[attributeName];

            if (attribute == null)
            {
               // Console.WriteLine($"Missing attribute '{attributeName}' in node: {node.OuterXml}");
                throw new ArgumentException($"The attribute '{attributeName}' does not exist");
            }

            return attribute.Value;
        }

        /// <summary>
        /// Gets the string value of the specified key from a JSON object.
        /// </summary>
        /// <param name="jsonObject">The JSON object.</param>
        /// <param name="key">The key.</param>
        /// <returns>The string value of the key.</returns>
        public static string StringValueOf(this JObject jsonObject, string key)
        {
            return jsonObject[key].ToString();
        }

        /// <summary>
        /// Gets the string value of the specified key from a JSON token.
        /// </summary>
        /// <param name="jsonToken">The JSON token.</param>
        /// <param name="key">The key.</param>
        /// <returns>The string value of the key.</returns>
        public static string StringValueOf(this JToken jsonToken, string key)
        {
            return jsonToken[key].ToString();
        }

        /// <summary>
        /// Gets the integer value of the specified key from a JSON token.
        /// </summary>
        /// <param name="jsonToken">The JSON token.</param>
        /// <param name="key">The key.</param>
        /// <returns>The integer value of the key.</returns>
        public static int IntValueOf(this JToken jsonToken, string key)
        {
            return Convert.ToInt32(jsonToken[key]);
        }

        /// <summary>
        /// Gets the player attribute with the specified key from a living entity.
        /// </summary>
        /// <param name="entity">The living entity.</param>
        /// <param name="attributeKey">The attribute key.</param>
        /// <returns>The player attribute with the specified key.</returns>
        public static PlayerAttribute GetAttribute(this LivingEntity entity, string attributeKey)
        {
            return entity.Attributes
                .First(pa => pa.Key.Equals(attributeKey, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Gets the list of game items in the specified category from an inventory.
        /// </summary>
        /// <param name="inventory">The inventory.</param>
        /// <param name="category">The item category.</param>
        /// <returns>The list of game items in the specified category.</returns>
        public static List<GameItem> ItemsThatAre(this IEnumerable<GameItem> inventory, GameItem.ItemCategory category)
        {
            return inventory.Where(i => i.Category == category).ToList();
        }
    }
}