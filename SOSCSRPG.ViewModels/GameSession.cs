using System.ComponentModel;
using System.Linq;
using SOSCSRPG.Services.Factories;
using SOSCSRPG.Models;
using SOSCSRPG.Services;
using Newtonsoft.Json;
using SOSCSRPG.Core;
using System.Collections.ObjectModel;
using MathNet.Numerics;
using System.Runtime.InteropServices;
using System.Security.Claims;
namespace SOSCSRPG.ViewModels
{
    /// <summary>
    /// ViewModel class for managing the game session.
    /// </summary>
    public class GameSession : INotifyPropertyChanged, IDisposable
    {
        // Message broker instance for raising messages
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();

        #region Properties

        // Backing fields for properties
        private Player _currentPlayer;
        private Location _currentLocation;
        private Battle _currentBattle;
        private Monster _currentMonster;


        private DialogueNode _currentDialogueNode;
        public DialogueNode CurrentDialogueNode
        {
            get => _currentDialogueNode;
            set
            {
                _currentDialogueNode = value;
                //OnPropertyChanged(nameof(CurrentDialogueNode));
                UpdateDialogueOptions();
            }
        }
        // private DialogueNode _nextDialogueNode;

        private string _dialogueOption1;
        public string DialogueOption1
        {
            get => _dialogueOption1;
            set
            {
                _dialogueOption1 = value;
                //OnPropertyChanged(nameof(DialogueOption1));
            }
        }

        private string _dialogueOption2;
        public string DialogueOption2
        {
            get => _dialogueOption2;
            set
            {
                _dialogueOption2 = value;
                //OnPropertyChanged(nameof(DialogueOption2));
            }
        }

        private string _dialogueOutput;

        public string DialogueOutput
        {
            get => _dialogueOutput;
            set
            {
                _dialogueOutput = value;
                // OnPropertyChanged(nameof(DialogueOutput));
            }
        }

        private void UpdateDialogueOptions()
        {
            if (CurrentDialogueNode != null && CurrentDialogueNode.Choices.Count > 0)
            {
                DialogueOption1 = CurrentDialogueNode.Choices.Count > 0 ? CurrentDialogueNode.Choices[0].Text : string.Empty;
                DialogueOption2 = CurrentDialogueNode.Choices.Count > 1 ? CurrentDialogueNode.Choices[1].Text : string.Empty;
            }
            else
            {
                DialogueOption1 = string.Empty;
                DialogueOption2 = string.Empty;
            }
        }

        /// <summary>
        /// Event that is raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the game details.
        /// </summary>
        [JsonIgnore]
        public GameDetails GameDetails { get; private set; }

        /// <summary>
        /// Gets the current world.
        /// </summary>
        [JsonIgnore]
        public World CurrentWorld { get; }

        /// <summary>
        /// Gets or sets the current player.
        /// </summary>
        public Player CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLeveledUp -= OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled -= OnPlayerKilled;
                }
                _currentPlayer = value;
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLeveledUp += OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled += OnPlayerKilled;
                }
            }
        }

        /// <summary>
        /// Gets or sets the current location.
        /// </summary>
        public Location CurrentLocation
        {
            get => _currentLocation;
            set
            {
                _currentLocation = value;
                CompleteQuestsAtLocation();
                GivePlayerQuestsAtLocation();
                CurrentMonster = MonsterFactory.GetMonsterFromLocation(CurrentLocation);
                CurrentTrader = CurrentLocation.TraderHere;

                // Update the current dialogue node based on the location
                CurrentDialogueNode = CurrentLocation.DialogueNode;

                // Update the dialogue output to reflect the current location's dialogue
                DialogueOutput = CurrentDialogueNode?.Text ?? string.Empty;

                _messageBroker.RaiseMessage(DialogueOutput);


            }
        }

        /// <summary>
        /// Gets or sets the current monster.
        /// </summary>
        [JsonIgnore]
        public Monster CurrentMonster
        {
            get => _currentMonster;
            set
            {
                if (_currentBattle != null)
                {
                    _currentBattle.OnCombatVictory -= OnCurrentMonsterKilled;
                    _currentBattle.Dispose();
                    _currentBattle = null;
                }
                _currentMonster = value;
                if (_currentMonster != null)
                {
                    _currentBattle = new Battle(CurrentPlayer, CurrentMonster);
                    _currentBattle.OnCombatVictory += OnCurrentMonsterKilled;
                }
            }
        }

        /// <summary>
        /// Gets or sets the current trader.
        /// </summary>
        [JsonIgnore]
        public Trader CurrentTrader { get; private set; }

        /// <summary>
        /// Dialog node for the current dialogue.
        /// </summary>
        //public DialogueNode CurrentDialogueNode { get; set; }

        /// <summary>
        /// Gets the collection of game messages.
        /// </summary>
        [JsonIgnore]
        public ObservableCollection<string> GameMessages { get; } = new ObservableCollection<string>();

        /// <summary>
        /// Gets or sets the popup details for player details.
        /// </summary>
        public PopupDetails PlayerDetails { get; set; }

        /// <summary>
        /// Gets or sets the popup details for inventory details.
        /// </summary>
        public PopupDetails InventoryDetails { get; set; }

        /// <summary>
        /// Gets or sets the popup details for quest details.
        /// </summary>
        public PopupDetails QuestDetails { get; set; }

        /// <summary>
        /// Gets or sets the popup details for recipes details.
        /// </summary>
        public PopupDetails RecipesDetails { get; set; }

        /// <summary>
        /// Gets or sets the popup details for game messages details.
        /// </summary>
        public PopupDetails GameMessagesDetails { get; set; }

        /// <summary>
        /// Gets a value indicating whether there is a location to the north.
        /// </summary>
        [JsonIgnore]
        public bool HasLocationToNorth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;

        /// <summary>
        /// Gets a value indicating whether there is a location to the east.
        /// </summary>
        [JsonIgnore]
        public bool HasLocationToEast => CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;

        /// <summary>
        /// Gets a value indicating whether there is a location to the south.
        /// </summary>
        [JsonIgnore]
        public bool HasLocationToSouth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;

        /// <summary>
        /// Gets a value indicating whether there is a location to the west.
        /// </summary>
        [JsonIgnore]
        public bool HasLocationToWest => CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;

        /// <summary>
        /// Gets a value indicating whether there is a monster at the current location.
        /// </summary>
        [JsonIgnore]
        public bool HasMonster => CurrentMonster != null;

        /// <summary>
        /// Gets a value indicating whether there is a trader at the current location.
        /// </summary>
        [JsonIgnore]
        public bool HasTrader => CurrentTrader != null;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSession"/> class with the specified player and coordinates.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="xCoordinate">The X coordinate of the player's starting location.</param>
        /// <param name="yCoordinate">The Y coordinate of the player's starting location.</param>
        public GameSession(Player player, int xCoordinate, int yCoordinate)
        {
            PopulateGameDetails();
            CurrentWorld = WorldFactory.CreateWorld();
            CurrentPlayer = player;
            CurrentLocation = CurrentWorld.LocationAt(xCoordinate, yCoordinate);


            // Setup popup window properties
            PlayerDetails = new PopupDetails
            {
                IsVisible = false,
                Top = 10,
                Left = 10,
                MinHeight = 75,
                MaxHeight = 400,
                MinWidth = 265,
                MaxWidth = 400
            };
            InventoryDetails = new PopupDetails
            {
                IsVisible = false,
                Top = 500,
                Left = 10,
                MinHeight = 75,
                MaxHeight = 175,
                MinWidth = 250,
                MaxWidth = 400
            };
            QuestDetails = new PopupDetails
            {
                IsVisible = false,
                Top = 500,
                Left = 275,
                MinHeight = 75,
                MaxHeight = 175,
                MinWidth = 250,
                MaxWidth = 400
            };
            RecipesDetails = new PopupDetails
            {
                IsVisible = false,
                Top = 500,
                Left = 575,
                MinHeight = 75,
                MaxHeight = 175,
                MinWidth = 250,
                MaxWidth = 400
            };
            GameMessagesDetails = new PopupDetails
            {
                IsVisible = false,
                Top = 250,
                Left = 10,
                MinHeight = 75,
                MaxHeight = 175,
                MinWidth = 350,
                MaxWidth = 400
            };

            _messageBroker.OnMessageRaised += OnGameMessageRaised;
        }

        /// <summary>
        /// Moves the player to the location to the north.
        /// </summary>
        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
            }
        }

        /// <summary>
        /// Moves the player to the location to the east.
        /// </summary>
        public void MoveEast()
        {
            if (HasLocationToEast)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
            }
        }

        /// <summary>
        /// Moves the player to the location to the south.
        /// </summary>
        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
            }
        }

        /// <summary>
        /// Moves the player to the location to the west.
        /// </summary>
        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
            }
        }

        /// <summary>
        /// Populates the game details.
        /// </summary>
        private void PopulateGameDetails()
        {
            GameDetails = GameDetailsService.ReadGameDetails();
        }



        /// <summary>
        /// Handles the game message raised event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
        {
            if (GameMessages.Count > 250)
            {
                GameMessages.RemoveAt(0);
            }

            GameMessages.Add(e.Message);
        }

        /// <summary>
        /// Completes quests at the current location.
        /// </summary>
        private void CompleteQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus questToComplete = CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID && !q.IsCompleted);
                if (questToComplete != null)
                {
                    if (CurrentPlayer.Inventory.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        CurrentPlayer.RemoveItemsFromInventory(quest.ItemsToComplete);
                        _messageBroker.RaiseMessage("");
                        _messageBroker.RaiseMessage($"You completed the '{quest.Name}' quest");
                        // Give the player the quest rewards
                        _messageBroker.RaiseMessage($"You receive {quest.RewardExperiencePoints} experience points");
                        CurrentPlayer.AddExperience(quest.RewardExperiencePoints);
                        _messageBroker.RaiseMessage($"You receive {quest.RewardGold} gold");
                        CurrentPlayer.ReceiveGold(quest.RewardGold);
                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                            _messageBroker.RaiseMessage($"You receive a {rewardItem.Name}");
                            CurrentPlayer.AddItemToInventory(rewardItem);
                        }
                        // Mark the Quest as completed
                        questToComplete.IsCompleted = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gives the player quests at the current location.
        /// </summary>
        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
                    _messageBroker.RaiseMessage("");
                    _messageBroker.RaiseMessage($"You receive the '{quest.Name}' quest");
                    _messageBroker.RaiseMessage(quest.Description);
                    _messageBroker.RaiseMessage("Return with:");
                    foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                    {
                        _messageBroker.RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }
                    _messageBroker.RaiseMessage("And you will receive:");
                    _messageBroker.RaiseMessage($"   {quest.RewardExperiencePoints} experience points");
                    _messageBroker.RaiseMessage($"   {quest.RewardGold} gold");
                    foreach (ItemQuantity itemQuantity in quest.RewardItems)
                    {
                        _messageBroker.RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }
                }
            }
        }

        /// <summary>
        /// Attacks the current monster.
        /// </summary>
        public void AttackCurrentMonster()
        {
            _currentBattle?.AttackOpponent();
        }

        /// <summary>
        /// Uses the current consumable item.
        /// </summary>
        public void UseCurrentConsumable()
        {
            if (CurrentPlayer.CurrentConsumable != null)
            {
                if (_currentBattle == null)
                {
                    CurrentPlayer.OnActionPerformed += OnConsumableActionPerformed;
                }
                CurrentPlayer.UseCurrentConsumable();
                if (_currentBattle == null)
                {
                    CurrentPlayer.OnActionPerformed -= OnConsumableActionPerformed;
                }
            }
        }

        /// <summary>
        /// Handles the consumable action performed event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="result">The result of the action.</param>
        private void OnConsumableActionPerformed(object sender, string result)
        {
            _messageBroker.RaiseMessage(result);
        }

        /// <summary>
        /// Crafts an item using the specified recipe.
        /// </summary>
        /// <param name="recipe">The recipe to use.</param>
        public void CraftItemUsing(Recipe recipe)
        {
            if (CurrentPlayer.Inventory.HasAllTheseItems(recipe.Ingredients))
            {
                CurrentPlayer.RemoveItemsFromInventory(recipe.Ingredients);
                foreach (ItemQuantity itemQuantity in recipe.OutputItems)
                {
                    for (int i = 0; i < itemQuantity.Quantity; i++)
                    {
                        GameItem outputItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                        CurrentPlayer.AddItemToInventory(outputItem);
                        _messageBroker.RaiseMessage($"You craft 1 {outputItem.Name}");
                    }
                }
            }
            else
            {
                _messageBroker.RaiseMessage("You do not have the required ingredients:");
                foreach (ItemQuantity itemQuantity in recipe.Ingredients)
                {
                    _messageBroker.RaiseMessage($"  {itemQuantity.QuantityItemDescription}");
                }
            }
        }

        /// <summary>
        /// Handles the player killed event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnPlayerKilled(object sender, System.EventArgs e)
        {
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage("You have been killed.");
            CurrentLocation = CurrentWorld.LocationAt(0, -1);
            CurrentPlayer.CompletelyHeal();
        }

        /// <summary>
        /// Handles the current monster killed event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="eventArgs">The event arguments.</param>
        private void OnCurrentMonsterKilled(object sender, System.EventArgs eventArgs)
        {
            // Get another monster to fight
            CurrentMonster = MonsterFactory.GetMonsterFromLocation(CurrentLocation);
        }

        /// <summary>
        /// Handles the current player leveled up event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="eventArgs">The event arguments.</param>
        private void OnCurrentPlayerLeveledUp(object sender, System.EventArgs eventArgs)
        {
            _messageBroker.RaiseMessage($"You are now level {CurrentPlayer.Level}!");
        }



        //private void OnDialogChosen(object sender, System.EventArgs e)
        //{
        //    _messageBroker.RaiseMessage("");
        //    _messageBroker.RaiseMessage("You see a trader in front of you. He seems bored and begrudingly asks you");
        //   // _messageBroker.RaiseMessage("You have been killed.");
        //    //CurrentLocation = CurrentWorld.LocationAt(0, -1);
        //    CurrentPlayer.CompletelyHeal();
        //}


        /// <summary>
        /// Disposes of the game session, unsubscribing from events.
        /// </summary>
        /// 
        public void Dispose()
        {
            _currentBattle.Dispose();
            _messageBroker.OnMessageRaised -= OnGameMessageRaised;
        }
    }
}