using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Equipment))]
public class EquipmentDialogueController : MonoBehaviour
{
    private Equipment _equipmentComponent;
    private DialoguePlayer _dialoguePlayerComponent;

    private enum EquipmentStates
    {
        SampleNotAcquired, SampleAcquired
    }

    private EquipmentStates _equipmentState;

    // Start is called before the first frame update
    void Start()
    {
        _equipmentComponent = GetComponent<Equipment>();
        _dialoguePlayerComponent = GetComponent<DialoguePlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SetEquipmentState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ChangeDialogueData();
        }
    }

    private void ChangeDialogueData()
    {
        if (_equipmentState == EquipmentStates.SampleNotAcquired && _equipmentComponent.equipmentName == "Work Desk")
            _dialoguePlayerComponent.curDialogueData = Resources.Load<DialogueData1>("Dialogues/WorkingDesk");
        else if(_equipmentState == EquipmentStates.SampleAcquired && _equipmentComponent.equipmentName == "Work Desk")
            _dialoguePlayerComponent.curDialogueData = Resources.Load<DialogueData1>("Dialogues/WorkingDeskOneLine");
    }

    private void SetEquipmentState()
    {
        switch(GameManager.instance.hasGotSample)
        {
            case true:
                _equipmentState = EquipmentStates.SampleAcquired;
                break;
            case false:
                _equipmentState = EquipmentStates.SampleNotAcquired;
                break;
        }
    }

}
