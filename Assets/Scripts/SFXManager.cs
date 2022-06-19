using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip[] clips;

    public GameObject speakerPrefab;

    public AudioSource gunShotPlayer;

    public void PlayClip(int idx)
    {
        var speaker = Instantiate(speakerPrefab);
        var audioPlayer = speaker.GetComponent<AudioSource>();
        audioPlayer.clip = clips[idx];
        audioPlayer.Play();
        Destroy(speaker, clips[idx].length + 1);
    }

    public void PlayGunShot()
    {
        gunShotPlayer.loop = true;
        gunShotPlayer.Play();
    }

    public void StopGunShot()
    {
        gunShotPlayer.loop = false;
    }
}
