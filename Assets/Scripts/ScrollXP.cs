using UnityEngine;

public class ScrollXP : MonoBehaviour
{
    [Header("XP Settings")]
    [SerializeField] private int xpAmount = 50;

    [Header("Visuals & Effects")]
    [SerializeField] private GameObject pickupEffect;
    [SerializeField] private AudioClip pickupSound;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 50f;

    [Header("Levitation Settings")]
    [SerializeField] private float levitationAmplitude = 0.5f;
    [SerializeField] private float levitationSpeed = 2f; 

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        float newY = startPosition.y + Mathf.Sin(Time.time * levitationSpeed) * levitationAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerExperience playerExperience = other.GetComponent<PlayerExperience>();

            if (playerExperience != null)
            {
                playerExperience.AddXP(xpAmount);

                if (pickupEffect != null)
                {
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                }

                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }

                gameObject.SetActive(false);

                Destroy(gameObject);
            }
        }
    }
}
