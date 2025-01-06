using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing the details of the game.
    /// </summary>
    public class GameDetails
    {
        /// <summary>
        /// Gets the title of the game.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the subtitle of the game.
        /// </summary>
        public string SubTitle { get; }

        /// <summary>
        /// Gets the version of the game.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Gets the list of player attributes in the game.
        /// </summary>
        public List<PlayerAttribute> PlayerAttributes { get; } = new List<PlayerAttribute>();

        /// <summary>
        /// Gets the list of races in the game.
        /// </summary>
        public List<Race> Races { get; } = new List<Race>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GameDetails"/> class with the specified title, subtitle, and version.
        /// </summary>
        /// <param name="title">The title of the game.</param>
        /// <param name="subTitle">The subtitle of the game.</param>
        /// <param name="version">The version of the game.</param>
        public GameDetails(string title, string subTitle, string version)
        {
            Title = title;
            SubTitle = subTitle;
            Version = version;
        }
    }
}