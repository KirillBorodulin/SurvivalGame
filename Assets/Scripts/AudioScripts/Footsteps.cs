using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField]
    private AudioClip Sound;
    [SerializeField]
    private float stepVolume = 0.5f;
    [SerializeField]
    private float stepDistance = 2f;

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
        distanceTraveled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;

        if (distanceTraveled >= stepDistance && controller.velocity.magnitude > 0.1f)
        {
            audioSource.PlayOneShot(Sound, stepVolume);
            distanceTraveled = 0f;
        }
    }
}