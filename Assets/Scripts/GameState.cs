using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{

    [SerializeField]
    private Camera playerCam, fireCam;

    [SerializeField]
    private Player player;

    [SerializeField]
    private FireStart fireStart;

    private bool playerActive;
    // Start is called before the first frame update
    void Start()
    {
        // Set to false so we can change it!
        playerActive = false;
        // Trigger the state set
        SwitchActive();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            SwitchActive();
        }
    }

    public void SwitchActive() {
        // Swap!
        playerActive = !playerActive;

        // Setup player!
        player.active = playerActive;
        playerCam.enabled = playerActive;
        playerCam.GetComponent<AudioListener>().enabled = playerActive;

        // Setup fireStart
        fireStart.ResetValues();
        fireStart.active = !playerActive;
        fireCam.enabled = !playerActive;
        fireCam.GetComponent<AudioListener>().enabled = !playerActive;

    }

    public void MakeFire() {
        if (!playerActive) {
            SwitchActive();
            player.MakeFire();
        } else {
            Debug.LogError("We shouldn't be able to make a fire unless we are currently in FireStart!");
        }
    }
}
