using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float topSpeed;

    [SerializeField]
    private float acceleration;

    public float temperatureStart, temperatureMinimum;

    private float temperature;

    [SerializeField]
    private Image tempFill;

    [SerializeField]
    private GameObject firePrefab;

    private bool inWinTrigger;

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
        temperature = temperatureStart;
        inWinTrigger = false;
    }

    private void Update() {
        float completed = (temperature - temperatureMinimum) / (temperatureStart - temperatureMinimum);
        //Debug.Log("Temperature is currently " + completed);
        tempFill.fillAmount = completed;
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
                temperature -= Time.deltaTime;
            }
        }

        if (temperature <= temperatureMinimum) {
            FindObjectOfType<GameState>().LoseGame();
        }
    }

    public void MakeFire() {
        Vector3 pos = transform.position;
        // Put it at their feet
        pos.y -= 1f;
        Instantiate(firePrefab, pos, new Quaternion());
        if (inWinTrigger) {
            FindObjectOfType<GameState>().WinGame(); 
        }
    }

    public void WarmUp(float amount) {
        //Debug.Log("Warming up " + amount);
        temperature = Mathf.Clamp(temperature + amount, temperatureMinimum, temperatureStart);
    }

    private void OnTriggerStay(Collider other) {
        Fire f = other.GetComponent<Fire>();
        if (f) {
            WarmUp(f.heat * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other) {
        WinTrigger w = other.GetComponent<WinTrigger>();
        if (w) {
            inWinTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        WinTrigger w = other.GetComponent<WinTrigger>();
        if (w) {
            inWinTrigger = false;
        }
    }
}
