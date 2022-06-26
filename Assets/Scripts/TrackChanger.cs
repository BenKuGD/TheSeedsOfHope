using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackChanger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Jukebox.instance.ChangeTheTrack();
    }
}
