using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI instance;

    public List<GameObject> dialogues = new List<GameObject>();

    private int _dIx = 0; // dialogue index

    public TextMeshProUGUI missionText;
    public Image sampleImage;
    public Image verifiedSample;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Scene curScene = SceneManager.GetActiveScene();
        if (curScene.buildIndex == 0)
            GameManager.instance.EnableTransition();
        else
            GameManager.instance.DisableTransition();

        CheckForStatuses();
    }

    public void EnableUIObjects()
    {
        dialogues[_dIx].gameObject.SetActive(true);
        GameManager.instance.playerIsBusy = true;
    }

    public void NextDialogueOption()
    {
        _dIx++;
        dialogues[_dIx-1].gameObject.SetActive(false);
        dialogues[_dIx].gameObject.SetActive(true);
        if (_dIx > dialogues.Count)
            DisableUIObjects();
    }

    public void DisableUIObjects()
    {
        dialogues[_dIx].gameObject.SetActive(false);
        GameManager.instance.playerIsBusy = false;
        GameManager.instance.EnableTransition();
        _dIx = 0;

        if (missionText == null)
            return;
        else
        {
            missionText.text = "Current Task: Volvox Study";
            GameManager.instance.hasGotMission = true;
        }
    }

    private void CheckForStatuses()
    {
        if(GameManager.instance.hasGotMission)
            missionText.text = "Current Task: Volvox Study";

        if(GameManager.instance.hasGotSample)
            sampleImage.gameObject.SetActive(true);
    }
}
