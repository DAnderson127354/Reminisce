using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource source;
    public AudioSource source1;
    public AudioClip click;
    public AudioClip shoot;
    public AudioClip hit;
    public AudioClip open;
    public AudioClip close;
    public AudioClip ghostSpawn;

    public void Click()
    {
        source1.PlayOneShot(click);
    }

    public void Shoot()
    {
        source1.PlayOneShot(shoot);
    }

    public void Hit()
    {
        source1.PlayOneShot(hit);
    }

    public void GhostSpawn()
    {
        source1.PlayOneShot(ghostSpawn);
    }

    public void Open()
    {
        source1.PlayOneShot(open);
    }

    public void Close()
    {
        source1.PlayOneShot(close);
    }

    public void Footsteps(AudioClip footsteps)
    {
        source.clip = footsteps;
        if (source.isPlaying == false)
        {
            source.Play(0);
            Debug.Log("playing");
        } 
    }

    public void AdjustPitch(float pitch)
    {
        source.pitch = pitch;
    }

    public void Stop()
    {
        source.Pause();
    }
}
