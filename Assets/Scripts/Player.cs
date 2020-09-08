using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float topSpeed;

    [SerializeField]
    private float acceleration;

    [SerializeField]
    private float energy;

    [SerializeField]
    private GameObject firePrefab;

    private Rigidbody rigidbody;

    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active) {
            float movement = Input.GetAxis("Horizontal");
            Vector3 velocity = rigidbody.velocity;
            float beforeX = velocity.x;
            velocity.x = Mathf.Clamp(velocity.x + (acceleration * Time.deltaTime * movement), -topSpeed, topSpeed);
            rigidbody.velocity = velocity;
            if (movement != 0f) {
                energy -= Time.deltaTime;
            }
        }
    }

    public void MakeFire() {
        Vector3 pos = transform.position;
        // Put it at their feet
        pos.y -= 1f;
        Instantiate(firePrefab, pos, new Quaternion());
    }
}
