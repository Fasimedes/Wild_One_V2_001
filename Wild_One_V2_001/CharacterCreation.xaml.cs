using System.Windows;
using System.Windows.Controls;
using SOSCSRPG.ViewModels;


namespace WPFUI
{
    /// <summary>
    /// Interaction logic for CharacterCreation.xaml
    /// </summary>
    public partial class CharacterCreation : Window
    {
        // ViewModel for character creation
        private CharacterCreationViewModel VM { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterCreation"/> class.
        /// </summary>
        public CharacterCreation()
        {
            InitializeComponent();
            VM = new CharacterCreationViewModel();
            DataContext = VM;
        }

        /// <summary>
        /// Handles the click event for the "Random Player" button.
        /// Rolls a new character with random attributes.
        /// </summary>
        private void RandomPlayer_OnClick(object sender, RoutedEventArgs e)
        {
            VM.RollNewCharacter();
        }

        /// <summary>
        /// Handles the click event for the "Use This Player" button.
        /// Creates a new main window with the selected player and closes the character creation window.
        /// </summary>
        private void UseThisPlayer_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(VM.GetPlayer());
            mainWindow.Show();
            Close();
        }

        /// <summary>
        /// Handles the selection changed event for the race selection.
        /// Applies the attribute modifiers based on the selected race.
        /// </summary>
        private void Race_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VM.ApplyAttributeModifiers();
        }
    }
}