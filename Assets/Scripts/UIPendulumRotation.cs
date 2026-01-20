using UnityEngine;

public class UIPendulumRotation : MonoBehaviour
{
    [Header("Pendulum Settings")]
    [SerializeField] private float rotationAngle = 5f; 
    [SerializeField] private float speed = 1f;           
    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.localRotation;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * speed) * rotationAngle;
        transform.localRotation = startRotation * Quaternion.Euler(0f, 0f, angle);
    }
}
