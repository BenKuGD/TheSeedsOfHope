using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StartingPositions
{
    GameStart,
    BackToBedroom,
    OfficeEntered,
    BackToOffice,
    LabEntered
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool playerIsBusy = false;
    public bool hasGotMission = false;
    public bool hasGotSample = false;
    public bool hasVerifiedSample = false;
    public bool isDialogueBlockingTheStudy = false;

    public GameObject transition;

    public StartingPositions startPosVar = StartingPositions.GameStart;


    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    public void DisableTransition()
    {
        if (Jukebox.instance.sceneName == "Main Lab")
            return;
        else
        {
            transition = GameObject.FindWithTag("Transition");
            transition.gameObject.SetActive(false);
        }
    }

    public void EnableTransition()
    {
        if(transition != null)
            transition.gameObject.SetActive(true);
    }
}
