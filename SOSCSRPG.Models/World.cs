using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing the game world, containing locations.
    /// </summary>
    public class World
    {
        // List to hold the locations in the world
        private readonly List<Location> _locations = new List<Location>();

        /// <summary>
        /// Adds a location to the world.
        /// </summary>
        /// <param name="location">The location to add.</param>
        public void AddLocation(Location location)
        {
            _locations.Add(location);
        }

        /// <summary>
        /// Gets the location at the specified coordinates.
        /// </summary>
        /// <param name="xCoordinate">The X coordinate of the location.</param>
        /// <param name="yCoordinate">The Y coordinate of the location.</param>
        /// <returns>The location at the specified coordinates, or null if no location exists there.</returns>
        public Location LocationAt(int xCoordinate, int yCoordinate)
        {
            foreach (Location loc in _locations)
            {
                if (loc.XCoordinate == xCoordinate && loc.YCoordinate == yCoordinate)
                {
                    return loc;
                }
            }
            return null;
        }
    }
}
