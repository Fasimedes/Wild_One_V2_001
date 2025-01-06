using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Models
{
    /// <summary>
    /// Class representing a dialogue node in a conversation.
    /// </summary>
    public class DialogueNode
    {
        /// <summary>
        /// Gets or sets the text of the dialogue node.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the list of choices available from this dialogue node.
        /// </summary>
        public List<DialogueNode> Choices { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogueNode"/> class with the specified text.
        /// </summary>
        /// <param name="text">The text of the dialogue node.</param>
        public DialogueNode(string text)
        {
            Text = text;
            Choices = new List<DialogueNode>();
        }

        /// <summary>
        /// Adds a choice to the list of choices for this dialogue node.
        /// </summary>
        /// <param name="choice">The dialogue node representing the choice.</param>
        public void AddChoice(DialogueNode choice)
        {
            Choices.Add(choice);
        }
    }
}
