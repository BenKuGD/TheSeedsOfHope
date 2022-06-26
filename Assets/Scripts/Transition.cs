using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public string levelName;
    private string _currentLevelName;
    private int _lastLevelIndex;

    void Start()
    {
        Jukebox.instance.CheckTheCurrentScene();
        _currentLevelName = Jukebox.instance.sceneName;
        //Jukebox.instance.AudioTrackChanger();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GetLastSceneIndex() < 0)
                GameManager.instance.startPosVar = StartingPositions.OfficeEntered;
            else if (_currentLevelName == "Office" && levelName == "Bedroom" && GetLastSceneIndex() == 0)
                GameManager.instance.startPosVar = StartingPositions.BackToBedroom;
            else if (levelName == "Main Lab" && _currentLevelName == "Office")
                GameManager.instance.startPosVar = StartingPositions.LabEntered;
            else
                return;

            SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        }
    }

    private int GetLastSceneIndex()
    {
        return _lastLevelIndex = SceneManager.GetActiveScene().buildIndex - 1;
    }
}
