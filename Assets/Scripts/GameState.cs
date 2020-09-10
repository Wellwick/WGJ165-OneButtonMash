using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{

    [SerializeField]
    private Camera playerCam, fireCam;

    [SerializeField]
    private Player player;

    [SerializeField]
    private FireStart fireStart;

    [SerializeField]
    private Image foreground;

    [SerializeField]
    private Text endText;

    [SerializeField]
    private float fadeOutTime = 3f;

    private float currentFadeOutTime;

    private bool finished;

    private float playTime;

    private bool playerActive;
    // Start is called before the first frame update
    void Start() {
        finished = false;

        // Set to false so we can change it!
        playerActive = false;
        // Trigger the state set
        SwitchActive();

        currentFadeOutTime = 0f;
        playTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && !finished) {
            SwitchActive();
        } else if (finished) {
            if (Input.GetKeyDown(KeyCode.Backspace)) {
                SceneManager.LoadScene("Firemaking");
            }
            if (currentFadeOutTime < fadeOutTime) {
                currentFadeOutTime = Mathf.Clamp(currentFadeOutTime + Time.deltaTime, 0f, fadeOutTime);
                float completed = currentFadeOutTime / fadeOutTime;
                Color foreColor = foreground.color;
                foreColor.a = completed;
                foreground.color = foreColor;
                Color textColor = endText.color;
                textColor.a = completed;
                endText.color = textColor;
            }
        }
        playTime += Time.deltaTime;
    }

    public void SwitchActive() {
        if ((playerActive && !player.CanBuildFire()) || finished) {
            // No point swapping if the player is building a fire
            // Or if we are finished
            return;
        }

        // Swap!
        playerActive = !playerActive;

        // Setup player!
        player.active = playerActive;
        playerCam.enabled = playerActive;
        //playerCam.GetComponent<AudioListener>().enabled = playerActive;

        // Setup fireStart
        fireStart.ResetValues();
        fireStart.active = !playerActive;
        fireCam.enabled = !playerActive;
        //fireCam.GetComponent<AudioListener>().enabled = !playerActive;

    }

    public void MakeFire() {
        if (!playerActive) {
            SwitchActive();
            player.MakeFire();
        } else {
            Debug.LogError("We shouldn't be able to make a fire unless we are currently in FireStart!");
        }
    }

    public void WinGame() {
        finished = true;

        playerActive = false;
        player.active = false;

        endText.text = "You signalled for help!\nYou succesfully escaped in \n"+playTime+ "seconds!\n\nPress Backspace to play again";
        Debug.Log("Game won!");
    }

    public void LoseGame() {
        finished = true;

        playerActive = false;
        player.active = false;

        endText.text = "You froze to death!\nGame Over!\n\nPress Backspace to play again";
        Debug.Log("Game over!");
    }
}
