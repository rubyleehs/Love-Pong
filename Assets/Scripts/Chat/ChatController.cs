using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatController : MonoBehaviour
{
    [HideInInspector]
    public ChatNavigation chatNavigation;

    private void Awake()
    {
        if (chatNavigation == null) chatNavigation = GetComponent<ChatNavigation>();
    }

    public void DisplayChat()
    {
        
    }
}
