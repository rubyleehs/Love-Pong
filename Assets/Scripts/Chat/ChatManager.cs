using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue//the thing is looping non stop coz the ball is always behind enemy paddle and it keeps calling
{
    public class ChatManager : MonoBehaviour
    {
        public GameObject speechBubbleGO;
        public Transform speechParent;

        public DialogueGraph[] dialogList;
        public static DialogueGraph dialog;

        private static IEnumerator currentlyPlaying;
        [HideInInspector]
        public List<SpeechBubble> speechBubbles;
        public bool isExpectingAnswer;

        void Start()
        {
            SelectDialog(Random.Range(0, dialogList.Length));
            Play(dialog.current);
        }

        public void Play(Chat chat)
        {
            if(currentlyPlaying != null) StopCoroutine(currentlyPlaying);
            currentlyPlaying = _Play(chat);
            StartCoroutine(currentlyPlaying);
        }

        private IEnumerator _Play(Chat chat)
        {
            Debug.Log(chat);
            for (int i = 0; i < chat.chatMessages.Count; i++)
            {
                var msg = chat.chatMessages[i];
                yield return new WaitForSeconds(msg.delay);
                Say(msg.isPlayer, msg.text);
            }

            SummonQuestion();
        }

        public void SelectDialog(int index)
        {
            dialog = dialogList[index];
            dialog.Restart();
        }

        public void Answer(int index)
        {
            dialog.AnswerQuestion(index);
            Play(dialog.current);
        }

        private void SummonQuestion()
        {
            isExpectingAnswer = true;
            string reply = "";
            for (int i = 0; i < dialog.current.answers.Count; i++)
            {
                GameManager.pongManager.SpawnBall(i, dialog.current.answers[i].text, dialog.current.answers[i].color);
                reply += " | " + i + " : " + dialog.current.answers[i].text + "/n";
            }
            Debug.Log(reply);
        }

        private void Say(bool isPlayer, string text)
        {
            SpeechBubble s = Instantiate(speechBubbleGO,Vector3.zero,Quaternion.Euler(180,0,0),speechParent).GetComponent<SpeechBubble>();
            s.transform.SetAsFirstSibling();
            s.SetCharacter(isPlayer);
            s.SetText(text);
        }
    }
}
