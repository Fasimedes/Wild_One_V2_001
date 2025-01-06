using SOSCSRPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Services
{
    /// <summary>
    /// Class for managing dialogue in the game.
    /// </summary>
    internal class DialogueManager
    {
        // The current dialogue node being displayed
        private DialogueNode _currentNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogueManager"/> class with the specified root dialogue node.
        /// </summary>
        /// <param name="rootNode">The root dialogue node.</param>
        public DialogueManager(DialogueNode rootNode)
        {
            _currentNode = rootNode;
        }

        /// <summary>
        /// Displays the current dialogue and handles user input for choices.
        /// </summary>
        public void DisplayCurrentDialogue()
        {
            Console.WriteLine(_currentNode.Text);

            if (_currentNode.Choices.Count > 0)
            {
                for (int i = 0; i < _currentNode.Choices.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_currentNode.Choices[i].Text}");
                }

                int choice = int.Parse(Console.ReadLine()) - 1;
                _currentNode = _currentNode.Choices[choice];
            }
        }
    }
}
