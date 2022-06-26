using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{ 
    public string equipmentName;
    public UIPopUp equipmentPopUp;

    protected Player _player;
    [SerializeField] private bool _isPlayerNearby;
    [SerializeField] private float _playerRange = 5.0f;
    protected BoxCollider _collider;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _collider = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
            CheckForMouseBeingOver();
            CheckIfNearby();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && this.gameObject.name == "Computer")
            UI.instance.EnableUIObjects();
        else if(other.CompareTag("Player"))
            GiveSample();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && this.gameObject.name == "Computer")
            UI.instance.DisableUIObjects();
    }

    protected void CheckForMouseBeingOver()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Equipment") && !GameManager.instance.playerIsBusy && _isPlayerNearby && hit.collider == _collider)
            {
                equipmentPopUp.gameObject.SetActive(true);
            }
            else 
                equipmentPopUp.gameObject.SetActive(false);
        }
    }

    private bool CheckIfNearby()
    {
        if (Mathf.Abs(Vector3.Distance(this.transform.position, _player.transform.position)) < _playerRange)
            return _isPlayerNearby = true;
        else
            return _isPlayerNearby = false;
    }

    private void GiveSample()
    {
        if (equipmentName == "Work Desk")
        {
            UI.instance.sampleImage.gameObject.SetActive(true);
            GameManager.instance.hasGotSample = true;
        }
           
    }
}

