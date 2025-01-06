using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing a player in the game.
    /// </summary>
    public class Player : LivingEntity
    {
        #region Properties

        // Backing field for ExperiencePoints property
        private int _experiencePoints;

        /// <summary>
        /// Gets or sets the experience points of the player.
        /// </summary>
        public int ExperiencePoints
        {
            get => _experiencePoints;
            private set
            {
                _experiencePoints = value;
                SetLevelAndMaximumHitPoints();
            }
        }

        /// <summary>
        /// Gets the collection of quests the player has.
        /// </summary>
        public ObservableCollection<QuestStatus> Quests { get; } = new ObservableCollection<QuestStatus>();

        /// <summary>
        /// Gets the collection of recipes the player has learned.
        /// </summary>
        public ObservableCollection<Recipe> Recipes { get; } = new ObservableCollection<Recipe>();

        #endregion

        /// <summary>
        /// Event that is raised when the player levels up.
        /// </summary>
        public event EventHandler OnLeveledUp;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class with the specified properties.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <param name="experiencePoints">The experience points of the player.</param>
        /// <param name="maximumHitPoints">The maximum hit points of the player.</param>
        /// <param name="currentHitPoints">The current hit points of the player.</param>
        /// <param name="attributes">The attributes of the player.</param>
        /// <param name="gold">The gold amount of the player.</param>
        public Player(string name, int experiencePoints, int maximumHitPoints, int currentHitPoints, IEnumerable<PlayerAttribute> attributes, int gold)
            : base(name, maximumHitPoints, currentHitPoints, attributes, gold)
        {
            ExperiencePoints = experiencePoints;
        }

        /// <summary>
        /// Adds experience points to the player.
        /// </summary>
        /// <param name="experiencePoints">The experience points to add.</param>
        public void AddExperience(int experiencePoints)
        {
            ExperiencePoints += experiencePoints;
        }

        /// <summary>
        /// Adds a recipe to the player's learned recipes if not already known.
        /// </summary>
        /// <param name="recipe">The recipe to learn.</param>
        public void LearnRecipe(Recipe recipe)
        {
            if (!Recipes.Any(r => r.ID == recipe.ID))
            {
                Recipes.Add(recipe);
            }
        }

        /// <summary>
        /// Sets the player's level and maximum hit points based on experience points.
        /// </summary>
        private void SetLevelAndMaximumHitPoints()
        {
            int originalLevel = Level;
            Level = (ExperiencePoints / 100) + 1;
            if (Level != originalLevel)
            {
                MaximumHitPoints = Level * 10;
                OnLeveledUp?.Invoke(this, System.EventArgs.Empty);
            }
        }
    }
}
