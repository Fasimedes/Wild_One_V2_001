namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing the state of the game.
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Gets the player in the game state.
        /// </summary>
        public Player Player { get; init; }

        /// <summary>
        /// Gets the X coordinate of the player's location.
        /// </summary>
        public int XCoordinate { get; init; }

        /// <summary>
        /// Gets the Y coordinate of the player's location.
        /// </summary>
        public int YCoordinate { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameState"/> class with the specified player and coordinates.
        /// </summary>
        /// <param name="player">The player in the game state.</param>
        /// <param name="xCoordinate">The X coordinate of the player's location.</param>
        /// <param name="yCoordinate">The Y coordinate of the player's location.</param>
        public GameState(Player player, int xCoordinate, int yCoordinate)
        {
            Player = player;
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
        }
    }
}