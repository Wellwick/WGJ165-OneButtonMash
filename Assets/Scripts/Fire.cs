using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float heat;

    [SerializeField]
    private float heatLossPerSecond;

    private ParticleSystem fireSystem, smokeSystem;

    private Light light;

    private Player player;
    private uint fireID;

    public AK.Wwise.RTPC fireRTPC;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        fireSystem = GetComponent<ParticleSystem>();
        foreach (Transform t in transform) {
            if (t.name == "Smoke") {
                smokeSystem = t.GetComponent<ParticleSystem>();
            } else if (t.name == "Light") {
                light = t.GetComponent<Light>();
            }
        }
        fireID = AkSoundEngine.PostEvent("Fire", gameObject);
        AkSoundEngine.SetRTPCValue("FirePower", 100.0f, gameObject);

        //AkSoundEngine.SetRTPCValueByPlayingID("FirePower", fireID)
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        heat = Mathf.Clamp(heat - heatLossPerSecond * Time.deltaTime, 0f, 30f);
        SetParticles();
    }

    private void SetParticles() {
        ParticleSystem.EmissionModule fEmission = fireSystem.emission;
        fEmission.rateOverTime = Mathf.Clamp(heat * 15, 0, 30);
        ParticleSystem.EmissionModule sEmission = smokeSystem.emission;
        sEmission.rateOverTime = Mathf.Clamp(heat * 30, 0, 60);
        light.intensity = Mathf.Clamp(heat * 0.5f, 0f, 3f);
        if (light.intensity < 0.01) {
            AkSoundEngine.StopPlayingID(fireID);
        }
        AkSoundEngine.SetRTPCValue("FirePower", light.intensity * 100.0f, gameObject);
    }

    public void AddLog() {
        heat = Mathf.Clamp(heat + 1, 0f, 10f);
        SetParticles();
    }

    
}
