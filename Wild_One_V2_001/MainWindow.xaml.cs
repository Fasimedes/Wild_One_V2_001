using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SOSCSRPG.Models;
using SOSCSRPG.Services;
using SOSCSRPG.ViewModels;
using Microsoft.Win32;
using WPFUI.Windows;
namespace WPFUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // File extension for save game files
        private const string SAVE_GAME_FILE_EXTENSION = "soscsrpg";

        // Dictionary to map user input actions to keys
        private readonly Dictionary<Key, Action> _userInputActions = new Dictionary<Key, Action>();

        // Current game session
        private GameSession _gameSession;

        // Starting point for drag operations
        private Point? _dragStart;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class with the specified player and coordinates.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="xLocation">The X coordinate of the player's starting location.</param>
        /// <param name="yLocation">The Y coordinate of the player's starting location.</param>
        public MainWindow(Player player, int xLocation = 0, int yLocation = 0)
        {
            InitializeComponent();
            InitializeUserInputActions();
            SetActiveGameSessionTo(new GameSession(player, xLocation, yLocation));

            // Enable drag for popup details canvases
            foreach (UIElement element in GameCanvas.Children)
            {
                if (element is Canvas)
                {
                    element.MouseDown += GameCanvas_OnMouseDown;
                    element.MouseMove += GameCanvas_OnMouseMove;
                    element.MouseUp += GameCanvas_OnMouseUp;
                }
            }
        }

        public void DisplayCurrentDialogue(DialogueNode currentNode)
        {
            // Display the current dialogue text
            _gameSession.GameMessages.Add(currentNode.Text);

            if (currentNode.Choices.Count > 0)
            {
                // Display the choices
                for (int i = 0; i < currentNode.Choices.Count; i++)
                {
                    _gameSession.GameMessages.Add($"{i + 1}. {currentNode.Choices[i].Text}");
                }

                // Create a TextBox for user input
                TextBox inputTextBox = new TextBox();
                inputTextBox.KeyDown += (sender, e) =>
                {
                    if (e.Key == Key.Enter)
                    {
                        // Validate the input
                        if (int.TryParse(inputTextBox.Text, out int choice) && choice > 0 && choice <= currentNode.Choices.Count)
                        {
                            // Display the next dialogue node based on the user's choice
                            DisplayCurrentDialogue(currentNode.Choices[choice - 1]);
                        }
                        else
                        {
                            // Display an error message for invalid input
                            _gameSession.GameMessages.Add("Invalid choice. Please enter a valid number.");
                        }
                    }
                };

                // Add the TextBox to the UI (assuming you have a method to add it to the UI)
                //AddInputTextBoxToUI(inputTextBox);
            }
        }

        private void OnClickDialogOption_1(object sender, RoutedEventArgs e)
        {
            if (_gameSession.CurrentDialogueNode != null && _gameSession.CurrentDialogueNode.Choices.Count > 0)
            {
                _gameSession.CurrentDialogueNode = _gameSession.CurrentDialogueNode.Choices[0];
            }
        }

        private void OnClickDialogOption_2(object sender, RoutedEventArgs e)
        {
            if (_gameSession.CurrentDialogueNode != null && _gameSession.CurrentDialogueNode.Choices.Count > 1)
            {
                _gameSession.CurrentDialogueNode = _gameSession.CurrentDialogueNode.Choices[1];
            }
        }


        // Event handler for moving north
        private void OnClick_MoveNorth(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveNorth();
        }

        // Event handler for moving west
        private void OnClick_MoveWest(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveWest();
        }

        // Event handler for moving east
        private void OnClick_MoveEast(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveEast();
        }

        // Event handler for moving south
        private void OnClick_MoveSouth(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveSouth();
        }

        // Event handler for attacking the current monster
        private void OnClick_AttackMonster(object sender, RoutedEventArgs e)
        {
            _gameSession.AttackCurrentMonster();
        }

        // Event handler for using the current consumable item
        private void OnClick_UseCurrentConsumable(object sender, RoutedEventArgs e)
        {
            _gameSession.UseCurrentConsumable();
        }

        // Event handler for displaying the trade screen
        private void OnClick_DisplayTradeScreen(object sender, RoutedEventArgs e)
        {
            if (_gameSession.CurrentTrader != null)
            {
                TradeScreen tradeScreen = new TradeScreen
                {
                    Owner = this,
                    DataContext = _gameSession
                };
                tradeScreen.ShowDialog();
            }
        }

        // Event handler for crafting an item using a recipe
        private void OnClick_Craft(object sender, RoutedEventArgs e)
        {
            Recipe recipe = ((FrameworkElement)sender).DataContext as Recipe;
            _gameSession.CraftItemUsing(recipe);
        }


        /// <summary>
        /// Event handler for handling the dialog options.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void OnClickDialogOption_1(object sender, RoutedEventArgs e)
        //{
        //   // _gameSession.GameMessages.Add("You nod to the trader");

        //    DisplayCurrentDialogue(_gameSession.RootDialogueNode);
        //}
        //private void OnClickDialogOption_2(object sender, RoutedEventArgs e)
        //{
        //    //_gameSession.GameMessages.Add("You tell trader to fuck off");
        //    DisplayCurrentDialogue(_gameSession.RootDialogueNode);

        //}


        // Initialize user input actions for keyboard shortcuts
        private void InitializeUserInputActions()
        {
            _userInputActions.Add(Key.W, () => _gameSession.MoveNorth());
            _userInputActions.Add(Key.A, () => _gameSession.MoveWest());
            _userInputActions.Add(Key.S, () => _gameSession.MoveSouth());
            _userInputActions.Add(Key.D, () => _gameSession.MoveEast());
            _userInputActions.Add(Key.Z, () => _gameSession.AttackCurrentMonster());
            _userInputActions.Add(Key.C, () => _gameSession.UseCurrentConsumable());
            _userInputActions.Add(Key.P, () => _gameSession.PlayerDetails.IsVisible = !_gameSession.PlayerDetails.IsVisible);
            _userInputActions.Add(Key.I, () => _gameSession.InventoryDetails.IsVisible = !_gameSession.InventoryDetails.IsVisible);
            _userInputActions.Add(Key.Q, () => _gameSession.QuestDetails.IsVisible = !_gameSession.QuestDetails.IsVisible);
            _userInputActions.Add(Key.R, () => _gameSession.RecipesDetails.IsVisible = !_gameSession.RecipesDetails.IsVisible);
            _userInputActions.Add(Key.M, () => _gameSession.GameMessagesDetails.IsVisible = !_gameSession.RecipesDetails.IsVisible);
            _userInputActions.Add(Key.T, () => OnClick_DisplayTradeScreen(this, new RoutedEventArgs()));
        }

        // Event handler for key down events
        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_userInputActions.ContainsKey(e.Key))
            {
                _userInputActions[e.Key].Invoke();
                e.Handled = true;
            }
        }

        // Set the active game session and update the DataContext
        private void SetActiveGameSessionTo(GameSession gameSession)
        {
            if (_gameSession != null)
            {
                _gameSession.GameMessages.CollectionChanged -= GameMessages_CollectionChanged;
            }

            _gameSession = gameSession;
            DataContext = _gameSession;

            _gameSession.GameMessages.CollectionChanged += GameMessages_CollectionChanged;
        }

        // Event handler for game messages collection changes
        private void GameMessages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            (GameMessagesFlowDocumentScrollViewer
                    .Template
                    .FindName("PART_ContentHost", GameMessagesFlowDocumentScrollViewer) as ScrollViewer)
                ?.ScrollToEnd();
        }

        // Event handler for starting a new game
        private void StartNewGame_OnClick(object sender, RoutedEventArgs e)
        {
            _gameSession?.Dispose();
            Startup startup = new Startup();
            startup.Show();
            Close();
        }

        // Event handler for saving the game
        private void SaveGame_OnClick(object sender, RoutedEventArgs e)
        {
            SaveGame();
        }

        // Event handler for exiting the application
        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Event handler for the window closing event
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            AskToSaveGame();
        }

        // Ask the user if they want to save the game before closing
        private void AskToSaveGame()
        {
            YesNoWindow message = new YesNoWindow("Save Game", "Do you want to save your game?");
            message.Owner = GetWindow(this);
            message.ShowDialog();
            if (message.ClickedYes)
            {
                SaveGame();
            }
        }

        // Save the game to a file
        private void SaveGame()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = $"Saved games (*.{SAVE_GAME_FILE_EXTENSION})|*.{SAVE_GAME_FILE_EXTENSION}"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                SaveGameService.Save(new GameState(_gameSession.CurrentPlayer,
                    _gameSession.CurrentLocation.XCoordinate,
                    _gameSession.CurrentLocation.YCoordinate), saveFileDialog.FileName);
            }
        }

        // Event handler for closing the player details window
        private void ClosePlayerDetailsWindow_OnClick(object sender, RoutedEventArgs e)
        {
            _gameSession.PlayerDetails.IsVisible = false;
        }

        // Event handler for closing the inventory window
        private void CloseInventoryWindow_OnClick(object sender, RoutedEventArgs e)
        {
            _gameSession.InventoryDetails.IsVisible = false;
        }

        // Event handler for closing the quests window
        private void CloseQuestsWindow_OnClick(object sender, RoutedEventArgs e)
        {
            _gameSession.QuestDetails.IsVisible = false;
        }

        // Event handler for closing the recipes window
        private void CloseRecipesWindow_OnClick(object sender, RoutedEventArgs e)
        {
            _gameSession.RecipesDetails.IsVisible = false;
        }

        // Event handler for closing the game messages details window
        private void CloseGameMessagesDetailsWindow_OnClick(object sender, RoutedEventArgs e)
        {
            _gameSession.GameMessagesDetails.IsVisible = false;
        }

        // Event handler for mouse down event on the game canvas
        private void GameCanvas_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }
            UIElement movingElement = (UIElement)sender;
            _dragStart = e.GetPosition(movingElement);
            movingElement.CaptureMouse();
            e.Handled = true;
        }

        // Event handler for mouse move event on the game canvas
        private void GameCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_dragStart == null || e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }
            Point mousePosition = e.GetPosition(GameCanvas);
            UIElement movingElement = (UIElement)sender;

            // Don't let player move popup details off the board
            if (mousePosition.X < _dragStart.Value.X ||
                mousePosition.Y < _dragStart.Value.Y ||
                mousePosition.X > GameCanvas.ActualWidth - ((Canvas)movingElement).ActualWidth + _dragStart.Value.X ||
                mousePosition.Y > GameCanvas.ActualHeight - ((Canvas)movingElement).ActualHeight + _dragStart.Value.Y)
            {
                return;
            }

            Canvas.SetLeft(movingElement, mousePosition.X - _dragStart.Value.X);
            Canvas.SetTop(movingElement, mousePosition.Y - _dragStart.Value.Y);
            e.Handled = true;
        }

        // Event handler for mouse up event on the game canvas
        private void GameCanvas_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var movingElement = (UIElement)sender;
            movingElement.ReleaseMouseCapture();
            _dragStart = null;
            e.Handled = true;
        }
        //private void AddInputTextBoxToUI(TextBox inputTextBox)
        //{
        //    // Assuming you have a panel or container to add the TextBox to
        //    InputPanel.Children.Clear(); // Clear any previous input boxes
        //    InputPanel.Children.Add(inputTextBox);
        //    inputTextBox.Focus(); // Set focus to the input box
        //}
    }
}