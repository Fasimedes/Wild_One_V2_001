using System;
using D20Tek.DiceNotation;
using D20Tek.DiceNotation.DieRoller;
namespace SOSCSRPG.Core
{
    public class DiceService : IDiceService
    {
        // Static singleton instance of DiceService
        private static readonly IDiceService s_instance = new DiceService();

        /// <summary>
        /// Make constructor private to implement singleton pattern.
        /// </summary>
        private DiceService()
        {
        }

        /// <summary>
        /// Static singleton property to get the instance of DiceService.
        /// </summary>
        public static IDiceService Instance => s_instance;

        //--- IDiceService implementation

        // Property to get the IDice instance
        public IDice Dice { get; } = new Dice();

        // Property to get or set the IDieRoller instance
        public IDieRoller DieRoller { get; private set; } = new RandomDieRoller();

        // Property to get the IDiceConfiguration instance
        public IDiceConfiguration Configuration => Dice.Configuration;

        // Property to get or set the IDieRollTracker instance
        public IDieRollTracker RollTracker { get; private set; } = null;

        /// <summary>
        /// Configures the dice roller with the specified roller type and options.
        /// </summary>
        /// <param name="rollerType">The type of roller to use.</param>
        /// <param name="enableTracker">Whether to enable the roll tracker.</param>
        /// <param name="constantValue">The constant value for the ConstantDieRoller.</param>
        public void Configure(RollerType rollerType, bool enableTracker = false, int constantValue = 1)
        {
            RollTracker = enableTracker ? new DieRollTracker() : null;

            switch (rollerType)
            {
                case RollerType.Random:
                    DieRoller = new RandomDieRoller(RollTracker);
                    break;
                case RollerType.Crypto:
                    DieRoller = new CryptoDieRoller(RollTracker);
                    break;
                case RollerType.MathNet:
                    DieRoller = new MathNetDieRoller(RollTracker);
                    break;
                case RollerType.Constant:
                    DieRoller = new ConstantDieRoller(constantValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rollerType));
            }
        }

        /// <summary>
        /// Rolls the dice using the specified dice notation.
        /// </summary>
        /// <param name="diceNotation">The dice notation string (e.g., "2d6+3").</param>
        /// <returns>The result of the dice roll.</returns>
        public DiceResult Roll(string diceNotation) => Dice.Roll(diceNotation, DieRoller);

        /// <summary>
        /// Rolls the specified number of dice with the given number of sides and modifier.
        /// </summary>
        /// <param name="sides">The number of sides on the dice.</param>
        /// <param name="numDice">The number of dice to roll.</param>
        /// <param name="modifier">The modifier to add to the roll result.</param>
        /// <returns>The result of the dice roll.</returns>
        public DiceResult Roll(int sides, int numDice = 1, int modifier = 0)
        {
            DiceResult result = Dice.Dice(sides, numDice).Constant(modifier).Roll(DieRoller);
            Dice.Clear();
            return result;
        }
    }
}