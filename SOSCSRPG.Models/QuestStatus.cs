using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing the status of a quest for a player.
    /// </summary>
    public class QuestStatus : INotifyPropertyChanged
    {
        /// <summary>
        /// Event that is raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the quest associated with this status.
        /// </summary>
        public Quest PlayerQuest { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the quest is completed.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestStatus"/> class with the specified quest.
        /// </summary>
        /// <param name="quest">The quest associated with this status.</param>
        public QuestStatus(Quest quest)
        {
            PlayerQuest = quest;
            IsCompleted = false;
        }
    }
}
