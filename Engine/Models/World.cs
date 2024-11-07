using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class World
    {
        private List<Location> _locations = new List<Location>(); // A list to store locations in the world

        internal void AddLocation(int xCoordinate, int yCoordinate, string name, string description, string imageName) // Creates a new location
        {
            Location loc = new Location(); // Instantiate a new Location object
            loc.XCoordinate = xCoordinate; // Set the X coordinate of the location
            loc.YCoordinate = yCoordinate; // Set the Y coordinate of the location
            loc.Name = name; // Set the name of the location
            loc.Description = description; // Set the description of the location
            loc.ImageName = $"E:\\Vše možné\\C#, Sql courses\\C#\\Semestralka\\Wild_One_V2_001\\Engine\\Images\\Locations\\{imageName}"; // Set the image reference for the location

            _locations.Add(loc); // Add the location to the list
        }

        public Location LocationAt(int xCoordinate, int yCoordinate) // Checks the current location
        {
            foreach (Location loc in _locations) // Iterate through all locations
            {
                if (loc.XCoordinate == xCoordinate && loc.YCoordinate == yCoordinate) // Check if coordinates match
                {
                    return loc; // Return the found location
                }
            }
            return null; // Return null if no matching location is found
        }
    }
}
