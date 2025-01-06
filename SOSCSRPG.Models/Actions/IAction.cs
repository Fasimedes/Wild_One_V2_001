using SOSCSRPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models.Actions
{
    /// <summary>
    /// Interface for actions that can be performed by a living entity.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// Event that is raised when an action is performed.
        /// </summary>
        event EventHandler<string> OnActionPerformed;

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="actor">The entity performing the action.</param>
        /// <param name="target">The entity being targeted by the action.</param>
        void Execute(LivingEntity actor, LivingEntity target);
    }
}
