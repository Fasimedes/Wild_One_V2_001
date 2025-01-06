using SOSCSRPG.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SOSCSRPG.Models;
using SOSCSRPG.Models.Shared;

namespace SOSCSRPG.Services
{
    /// <summary>
    /// Service class for reading game details from a JSON file.
    /// </summary>
    public static class GameDetailsService
    {
        /// <summary>
        /// Reads the game details from the JSON file and returns a <see cref="GameDetails"/> object.
        /// </summary>
        /// <returns>A <see cref="GameDetails"/> object containing the game details.</returns>
        public static GameDetails ReadGameDetails()
        {
            // Read the JSON file
            JObject gameDetailsJson = JObject.Parse(File.ReadAllText(".\\GameData\\GameDetails.json"));

            // Create a new GameDetails object
            GameDetails gameDetails = new GameDetails(
                gameDetailsJson.StringValueOf("Title"),
                gameDetailsJson.StringValueOf("SubTitle"),
                gameDetailsJson.StringValueOf("Version")
            );

            // Add player attributes to the GameDetails object
            foreach (JToken token in gameDetailsJson["PlayerAttributes"])
            {
                gameDetails.PlayerAttributes.Add(new PlayerAttribute(
                    token.StringValueOf("Key"),
                    token.StringValueOf("DisplayName"),
                    token.StringValueOf("DiceNotation")
                ));
            }

            // Add races to the GameDetails object
            if (gameDetailsJson["Races"] != null)
            {
                foreach (JToken token in gameDetailsJson["Races"])
                {
                    Race race = new Race
                    {
                        Key = token.StringValueOf("Key"),
                        DisplayName = token.StringValueOf("DisplayName")
                    };

                    // Add player attribute modifiers to the race
                    if (token["PlayerAttributeModifiers"] != null)
                    {
                        foreach (JToken childToken in token["PlayerAttributeModifiers"])
                        {
                            race.PlayerAttributeModifiers.Add(new PlayerAttributeModifier
                            {
                                AttributeKey = childToken.StringValueOf("Key"),
                                Modifier = childToken.IntValueOf("Modifier")
                            });
                        }
                    }

                    gameDetails.Races.Add(race);
                }
            }

            return gameDetails;
        }
    }
}
