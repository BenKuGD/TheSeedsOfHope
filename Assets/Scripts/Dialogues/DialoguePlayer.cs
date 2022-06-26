using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialoguePlayer : MonoBehaviour
{
    public DialogueData1 curDialogueData;
    public TextMeshProUGUI dialogueText;
    public Image dialogueActor;
    public GameObject dialogueWindow;
    public Button dialogueButton;

    private int _dialogueDataLength;
    private int _curDialogueLine;

    public bool isDialogueEnabled = true;
    public bool isAnEquipmentInterface = false;

    private void Start()
    {
        _dialogueDataLength = curDialogueData.dialogueLines.Length;
        dialogueText.text = curDialogueData.dialogueLines[0];
        dialogueActor.sprite = GetDialoguePortrait(curDialogueData.actorsIndices[0]);
        _curDialogueLine = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isDialogueEnabled)
            InitializeDialogue();
    }

    private void OnTriggerExit(Collider other)
    {
        _dialogueDataLength = curDialogueData.dialogueLines.Length;
        dialogueText.text = string.Empty;
        dialogueActor.sprite = GetDialoguePortrait(curDialogueData.actorsIndices[0]);
        _curDialogueLine = 0;

        if (this.CompareTag("Cutscene"))
            Destroy(this.gameObject);
    }

    // method for getting a dialogue portrait
    private Sprite GetDialoguePortrait(int actorIndex)
    {
        Sprite actorSprite = dialogueActor.sprite;

        switch(actorIndex)
        {
            case 0:
                actorSprite = Resources.Load<Sprite>("Barbara");
                break;
            case 1:
                actorSprite = Resources.Load<Sprite>("Merystem");
                break;
            case 2:
                actorSprite = Resources.Load<Sprite>("Barbara Sad");
                break;
            case 3:
                actorSprite = Resources.Load<Sprite>("Barbara Smile");
                break;
        }

        return actorSprite;
    }

    // method for going over dialogue lines
    public void NextDialogueLine()
    {
        if(_dialogueDataLength == 1 && !isAnEquipmentInterface)
        {
            dialogueWindow.SetActive(false);
            GameManager.instance.playerIsBusy = false;
            dialogueButton.onClick.RemoveAllListeners();
            _curDialogueLine = 0;
        }

        else if(_dialogueDataLength == 1 && isAnEquipmentInterface)
        {
            GameManager.instance.isDialogueBlockingTheStudy = false;
            dialogueWindow.SetActive(false);
            dialogueButton.onClick.RemoveAllListeners();
            _curDialogueLine = 0;
        }

        else
        {
            _curDialogueLine++;

            if (_curDialogueLine < _dialogueDataLength)
            {
                dialogueText.text = curDialogueData.dialogueLines[_curDialogueLine];
                dialogueActor.sprite = GetDialoguePortrait(curDialogueData.actorsIndices[_curDialogueLine]);
            }

            else if(isAnEquipmentInterface)
            {
                GameManager.instance.isDialogueBlockingTheStudy = false;
                dialogueWindow.SetActive(false);
                dialogueButton.onClick.RemoveAllListeners();
                _curDialogueLine = 0;
            }

            else 
            {
                dialogueWindow.SetActive(false);
                GameManager.instance.playerIsBusy = false;
                dialogueButton.onClick.RemoveAllListeners();
                _curDialogueLine = 0;
            }
        }
    }

    public void InitializeDialogue()
    {
        _dialogueDataLength = curDialogueData.dialogueLines.Length;
        dialogueText.text = curDialogueData.dialogueLines[0];
        dialogueActor.sprite = GetDialoguePortrait(curDialogueData.actorsIndices[0]);
        dialogueButton.onClick.AddListener(() => NextDialogueLine());
        dialogueWindow.SetActive(true);
        GameManager.instance.playerIsBusy = true;
    }

    public void BlockTheInterface(bool isAnInterface)
    {
        if (isAnInterface)
            GameManager.instance.isDialogueBlockingTheStudy = true;
        else
            return;
    }
}
