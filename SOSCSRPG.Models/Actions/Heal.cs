using SOSCSRPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models.Actions
{
    /// <summary>
    /// Class representing an action to heal a living entity.
    /// </summary>
    public class Heal : BaseAction, IAction
    {
        // The amount of hit points to heal
        private readonly int _hitPointsToHeal;

        /// <summary>
        /// Initializes a new instance of the <see cref="Heal"/> class.
        /// </summary>
        /// <param name="itemInUse">The consumable item in use.</param>
        /// <param name="hitPointsToHeal">The amount of hit points to heal.</param>
        public Heal(GameItem itemInUse, int hitPointsToHeal)
            : base(itemInUse)
        {
            // Ensure the item is a consumable
            if (itemInUse.Category != GameItem.ItemCategory.Consumable)
            {
                throw new ArgumentException($"{itemInUse.Name} is not consumable");
            }

            _hitPointsToHeal = hitPointsToHeal;
        }

        /// <summary>
        /// Executes the heal action.
        /// </summary>
        /// <param name="actor">The entity performing the heal.</param>
        /// <param name="target">The entity being healed.</param>
        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = (actor is Player) ? "You" : $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "yourself" : $"the {target.Name.ToLower()}";

            // Report the result of the heal action
            ReportResult($"{actorName} heal {targetName} for {_hitPointsToHeal} point{(_hitPointsToHeal > 1 ? "s" : "")}.");
            target.Heal(_hitPointsToHeal);
        }
    }
}