using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Dialogue
{
    [NodeTint("#CCFFCC")]
    public class Chat : DialogueBaseNode
    {
        [Input] public Connection input;
        [Output] public Connection output;
        public List<ChatMessage> chatMessages = new List<ChatMessage>();

        public bool pickRandomAnswer;
        public List<Answer> answers = new List<Answer>();

        [System.Serializable]
        public class Answer
        {
            public string text;
            public Color color;
        }

        [System.Serializable]
        public class ChatMessage
        {
            public string text;
            public bool isPlayer;
            public float delay;
        }

        public void AnswerQuestion(int index)
        {
            NodePort port = null;
            if (answers.Count == 0)
            {
                port = GetOutputPort("output");
            }
            else
            {
                if (answers.Count <= index) return;
                port = GetOutputPort("answers " + index);
            }

            if (port == null)
            {
                GameManager.chatManager.End(false);
            }
            for (int i = 0; i < port.ConnectionCount; i++)
            {
                NodePort connection = port.GetConnection(i);
                (connection.node as DialogueBaseNode).Trigger();
            }
        }

        public override void Trigger()
        {
            (graph as DialogueGraph).current = this;
        }
    }
}