using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip music;
    [SerializeField, Range(0f,10f)]
    private float volume = 0.5f;
    [SerializeField]
    private bool playOnStart = true;
    [SerializeField]
    private bool loop = true;

    private AudioSource audioSource;

    public void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = music;
        audioSource.volume = volume;
        audioSource.loop = loop;

        if (playOnStart)
            Play();
    }

    public void Play()
    {
        if (music != null)
            audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        audioSource.volume = volume;
    }
}