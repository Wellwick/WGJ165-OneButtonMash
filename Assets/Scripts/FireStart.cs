using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStart : MonoBehaviour
{
    [SerializeField]
    private float thresholdHeat;

    [SerializeField]
    private float startAcceleration;

    //[SerializeField]
    private float maxRotate = (14f / (Mathf.PI * 2)) * 360f;

    private float currentRotate = (7f / (Mathf.PI * 2)) * 360f;

    public float currentHeat;

    private float rotationDirection = 1f;
    private Vector3 velocity;

    public Transform hand;
    // Start is called before the first frame update
    void Start()
    {
        currentHeat = 0f;
        velocity = new Vector3();
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
            Debug.Log("Current heat is now " + currentHeat);
        }
        Vector3 current = hand.localPosition;
        current.z = -((currentRotate / 360f) * Mathf.PI * 2) + 7f;
        hand.localPosition = current;
    }

    private void HandleRotation() {

        Vector3 currentRot = transform.localEulerAngles;
        float newSpeed = velocity.y + rotationDirection * startAcceleration * Time.deltaTime;
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
        rotationDirection *= -1f;
        HandleRotation();
    }
}
