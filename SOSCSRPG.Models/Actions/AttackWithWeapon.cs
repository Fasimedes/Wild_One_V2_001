using System;
using SOSCSRPG.Models;
using SOSCSRPG.Models.Shared;
using SOSCSRPG.Core;
namespace SOSCSRPG.Models.Actions
{
    /// <summary>
    /// Class representing an action to attack with a weapon.
    /// </summary>
    public class AttackWithWeapon : BaseAction, IAction
    {
        // Dice notation for the weapon's damage
        private readonly string _damageDice;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttackWithWeapon"/> class.
        /// </summary>
        /// <param name="itemInUse">The weapon item in use.</param>
        /// <param name="damageDice">The dice notation for the weapon's damage.</param>
        public AttackWithWeapon(GameItem itemInUse, string damageDice)
            : base(itemInUse)
        {
            // Ensure the item is a weapon
            if (itemInUse.Category != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($"{itemInUse.Name} is not a weapon");
            }

            // Ensure the damage dice notation is valid
            if (string.IsNullOrWhiteSpace(damageDice))
            {
                throw new ArgumentException("damageDice must be valid dice notation");
            }

            _damageDice = damageDice;
        }

        /// <summary>
        /// Executes the attack action.
        /// </summary>
        /// <param name="actor">The entity performing the attack.</param>
        /// <param name="target">The entity being attacked.</param>
        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = (actor is Player) ? "You" : $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "you" : $"the {target.Name.ToLower()}";

            // Check if the attack succeeded
            if (AttackSucceeded(actor, target))
            {
                // Roll for damage
                int damage = DiceService.Instance.Roll(_damageDice).Value;
                ReportResult($"{actorName} hit {targetName} for {damage} point{(damage > 1 ? "s" : "")}.");
                target.TakeDamage(damage);
            }
            else
            {
                ReportResult($"{actorName} missed {targetName}.");
            }
        }

        /// <summary>
        /// Determines if the attack succeeded based on the attacker's and target's dexterity and a random factor.
        /// </summary>
        /// <param name="attacker">The entity performing the attack.</param>
        /// <param name="target">The entity being attacked.</param>
        /// <returns>True if the attack succeeded, otherwise false.</returns>
        private static bool AttackSucceeded(LivingEntity attacker, LivingEntity target)
        {
            // Currently using the same formula as FirstAttacker initiative.
            // This will change as we include attack/defense skills,
            // armor, weapon bonuses, enchantments/curses, etc.
            int playerDexterity = attacker.GetAttribute("DEX").ModifiedValue *
                                  attacker.GetAttribute("DEX").ModifiedValue;
            int opponentDexterity = target.GetAttribute("DEX").ModifiedValue *
                                    target.GetAttribute("DEX").ModifiedValue;
            decimal dexterityOffset = (playerDexterity - opponentDexterity) / 10m;
            int randomOffset = DiceService.Instance.Roll(20).Value - 10;
            decimal totalOffset = dexterityOffset + randomOffset;
            return DiceService.Instance.Roll(100).Value <= 50 + totalOffset;
        }
    }
}