using System.ComponentModel;
using SOSCSRPG.Core;
namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing a player attribute.
    /// </summary>
    public class PlayerAttribute : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the key of the attribute.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the display name of the attribute.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Gets the dice notation used to determine the attribute's value.
        /// </summary>
        public string DiceNotation { get; }

        /// <summary>
        /// Gets or sets the base value of the attribute.
        /// </summary>
        public int BaseValue { get; set; }

        /// <summary>
        /// Gets or sets the modified value of the attribute.
        /// </summary>
        public int ModifiedValue { get; set; }

        /// <summary>
        /// Event that is raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor that will use DiceService to create a BaseValue.
        /// The constructor this calls will put that same value into BaseValue and ModifiedValue.
        /// </summary>
        /// <param name="key">The key of the attribute.</param>
        /// <param name="displayName">The display name of the attribute.</param>
        /// <param name="diceNotation">The dice notation used to determine the attribute's value.</param>
        public PlayerAttribute(string key, string displayName, string diceNotation)
            : this(key, displayName, diceNotation, DiceService.Instance.Roll(diceNotation).Value)
        {
        }

        /// <summary>
        /// Constructor that takes a baseValue and also uses it for modifiedValue,
        /// for when we're creating a new attribute.
        /// </summary>
        /// <param name="key">The key of the attribute.</param>
        /// <param name="displayName">The display name of the attribute.</param>
        /// <param name="diceNotation">The dice notation used to determine the attribute's value.</param>
        /// <param name="baseValue">The base value of the attribute.</param>
        public PlayerAttribute(string key, string displayName, string diceNotation, int baseValue)
            : this(key, displayName, diceNotation, baseValue, baseValue)
        {
        }

        /// <summary>
        /// This constructor is eventually called by the others,
        /// or used when reading a Player's attributes from a saved game file.
        /// </summary>
        /// <param name="key">The key of the attribute.</param>
        /// <param name="displayName">The display name of the attribute.</param>
        /// <param name="diceNotation">The dice notation used to determine the attribute's value.</param>
        /// <param name="baseValue">The base value of the attribute.</param>
        /// <param name="modifiedValue">The modified value of the attribute.</param>
        public PlayerAttribute(string key, string displayName, string diceNotation, int baseValue, int modifiedValue)
        {
            Key = key;
            DisplayName = displayName;
            DiceNotation = diceNotation;
            BaseValue = baseValue;
            ModifiedValue = modifiedValue;
        }

        /// <summary>
        /// Re-rolls the attribute's value using the dice notation.
        /// </summary>
        public void ReRoll()
        {
            BaseValue = DiceService.Instance.Roll(DiceNotation).Value;
            ModifiedValue = BaseValue;
        }
    }
}