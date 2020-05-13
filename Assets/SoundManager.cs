using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField]
    private AudioSource gripperGrab;
    [SerializeField]
    private AudioSource gripperRelease;
    [SerializeField]
    private AudioSource buttonPush;
    [SerializeField]
    private AudioSource buttonRelease;

    private void Awake()
    {
        Instance = this;
    }

    private void PlaySound(AudioSource sound, Vector3 position)
    {
        sound.transform.position = position;
        sound.Play();
    }

    public void PlayGripGrab(Vector3 position)
    {
        PlaySound(gripperGrab, position);
    }

    public void PlayGripRelease(Vector3 position)
    {
        PlaySound(gripperRelease, position);
    }
}
