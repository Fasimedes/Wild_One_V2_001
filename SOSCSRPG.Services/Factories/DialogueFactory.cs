using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSCSRPG.Models;
using System.Xml;
using System.IO;
using SOSCSRPG.Models.Shared;

namespace SOSCSRPG.Services.Factories
{
    public static class DialogueFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\DialogueNodes.xml";
        private static readonly Dictionary<int, DialogueNode> _dialogueNodes = new Dictionary<int, DialogueNode>();

        static DialogueFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                LoadDialogueNodesFromNodes(data.SelectNodes("/DialogueNodes/DialogueNode"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        private static void LoadDialogueNodesFromNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                int id = node.AttributeAsInt("ID");
                string text = node.AttributeAsString("Text");
                DialogueNode dialogueNode = new DialogueNode(text);

                XmlNode choicesNode = node.SelectSingleNode("./Choices");
                if (choicesNode != null)
                {
                    AddChoices(dialogueNode, choicesNode);
                }

                _dialogueNodes[id] = dialogueNode;
            }
        }

        private static void AddChoices(DialogueNode parentNode, XmlNode choicesNode)
        {
            foreach (XmlNode choiceNode in choicesNode.SelectNodes("./Choice"))
            {
                string choiceText = choiceNode.AttributeAsString("Text");
                DialogueNode choiceDialogueNode = new DialogueNode(choiceText);

                XmlNode subChoicesNode = choiceNode.SelectSingleNode("./Choices");
                if (subChoicesNode != null)
                {
                    AddChoices(choiceDialogueNode, subChoicesNode);
                }

                parentNode.AddChoice(choiceDialogueNode);
            }
        }

        public static DialogueNode GetDialogueNodeByID(int id)
        {
            return _dialogueNodes.TryGetValue(id, out DialogueNode dialogueNode) ? dialogueNode : null;
        }
    }
}
