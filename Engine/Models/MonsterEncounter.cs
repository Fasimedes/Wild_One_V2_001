using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class MonsterEncounter
    {
        public int MonsterID { get; set; } // Property to hold the ID of the monster
        public int ChanceOfEncountering { get; set; } // Property to hold the chance of encountering the monster

        // Constructor to initialize a MonsterEncounter with specified monster ID and chance of encountering
        public MonsterEncounter(int monsterID, int chanceOfEncountering)
        {
            MonsterID = monsterID; // Set the MonsterID
            ChanceOfEncountering = chanceOfEncountering; // Set the chance of encountering the monster
        }
    }
}
