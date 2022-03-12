using UnityEngine;

public class GeneralAudioScript : MonoBehaviour
{
    public AudioClip carCrashSfx;

    public AudioSource audioSrc;

    public void PlayCarCrashSfx()
    {
        audioSrc.PlayOneShot(carCrashSfx);
    }
}
