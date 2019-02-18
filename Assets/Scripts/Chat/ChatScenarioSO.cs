using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Chat/ChatScenario")]
public class ChatScenarioSO : ScriptableObject
{
    [TextArea]
    public string chatHead;

    [TextArea]
    public string chatBody;


}