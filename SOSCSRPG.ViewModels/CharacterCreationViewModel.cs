using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using SOSCSRPG.Services.Factories;
using SOSCSRPG.Models;
using SOSCSRPG.Services;

namespace SOSCSRPG.ViewModels
{
    /// <summary>
    /// ViewModel class for managing character creation.
    /// </summary>
    public class CharacterCreationViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event that is raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the game details.
        /// </summary>
        public GameDetails GameDetails { get; }

        /// <summary>
        /// Gets or sets the selected race for the character.
        /// </summary>
        public Race SelectedRace { get; init; }

        /// <summary>
        /// Gets or sets the name of the character.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the collection of player attributes.
        /// </summary>
        public ObservableCollection<PlayerAttribute> PlayerAttributes { get; } = new ObservableCollection<PlayerAttribute>();

        /// <summary>
        /// Gets a value indicating whether there are any races available.
        /// </summary>
        public bool HasRaces => GameDetails.Races.Any();

        /// <summary>
        /// Gets a value indicating whether there are any race attribute modifiers available.
        /// </summary>
        public bool HasRaceAttributeModifiers => HasRaces && GameDetails.Races.Any(r => r.PlayerAttributeModifiers.Any());

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterCreationViewModel"/> class.
        /// </summary>
        public CharacterCreationViewModel()
        {
            GameDetails = GameDetailsService.ReadGameDetails();
            if (HasRaces)
            {
                SelectedRace = GameDetails.Races.First();
            }

            RollNewCharacter();
        }

        /// <summary>
        /// Rolls a new character by generating random attribute values.
        /// </summary>
        public void RollNewCharacter()
        {
            PlayerAttributes.Clear();
            foreach (PlayerAttribute playerAttribute in GameDetails.PlayerAttributes)
            {
                playerAttribute.ReRoll();
                PlayerAttributes.Add(playerAttribute);
            }

            ApplyAttributeModifiers();
        }

        /// <summary>
        /// Applies the attribute modifiers based on the selected race.
        /// </summary>
        public void ApplyAttributeModifiers()
        {
            foreach (PlayerAttribute playerAttribute in PlayerAttributes)
            {
                var attributeRaceModifier = SelectedRace.PlayerAttributeModifiers.FirstOrDefault(pam => pam.AttributeKey.Equals(playerAttribute.Key));
                playerAttribute.ModifiedValue = playerAttribute.BaseValue + (attributeRaceModifier?.Modifier ?? 0);
            }
        }

        /// <summary>
        /// Creates and returns a new player object based on the character creation settings.
        /// </summary>
        /// <returns>A new player object.</returns>
        public Player GetPlayer()
        {
            Player player = new Player(Name, 0, 10, 10, PlayerAttributes, 10);

            // Give player default inventory items, weapons, recipes, etc.
            player.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            player.AddItemToInventory(ItemFactory.CreateGameItem(2001));
            player.LearnRecipe(RecipeFactory.RecipeByID(1));
            player.AddItemToInventory(ItemFactory.CreateGameItem(3001));
            player.AddItemToInventory(ItemFactory.CreateGameItem(3002));
            player.AddItemToInventory(ItemFactory.CreateGameItem(3003));

            return player;
        }
    }
}