using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Microscope : Equipment
{
    [Header("UI Elements")]
    public GameObject microscopeInterface;
    public GameObject warningMessage;
    public Scrollbar lightingSlider;
    public Button magnificationControl;
    public RectTransform magnificationControlPos;
    public Image microscopeField;
    public Image sample;
    public Image magnificationControlImage;
    public Image verifiedSample;
    public Button[] scanButtons;
    public List<GameObject> microscopeMessages = new List<GameObject>();
    public RectTransform[] magnificationControlTargets;

    [Header("Variables")]
    public string sampleName;
    private int  _curMagnifyLevel;
    private bool _hasStartedResearch;
    private bool _hasFinishedResearch;
    public bool HasLightingSet { get; private set; }
    public bool HasScanningEnabled { get; private set; }
    private BoxCollider _microscopeCollider;
    private DialoguePlayer _dialoguePlayer;

    public delegate void OnMagnificationSet();
    public static OnMagnificationSet OnMagnificationChanged;

    private void Start()
    {
        _player = FindObjectOfType<Player>();     
        OnMagnificationChanged += OnMagnificationChange;
        _microscopeCollider = GetComponent<BoxCollider>();
        _collider = _microscopeCollider;
        _dialoguePlayer = GetComponent <DialoguePlayer>();

        InitializeMicroscope();
    }

    private void LateUpdate()
    {
        if (!_hasFinishedResearch)
            EnableScanning();
        else
            return;
    }

    private void InitializeMicroscope()
    {
        _curMagnifyLevel = 0;
        _hasStartedResearch = true;
        _hasFinishedResearch = false;
        HasScanningEnabled = false;
        HasLightingSet = false;
        _dialoguePlayer.isAnEquipmentInterface = true;

        lightingSlider.value = .0f;
        magnificationControlImage.color = Color.gray;
        magnificationControl.interactable = false;
        microscopeField.color = Color.black;
        sample.color = new Color(0, 0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.instance.hasGotSample)
        {
            if (other.CompareTag("Player") && !_hasFinishedResearch)
            {
                _dialoguePlayer.isDialogueEnabled = true;
                _dialoguePlayer.BlockTheInterface(true);
                microscopeInterface.SetActive(true);
                GameManager.instance.playerIsBusy = true;
            }
        }
        else
        {
            _dialoguePlayer.isDialogueEnabled = false;
            GameManager.instance.playerIsBusy = true;
            warningMessage.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !_hasFinishedResearch)
        {
            CheckForDialogue();
        }
    }

    public void LitTheField()
    {
        float red = lightingSlider.value;
        float blue = lightingSlider.value;
        float green = lightingSlider.value;
        microscopeField.color = new Color(red, blue, green);
        EnableMagnification(true);
    }

    private void EnableMagnification(bool isLit)
    {
        if (isLit && lightingSlider.value == 1.0f)
        {
            HasLightingSet = true;
            magnificationControl.interactable = true;
            magnificationControlImage.color = Color.white;

            if (_hasStartedResearch)
            {
                sample.color = Color.white;
                _hasStartedResearch = false;
            }
        }
        else if(!isLit || lightingSlider.value != 1.0f)
        {
            HasLightingSet = false;
            magnificationControl.interactable = false;
            sample.color = new Color(0, 0, 0, 0);
            _hasStartedResearch = true;
        }
    }

    public void MoveMagnificationControl()
    {
        if (_curMagnifyLevel == 0)
        {
            magnificationControlPos.transform.localPosition = magnificationControlTargets[1].localPosition;
            sample.rectTransform.sizeDelta = new Vector2(251, 246);
            _curMagnifyLevel++;
            OnMagnificationChanged();
        }
        else if(_curMagnifyLevel == 1)
        {
            magnificationControlPos.transform.localPosition = magnificationControlTargets[2].localPosition;
            sample.rectTransform.sizeDelta = new Vector2(251 * 1.5f, 246 * 1.5f);
            _curMagnifyLevel++;
            OnMagnificationChanged();
        }
        else if(_curMagnifyLevel == 2)
        {
            magnificationControlPos.transform.localPosition = magnificationControlTargets[3].localPosition;
            sample.rectTransform.sizeDelta = new Vector2(251 * 3.0f, 246 * 3.0f);
            _curMagnifyLevel++;
            OnMagnificationChanged();
        }
        else if(_curMagnifyLevel == 3)
        {
            magnificationControl.interactable = false;
        }
    }

    private void OnMagnificationChange()
    {
        EnableMagnification(false);
        lightingSlider.value = Random.Range(.0f, .89f);
        HasLightingSet = false;
    }

    private void EnableScanning()
    {
        if(HasLightingSet)
        {
            if (_curMagnifyLevel == 3 && sample.rectTransform.rect.width >= 251 && sample.rectTransform.localPosition.x <= -100 || _curMagnifyLevel == 3 && sample.rectTransform.rect.width >= 251 && sample.rectTransform.localPosition.x >= 100)
            {
                scanButtons[0].gameObject.SetActive(true);
                HasScanningEnabled = true;
            }

        }
    }

    public void OnScanButton()
    {
        microscopeMessages[0].gameObject.SetActive(true);
        GameManager.instance.hasVerifiedSample = true;
        Invoke(nameof(DisableMicroscope), 3.0f);
    }

    private void DisableMicroscope()
    {
        microscopeInterface.gameObject.SetActive(false);
        _hasFinishedResearch = true;
        GameManager.instance.playerIsBusy = false;
        verifiedSample.gameObject.SetActive(true);
        _dialoguePlayer.isDialogueEnabled = false;
    }

    public void DismissWarning()
    {
        GameManager.instance.playerIsBusy = false;
        warningMessage.SetActive(false);
    }

    private void CheckForDialogue()
    {
        if (GameManager.instance.isDialogueBlockingTheStudy)
        {
            lightingSlider.interactable = false;
            magnificationControl.interactable = false;
        }
        else if(!GameManager.instance.isDialogueBlockingTheStudy)
        {
            lightingSlider.interactable = true;
        }
    }

    public bool OnScanningEnabled()
    {
        return HasScanningEnabled;
    }

    public bool OnLightingSet()
    {
        return HasLightingSet;
    }
}
