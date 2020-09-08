using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStart : MonoBehaviour
{
    [SerializeField]
    private float thresholdHeat;

    [SerializeField]
    private float startAcceleration;

    public float acceleration;

    //[SerializeField]
    private float maxRotate = (14f / (Mathf.PI * 2)) * 360f;

    private float currentRotate = (7f / (Mathf.PI * 2)) * 360f;

    public float currentHeat;

    private float rotationDirection = 1f;
    private Vector3 velocity;

    public Transform rightHand;
    public Transform leftHand;

    private ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        currentHeat = 0f;
        velocity = new Vector3();
        ps = GetComponent<ParticleSystem>();
        acceleration = startAcceleration;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ChangeDirection();
        }
        if (velocity.magnitude > currentHeat) {
            currentHeat = velocity.magnitude;
        }
        if ((currentRotate < maxRotate || rotationDirection == -1f) && (currentRotate > 0f || rotationDirection == 1f)) {
            HandleRotation();
        } else {
            currentHeat *= Mathf.Pow(0.5f, Time.deltaTime);
        }
        Vector3 rightCurrent = rightHand.localPosition;
        rightCurrent.z = -((currentRotate / 360f) * Mathf.PI * 2) + 7f;
        rightHand.localPosition = rightCurrent;
        Vector3 leftCurrent = leftHand.localPosition;
        leftCurrent.z = ((currentRotate / 360f) * Mathf.PI * 2) - 7f;
        leftHand.localPosition = leftCurrent;
        ParticleSystem.EmissionModule emission = ps.emission;
        emission.rateOverTime = currentHeat * 10;
        if (currentHeat > thresholdHeat) {
            Debug.Log("We did it!");
        }
    }

    private void HandleRotation() {

        Vector3 currentRot = transform.localEulerAngles;
        float newSpeed = velocity.y + rotationDirection * acceleration * Time.deltaTime;
        currentRotate = currentRotate + newSpeed;
        if (currentRotate > maxRotate || currentRotate < 0f) {
            velocity = new Vector3();
            currentRotate = Mathf.Clamp(currentRotate, 0f, maxRotate);
        } else {
            velocity.y = newSpeed;
            currentRot += velocity;
            transform.localEulerAngles = currentRot;
        }
    }

    private void ChangeDirection() {
        // If we're in 70%+, we increase the acceleration.
        // Otherwise let's reduce it
        float completed = CompletedMotion();
        if (completed < 1.0f) {
            acceleration *= Mathf.Lerp(1.0f, 1.2f, completed);
        } else {
            acceleration *= 0.95f;
        }
        if (acceleration < startAcceleration) {
            acceleration = startAcceleration;
        }
        rotationDirection *= -1f;
        HandleRotation();
    }

    private float CompletedMotion() {
        if (rotationDirection == 1f) {
            return currentRotate / maxRotate;
        } else {
            return 1f - (currentRotate / maxRotate);
        }
    }
}
