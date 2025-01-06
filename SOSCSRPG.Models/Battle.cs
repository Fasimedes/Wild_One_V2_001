using System;
using SOSCSRPG.Models.Shared;
using SOSCSRPG.Core;
using SOSCSRPG.Models.EventArgs;
namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing a battle between a player and a monster.
    /// </summary>
    public class Battle : IDisposable
    {
        // Message broker instance for raising messages
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();

        // The player involved in the battle
        private readonly Player _player;

        // The monster opponent in the battle
        private readonly Monster _opponent;

        // Enum representing the combatants in the battle
        private enum Combatant
        {
            Player,
            Opponent
        }

        /// <summary>
        /// Event that is raised when the player wins the combat.
        /// </summary>
        public event EventHandler<CombatVictoryEventArgs> OnCombatVictory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Battle"/> class.
        /// </summary>
        /// <param name="player">The player involved in the battle.</param>
        /// <param name="opponent">The monster opponent in the battle.</param>
        public Battle(Player player, Monster opponent)
        {
            _player = player;
            _opponent = opponent;

            // Subscribe to events
            _player.OnActionPerformed += OnCombatantActionPerformed;
            _opponent.OnActionPerformed += OnCombatantActionPerformed;
            _opponent.OnKilled += OnOpponentKilled;

            // Raise initial messages
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"You see a {_opponent.Name} here!");

            // Determine the first attacker and attack if it's the opponent
            if (FirstAttacker(_player, _opponent) == Combatant.Opponent)
            {
                AttackPlayer();
            }
        }

        /// <summary>
        /// Attacks the opponent.
        /// </summary>
        public void AttackOpponent()
        {
            if (_player.CurrentWeapon == null)
            {
                _messageBroker.RaiseMessage("You must select a weapon, to attack.");
                return;
            }

            _player.UseCurrentWeaponOn(_opponent);

            if (_opponent.IsAlive)
            {
                AttackPlayer();
            }
        }

        /// <summary>
        /// Disposes of the battle, unsubscribing from events.
        /// </summary>
        public void Dispose()
        {
            _player.OnActionPerformed -= OnCombatantActionPerformed;
            _opponent.OnActionPerformed -= OnCombatantActionPerformed;
            _opponent.OnKilled -= OnOpponentKilled;
        }

        /// <summary>
        /// Event handler for when the opponent is killed.
        /// </summary>
        private void OnOpponentKilled(object sender, System.EventArgs e)
        {
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"You defeated the {_opponent.Name}!");
            _messageBroker.RaiseMessage($"You receive {_opponent.RewardExperiencePoints} experience points.");
            _player.AddExperience(_opponent.RewardExperiencePoints);
            _messageBroker.RaiseMessage($"You receive {_opponent.Gold} gold.");
            _player.ReceiveGold(_opponent.Gold);

            // Add opponent's items to player's inventory
            foreach (GameItem gameItem in _opponent.Inventory.Items)
            {
                _messageBroker.RaiseMessage($"You receive one {gameItem.Name}.");
                _player.AddItemToInventory(gameItem);
            }

            // Raise the combat victory event
            OnCombatVictory?.Invoke(this, new CombatVictoryEventArgs());
        }

        /// <summary>
        /// Attacks the player.
        /// </summary>
        private void AttackPlayer()
        {
            _opponent.UseCurrentWeaponOn(_player);
        }

        /// <summary>
        /// Event handler for when a combatant performs an action.
        /// </summary>
        private void OnCombatantActionPerformed(object sender, string result)
        {
            _messageBroker.RaiseMessage(result);
        }

        /// <summary>
        /// Determines the first attacker based on dexterity and a random factor.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="opponent">The monster opponent.</param>
        /// <returns>The combatant that attacks first.</returns>
        private static Combatant FirstAttacker(Player player, Monster opponent)
        {
            // Formula is: ((Dex(player)^2 - Dex(monster)^2)/10) + Random(-10/10)
            // For dexterity values from 3 to 18, this should produce an offset of +/- 41.5
            int playerDexterity = player.GetAttribute("DEX").ModifiedValue *
                                  player.GetAttribute("DEX").ModifiedValue;
            int opponentDexterity = opponent.GetAttribute("DEX").ModifiedValue *
                                    opponent.GetAttribute("DEX").ModifiedValue;
            decimal dexterityOffset = (playerDexterity - opponentDexterity) / 10m;
            int randomOffset = DiceService.Instance.Roll(20).Value - 10;
            decimal totalOffset = dexterityOffset + randomOffset;
            return DiceService.Instance.Roll(100).Value <= 50 + totalOffset
                ? Combatant.Player
                : Combatant.Opponent;
        }
    }
}