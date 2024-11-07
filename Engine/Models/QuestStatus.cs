using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    // Represents the status of a quest for a player
    public class QuestStatus
    {
        // The quest assigned to the player
        public Quest PlayerQuest { get; set; }

        // Indicates if the quest has been completed
        public bool IsCompleted { get; set; }

        // Constructor initializing the player's quest and setting completion to false
        public QuestStatus(Quest quest)
        {
            PlayerQuest = quest;   // Assign the given quest to the player
            IsCompleted = false;   // Initialize the quest as incomplete
        }
    }
}
