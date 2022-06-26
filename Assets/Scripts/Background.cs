using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public static Background instance;
    private BoxCollider _collider;

    private void Awake()
    {
        instance = this;
        _collider = GetComponent<BoxCollider>();
    }

    public void CheckForMovement(bool playerIsMoving)
    {
        if (playerIsMoving)
            _collider.enabled = false;
        else if (!playerIsMoving)
            _collider.enabled = true;
    }
}
