using SOSCSRPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models.Actions
{
    /// <summary>
    /// Abstract base class for actions that can be performed with a game item.
    /// </summary>
    public abstract class BaseAction
    {
        // The game item being used to perform the action
        protected readonly GameItem _itemInUse;

        /// <summary>
        /// Event that is raised when an action is performed.
        /// </summary>
        public event EventHandler<string> OnActionPerformed;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAction"/> class.
        /// </summary>
        /// <param name="itemInUse">The game item being used to perform the action.</param>
        protected BaseAction(GameItem itemInUse)
        {
            _itemInUse = itemInUse;
        }

        /// <summary>
        /// Reports the result of the action by raising the <see cref="OnActionPerformed"/> event.
        /// </summary>
        /// <param name="result">The result of the action.</param>
        protected void ReportResult(string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
    }
}
