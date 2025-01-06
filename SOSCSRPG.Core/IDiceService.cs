using D20Tek.DiceNotation;
using D20Tek.DiceNotation.DieRoller;
namespace SOSCSRPG.Core
{
    /// <summary>
    /// Interface for dice service, providing methods for rolling dice and configuring the dice roller.
    /// </summary>
    public interface IDiceService
    {
        /// <summary>
        /// Gets the IDice instance.
        /// </summary>
        IDice Dice { get; }

        /// <summary>
        /// Gets the IDiceConfiguration instance.
        /// </summary>
        IDiceConfiguration Configuration { get; }

        /// <summary>
        /// Gets the IDieRollTracker instance.
        /// </summary>
        IDieRollTracker RollTracker { get; }

        /// <summary>
        /// Configures the dice roller with the specified roller type and options.
        /// </summary>
        /// <param name="rollerType">The type of roller to use.</param>
        /// <param name="enableTracker">Whether to enable the roll tracker.</param>
        /// <param name="constantValue">The constant value for the ConstantDieRoller.</param>
        void Configure(RollerType rollerType, bool enableTracker = false, int constantValue = 1);

        /// <summary>
        /// Rolls the dice using the specified dice notation.
        /// </summary>
        /// <param name="diceNotation">The dice notation string (e.g., "2d6+3").</param>
        /// <returns>The result of the dice roll.</returns>
        DiceResult Roll(string diceNotation);

        /// <summary>
        /// Rolls the specified number of dice with the given number of sides and modifier.
        /// </summary>
        /// <param name="sides">The number of sides on the dice.</param>
        /// <param name="numDice">The number of dice to roll.</param>
        /// <param name="modifier">The modifier to add to the roll result.</param>
        /// <returns>The result of the dice roll.</returns>
        DiceResult Roll(int sides, int numDice = 1, int modifier = 0);
    }
    /// <summary>
    /// Enum representing the different types of dice rollers.
    /// </summary>
    public enum RollerType
    {
        /// <summary>
        /// Random number generator-based roller.
        /// </summary>
        Random = 0,

        /// <summary>
        /// Cryptographically secure random number generator-based roller.
        /// </summary>
        Crypto = 1,

        /// <summary>
        /// Math.NET-based roller.
        /// </summary>
        MathNet = 2,

        /// <summary>
        /// Constant value roller.
        /// </summary>
        Constant = 3
    }
}