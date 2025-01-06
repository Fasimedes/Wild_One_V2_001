using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;
namespace SOSCSRPG.Models
{
    /// <summary>
    /// Abstract base class representing a living entity in the game.
    /// </summary>
    public abstract class LivingEntity : INotifyPropertyChanged
    {
        #region Properties

        // Backing fields for current weapon and consumable
        private GameItem _currentWeapon;
        private GameItem _currentConsumable;

        /// <summary>
        /// Event that is raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the collection of player attributes.
        /// </summary>
        public ObservableCollection<PlayerAttribute> Attributes { get; } = new ObservableCollection<PlayerAttribute>();

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the current hit points of the entity.
        /// </summary>
        public int CurrentHitPoints { get; private set; }

        /// <summary>
        /// Gets or sets the maximum hit points of the entity.
        /// </summary>
        public int MaximumHitPoints { get; protected set; }

        /// <summary>
        /// Gets or sets the gold amount of the entity.
        /// </summary>
        public int Gold { get; private set; }

        /// <summary>
        /// Gets or sets the level of the entity.
        /// </summary>
        public int Level { get; protected set; }

        /// <summary>
        /// Gets or sets the inventory of the entity.
        /// </summary>
        public Inventory Inventory { get; private set; }

        /// <summary>
        /// Gets or sets the current weapon of the entity.
        /// </summary>
        public GameItem CurrentWeapon
        {
            get => _currentWeapon;
            set
            {
                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }
                _currentWeapon = value;
                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }
            }
        }

        /// <summary>
        /// Gets or sets the current consumable of the entity.
        /// </summary>
        public GameItem CurrentConsumable
        {
            get => _currentConsumable;
            set
            {
                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }
                _currentConsumable = value;
                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }
            }
        }

        /// <summary>
        /// Gets the hit points as a string in the format "CurrentHitPoints/MaximumHitPoints".
        /// </summary>
        [JsonIgnore]
        public string HitPoints => $"{CurrentHitPoints}/{MaximumHitPoints}";

        /// <summary>
        /// Gets a value indicating whether the entity is alive.
        /// </summary>
        [JsonIgnore]
        public bool IsAlive => CurrentHitPoints > 0;

        /// <summary>
        /// Gets a value indicating whether the entity is dead.
        /// </summary>
        [JsonIgnore]
        public bool IsDead => !IsAlive;

        #endregion

        /// <summary>
        /// Event that is raised when an action is performed.
        /// </summary>
        public event EventHandler<string> OnActionPerformed;

        /// <summary>
        /// Event that is raised when the entity is killed.
        /// </summary>
        public event EventHandler OnKilled;

        /// <summary>
        /// Initializes a new instance of the <see cref="LivingEntity"/> class with the specified properties.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        /// <param name="maximumHitPoints">The maximum hit points of the entity.</param>
        /// <param name="currentHitPoints">The current hit points of the entity.</param>
        /// <param name="attributes">The attributes of the entity.</param>
        /// <param name="gold">The gold amount of the entity.</param>
        /// <param name="level">The level of the entity.</param>
        protected LivingEntity(string name, int maximumHitPoints, int currentHitPoints,
                               IEnumerable<PlayerAttribute> attributes, int gold, int level = 1)
        {
            Name = name;
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            Gold = gold;
            Level = level;

            foreach (PlayerAttribute attribute in attributes)
            {
                Attributes.Add(attribute);
            }

            Inventory = new Inventory();
        }

        /// <summary>
        /// Uses the current weapon on the specified target.
        /// </summary>
        /// <param name="target">The target entity.</param>
        public void UseCurrentWeaponOn(LivingEntity target)
        {
            CurrentWeapon.PerformAction(this, target);
        }

        /// <summary>
        /// Uses the current consumable.
        /// </summary>
        public void UseCurrentConsumable()
        {
            CurrentConsumable.PerformAction(this, this);
            RemoveItemFromInventory(CurrentConsumable);
        }

        /// <summary>
        /// Takes damage and reduces the current hit points.
        /// </summary>
        /// <param name="hitPointsOfDamage">The amount of damage to take.</param>
        public void TakeDamage(int hitPointsOfDamage)
        {
            CurrentHitPoints -= hitPointsOfDamage;

            if (IsDead)
            {
                CurrentHitPoints = 0;
                RaiseOnKilledEvent();
            }
        }

        /// <summary>
        /// Heals the entity by the specified amount of hit points.
        /// </summary>
        /// <param name="hitPointsToHeal">The amount of hit points to heal.</param>
        public void Heal(int hitPointsToHeal)
        {
            CurrentHitPoints += hitPointsToHeal;

            if (CurrentHitPoints > MaximumHitPoints)
            {
                CurrentHitPoints = MaximumHitPoints;
            }
        }

        /// <summary>
        /// Completely heals the entity to its maximum hit points.
        /// </summary>
        public void CompletelyHeal()
        {
            CurrentHitPoints = MaximumHitPoints;
        }

        /// <summary>
        /// Receives gold and adds it to the entity's gold amount.
        /// </summary>
        /// <param name="amountOfGold">The amount of gold to receive.</param>
        public void ReceiveGold(int amountOfGold)
        {
            Gold += amountOfGold;
        }

        /// <summary>
        /// Spends gold and reduces the entity's gold amount.
        /// </summary>
        /// <param name="amountOfGold">The amount of gold to spend.</param>
        public void SpendGold(int amountOfGold)
        {
            if (amountOfGold > Gold)
            {
                throw new ArgumentOutOfRangeException($"{Name} only has {Gold} gold, and cannot spend {amountOfGold} gold");
            }

            Gold -= amountOfGold;
        }

        /// <summary>
        /// Adds an item to the entity's inventory.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddItemToInventory(GameItem item)
        {
            Inventory = Inventory.AddItem(item);
        }

        /// <summary>
        /// Removes an item from the entity's inventory.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public void RemoveItemFromInventory(GameItem item)
        {
            Inventory = Inventory.RemoveItem(item);
        }

        /// <summary>
        /// Removes multiple items from the entity's inventory based on item quantities.
        /// </summary>
        /// <param name="itemQuantities">The item quantities to remove.</param>
        public void RemoveItemsFromInventory(IEnumerable<ItemQuantity> itemQuantities)
        {
            Inventory = Inventory.RemoveItems(itemQuantities);
        }

        #region Private functions

        /// <summary>
        /// Raises the OnKilled event.
        /// </summary>
        private void RaiseOnKilledEvent()
        {
            OnKilled?.Invoke(this, new System.EventArgs());
        }

        /// <summary>
        /// Raises the OnActionPerformed event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="result">The result of the action.</param>
        private void RaiseActionPerformedEvent(object sender, string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }

        #endregion
    }
}