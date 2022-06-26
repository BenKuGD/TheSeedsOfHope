using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicroscopeSample : MonoBehaviour
{ 
    private float _moveSpeed;
    private Vector3 _sampleStartingPos;
    private Camera _cam;
    private Vector2 mousePosition;
    [SerializeField] private Transform[] sampleBounds;

    private float _scalingFactor;
    private Microscope _microscopeParent;

    public Scrollbar lightingSlider;


    // Start is called before the first frame update
    void Start()
    {
        _sampleStartingPos = this.transform.position;
        _moveSpeed = 300.0f;
        _cam = Camera.main;
        _microscopeParent = FindObjectOfType<Microscope>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_microscopeParent.OnScanningEnabled())
        {
            if (IsMouseOverSample() && Input.GetMouseButton(0) && lightingSlider.value == 1.0f)
            {
                MoveSample();
            }
        }
        else
            return;
    }

    private void LateUpdate()
    {
        ResetUnlitSample();
    }

    private void MoveSample()
    {
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;

        if (!GameManager.instance.hasVerifiedSample && _microscopeParent.OnLightingSet())
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(x, y, transform.position.z), Time.deltaTime * _moveSpeed);
    }

    private bool IsMouseOverSample()
    {
        Vector2 mousePosition = _cam.ScreenToWorldPoint(Input.mousePosition);

        float leftX = sampleBounds[0].position.x;
        float rightX = sampleBounds[1].position.x;
        float upperY = sampleBounds[2].position.y;
        float lowerY = sampleBounds[3].position.y;

        if (mousePosition.x >= leftX && mousePosition.x <= rightX && mousePosition.y >= lowerY && mousePosition.y <= upperY)
            return true;
        else
            return false;
    }

    private void ResetUnlitSample()
    {
        if (!_microscopeParent.OnLightingSet())
            transform.position = _sampleStartingPos;
        else
            return;
    }
}
