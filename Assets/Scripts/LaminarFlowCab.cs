using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaminarFlowCab : MonoBehaviour
{
    public GameObject laminarInterface;
    public List<Scrollbar> laminarControls = new List<Scrollbar>();
    public List<GameObject> dialoguesLaminar = new List<GameObject>();
    public List<Image> experimentImages = new List<Image>();
    public GameObject exposureButton;
    public GameObject warningMessage;

    [SerializeField] private GameObject endScreen;
    private DialoguePlayer _dialoguePlayerComponent;

    private readonly float _startingExposureValue = .0f;
    private readonly float _criticalExposureValue = .8f;
    private readonly float _optimalValueMin = .35f;
    private readonly float _optimalValueMax = .65f;
    private bool _hasDoneFirstRun = false;
    private bool _hasDamagedSample = false;

    private void Start()
    {
        _dialoguePlayerComponent = GetComponent<DialoguePlayer>();
        SetUpTheInterface();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.instance.hasVerifiedSample)
        {
            if (other.CompareTag("Player"))
            {
               _dialoguePlayerComponent.isDialogueEnabled = true;
               laminarInterface.gameObject.SetActive(true);
               GameManager.instance.playerIsBusy = true;
               GameManager.instance.isDialogueBlockingTheStudy = true;
            }
        }
        else
        {
            _dialoguePlayerComponent.isDialogueEnabled = false;
            GameManager.instance.playerIsBusy = true;
            warningMessage.SetActive(true);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            CheckForDialogue();
    }

    public void ExposureButtonControl()
    {
        if (laminarControls[0].value <= _startingExposureValue + .15f && laminarControls[1].value > _startingExposureValue && laminarControls[1].value <= _optimalValueMin)
        {
            _hasDoneFirstRun = true;
            _hasDamagedSample = false;
            dialoguesLaminar[0].gameObject.SetActive(true);
            experimentImages[1].gameObject.SetActive(false);
            experimentImages[0].gameObject.SetActive(true);
        }
        else if (laminarControls[0].value >= _criticalExposureValue && laminarControls[1].value >= _criticalExposureValue)
        {
            dialoguesLaminar[1].gameObject.SetActive(true);
            experimentImages[1].gameObject.SetActive(true);
            _hasDamagedSample = true;
        }
        else if(_hasDoneFirstRun && !_hasDamagedSample && OnSuccessfulExperiment())
        {
            dialoguesLaminar[2].gameObject.SetActive(true);
            experimentImages[2].gameObject.SetActive(true);

            exposureButton.gameObject.SetActive(false);

            StartCoroutine(EndScreenPopup(CallEndScreen));
        }
        else if(!_hasDoneFirstRun && !_hasDamagedSample)
        {
            dialoguesLaminar[3].gameObject.SetActive(true);
        }
        else if(_hasDamagedSample && !OnLaminarControlsReset())
        {
            dialoguesLaminar[4].gameObject.SetActive(true);
        }
        else if (_hasDoneFirstRun)
        {
            experimentImages[1].gameObject.SetActive(false);
            experimentImages[0].gameObject.SetActive(true);
            dialoguesLaminar[0].gameObject.SetActive(true);
        }
        else if(_hasDamagedSample && OnLaminarControlsReset())
        {
            _hasDamagedSample = false;
            experimentImages[1].gameObject.SetActive(false);
            experimentImages[0].gameObject.SetActive(true);
            dialoguesLaminar[0].gameObject.SetActive(true);
        }
    }

    private void SetUpTheInterface()
    {
        foreach (Scrollbar laminarControl in laminarControls)
            laminarControl.value = .0f;
    }

    public void DismissDialogue()
    {
        foreach (GameObject gameObject in dialoguesLaminar)
            gameObject.SetActive(false);
    }

    void CallEndScreen()
    {
        endScreen.gameObject.SetActive(true);

        foreach (Scrollbar scrollbar in laminarControls)
            scrollbar.gameObject.SetActive(false);
    }

    public IEnumerator EndScreenPopup(System.Action callback)
    {
        yield return new WaitForSeconds(3.0f);
        callback();
    }

    public void DismissWarning()
    {
        GameManager.instance.playerIsBusy = false;
        warningMessage.SetActive(false);
    }

    private void CheckForDialogue()
    {
        if(GameManager.instance.isDialogueBlockingTheStudy)
        {
            foreach (Scrollbar laminarControl in laminarControls)
                laminarControl.interactable = false;
            exposureButton.SetActive(false);
        }
        else if(!GameManager.instance.isDialogueBlockingTheStudy)
        {
            foreach (Scrollbar laminarControl in laminarControls)
                laminarControl.interactable = true;
            exposureButton.SetActive(true);
        }
    }

    private bool OnSuccessfulExperiment()
    {
        if (laminarControls[1].value <= _optimalValueMax && laminarControls[1].value >= _optimalValueMin && laminarControls[0].value <= _optimalValueMax && laminarControls[0].value >= _optimalValueMin)
            return true;
        else
            return false;
    }

    private bool OnLaminarControlsReset()
    {
        bool areReset = false;

        foreach (Scrollbar laminarControl in laminarControls)
        {
            areReset = laminarControl.value == _startingExposureValue ? true : false;
        }

        return areReset;
    }
}
