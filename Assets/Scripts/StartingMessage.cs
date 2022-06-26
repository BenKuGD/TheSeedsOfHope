using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartingMessage : MonoBehaviour
{
    public GameObject startingMessage;
    public TextMeshProUGUI messageText;
    public string[] messages;

    private int _messagesNumber;
    private int _curMessageIndex;

    private void Start()
    {
        if (GameManager.instance.startPosVar == StartingPositions.BackToBedroom)
            startingMessage.gameObject.SetActive(false);
        else
        {
            GameManager.instance.playerIsBusy = true;
            _curMessageIndex = 0;
            _messagesNumber = messages.Length;
            messageText.text = messages[_curMessageIndex];
        }
    }

    public void NextMessage()
    {
        _curMessageIndex++;

        if (_curMessageIndex < _messagesNumber)
            messageText.text = messages[_curMessageIndex];
        else
            DisableStartingMessage();
    }

    private void DisableStartingMessage()
    {
        startingMessage.gameObject.SetActive(false);
        GameManager.instance.playerIsBusy = false;
    }
}
