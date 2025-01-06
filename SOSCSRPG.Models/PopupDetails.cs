using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing the details of a popup window in the game.
    /// </summary>
    public class PopupDetails : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets a value indicating whether the popup is visible.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the top position of the popup.
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Gets or sets the left position of the popup.
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// Gets or sets the minimum height of the popup.
        /// </summary>
        public int MinHeight { get; set; }

        /// <summary>
        /// Gets or sets the maximum height of the popup.
        /// </summary>
        public int MaxHeight { get; set; }

        /// <summary>
        /// Gets or sets the minimum width of the popup.
        /// </summary>
        public int MinWidth { get; set; }

        /// <summary>
        /// Gets or sets the maximum width of the popup.
        /// </summary>
        public int MaxWidth { get; set; }

        /// <summary>
        /// Event that is raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
