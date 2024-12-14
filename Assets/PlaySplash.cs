using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySplash : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && Time.timeSinceLevelLoad > 10f)
        {
            _audioSource.Play();
        }
    }
}
