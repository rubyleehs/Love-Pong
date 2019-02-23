using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue//the thing is looping non stop coz the ball is always behind enemy paddle and it keeps calling
{
    public class ChatManager : MonoBehaviour
    {
        public GameObject speechBubbleGO;
        public GameObject notificationGO;
        public Transform speechParent;

        public DialogueGraph[] dialogList;
        public static DialogueGraph dialog;

        public float minChatBubbleDistToTop;

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
            Debug.Log(index);
            dialog.AnswerQuestion(index);
            Play(dialog.current);
        }

        private void SummonQuestion()
        {
            if (dialog.current.name == "End")
            {
                End(false);
                return;
            }
            else if (dialog.current.name == "Victory")
            {
                End(true);
                return;
            }
            if (dialog.current.pickRandomAnswer || dialog.current.answers == null || dialog.current.answers.Count <= 1)
            {
                int randIndex = Random.Range(0, dialog.current.answers.Count);
                Answer(randIndex);
                
                return;
            }
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
            Transform temp = speechParent.GetChild(speechParent.childCount - 1).transform;
            Debug.Log((temp.position.y + temp.localScale.y)+ " || " + (MainCamera.topRight.y - minChatBubbleDistToTop));
            if (temp.position.y + temp.localScale.y > MainCamera.topRight.y - minChatBubbleDistToTop) 
            {
                Destroy(temp.gameObject);
            }
        }

        public void End(bool isSucess)
        {
            Debug.Log("End_Reached");
        }
    }
}
