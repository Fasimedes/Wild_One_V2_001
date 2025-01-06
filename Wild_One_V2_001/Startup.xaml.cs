using System;
using System.Windows;
using SOSCSRPG.Models;
using SOSCSRPG.Services;
using SOSCSRPG.ViewModels;
using Microsoft.Win32;
namespace WPFUI
{
    /// <summary>
    /// Interaction logic for Startup.xaml
    /// </summary>
    public partial class Startup : Window
    {
        // File extension for save game files
        private const string SAVE_GAME_FILE_EXTENSION = "soscsrpg";

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        public Startup()
        {
            InitializeComponent();
            DataContext = GameDetailsService.ReadGameDetails();
        }

        /// <summary>
        /// Handles the click event for starting a new game.
        /// Opens the character creation window and closes the startup window.
        /// </summary>
        private void StartNewGame_OnClick(object sender, RoutedEventArgs e)
        {
            CharacterCreation characterCreationWindow = new CharacterCreation();
            characterCreationWindow.Show();
            Close();
        }

        /// <summary>
        /// Handles the click event for loading a saved game.
        /// Opens a file dialog to select a saved game file, loads the game state, and opens the main window.
        /// </summary>
        private void LoadSavedGame_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = $"Saved games (*.{SAVE_GAME_FILE_EXTENSION})|*.{SAVE_GAME_FILE_EXTENSION}"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                GameState gameState = SaveGameService.LoadLastSaveOrCreateNew(openFileDialog.FileName);

                MainWindow mainWindow = new MainWindow(gameState.Player, gameState.XCoordinate, gameState.YCoordinate);
                mainWindow.Show();
                Close();
            }
        }

        /// <summary>
        /// Handles the click event for exiting the application.
        /// </summary>
        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}