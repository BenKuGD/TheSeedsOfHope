using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 _targetPosition;
    public float moveSpeed;
    private Animator _playersAnimator;

    void Start()
    {
        SetStartingPosition();
        _playersAnimator = GetComponent<Animator>();
        Jukebox.instance.CheckTheCurrentScene();
        Jukebox.instance.AudioTrackChanger();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Move();

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void FixedUpdate()
    {
        AnimationControl(GameManager.instance.playerIsBusy);
    }

    void Move()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            _targetPosition = hit.point;

            if (!hit.collider.CompareTag("Background"))
                return;

            StartCoroutine(MoveOverTime());

            IEnumerator MoveOverTime()
            {
                while (this.transform.position != _targetPosition && !GameManager.instance.playerIsBusy)
                {
                    AnimationToggle(true);
                    Background.instance.CheckForMovement(true);
                    transform.position = Vector3.MoveTowards(transform.position, _targetPosition, moveSpeed * Time.deltaTime);
                    yield return null;
                }
                AnimationToggle(false);
                Background.instance.CheckForMovement(false);
            }

            StopCoroutine(MoveOverTime());
        }

    }

    public void SetStartingPosition()
    {
        switch (GameManager.instance.startPosVar)
        {
            case StartingPositions.GameStart:
                transform.position = new Vector3(2.3f, -1f, 0f);
                break;
            case StartingPositions.BackToBedroom:
                transform.position = new Vector3(8.33f, -0.88f, 0f);
                break;
            case StartingPositions.OfficeEntered:
                transform.position = new Vector3(-6.88f, -0.08f, 0f);
                break;
            case StartingPositions.LabEntered:
                transform.position = new Vector3(.4f, .17f, .0f);
                break;
            default:
                break;
        }
    }

    private void AnimationToggle(bool isMoving)
    {
        _playersAnimator.SetBool("IsWalking", isMoving);
    }

    private void AnimationControl(bool isBusy)
    {
        if (isBusy)
            _playersAnimator.enabled = false;
        else
            _playersAnimator.enabled = true;
    }
}