using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioClip stepSound;
    public float stepVolume = 0.5f;
    public float stepDistance = 2f;

    private AudioSource audioSource;
    private CharacterController controller;
    private Vector3 lastPosition;
    private float distanceTraveled;

    public void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
    }

    public void Update()
    {
        // Считаем пройденное расстояние
        distanceTraveled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;

        // Если прошли нужное расстояние - звук шага
        if (distanceTraveled >= stepDistance && controller.velocity.magnitude > 0.1f)
        {
            audioSource.PlayOneShot(stepSound, stepVolume);
            distanceTraveled = 0f;
        }
    }
}