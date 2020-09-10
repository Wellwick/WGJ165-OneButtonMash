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

    private float time = 0.0f;
    public float timeBetween = 0.5f;
    private Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        lastPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;

        if (time >= Time.deltaTime) {
            time = time - timeBetween;
            //Debug.Log(transform.position);
            if ((lastPos - transform.position).magnitude < 0.2) {
                //Debug.Log("Not Moving");
                
            }
            else {
                AkSoundEngine.PostEvent("Footsteps_Snow", gameObject);
                //Debug.Log((lastPos - transform.position).magnitude);
            }
            lastPos = transform.position;

        }

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
