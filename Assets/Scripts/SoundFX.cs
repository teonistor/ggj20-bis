using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour {
    [SerializeField] AudioClip begin;
    [SerializeField] AudioClip conquer;
    [SerializeField] AudioClip kill;
    [SerializeField] AudioClip launch;
    [SerializeField] AudioClip win;

    private AudioSource audioSource;
    private static SoundFX instance;

    void Awake () {
        audioSource = GetComponent<AudioSource>();
        instance = this;
    }

    internal static void Begin () {
        instance.audioSource.PlayOneShot(instance.begin);
    }
    internal static void Conquer () {
        instance.audioSource.PlayOneShot(instance.conquer);
    }
    internal static void Kill () {
        instance.audioSource.PlayOneShot(instance.kill);
    }
    internal static void Launch () {
        instance.audioSource.PlayOneShot(instance.launch);
    }
    internal static void Win () {
        instance.audioSource.PlayOneShot(instance.win);
    }
}
