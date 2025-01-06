using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing a monster encounter in the game.
    /// </summary>
    public class MonsterEncounter
    {
        /// <summary>
        /// Gets the ID of the monster.
        /// </summary>
        public int MonsterID { get; }

        /// <summary>
        /// Gets or sets the chance of encountering the monster.
        /// </summary>
        public int ChanceOfEncountering { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonsterEncounter"/> class with the specified monster ID and chance of encountering.
        /// </summary>
        /// <param name="monsterID">The ID of the monster.</param>
        /// <param name="chanceOfEncountering">The chance of encountering the monster.</param>
        public MonsterEncounter(int monsterID, int chanceOfEncountering)
        {
            MonsterID = monsterID;
            ChanceOfEncountering = chanceOfEncountering;
        }
    }
}
