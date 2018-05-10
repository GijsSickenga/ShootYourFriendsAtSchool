using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour {

    public bool gamePaused;

    public GameObject pauseImage;

    public EventSystem eventSystem;
    public GameObject firstButton;

    private GamePadState[] currentState;
    private GamePadState[] previousState;

    void Awake()
    {
        gamePaused = false;
        currentState = new GamePadState[4];
        previousState = new GamePadState[4];
    }
	
	void Update () {
        for (int i = 0; i < currentState.Length; i++)
        {
            currentState[i] = GamePad.GetState((PlayerIndex)i);
            
            if(currentState[i].Buttons.Start == ButtonState.Released && previousState[i].Buttons.Start == ButtonState.Pressed)
            {
                TogglePauseGame();
            }

            previousState[i] = currentState[i];
        }

	    
	}

    public void TogglePauseGame()
    {
        gamePaused = gamePaused ? false : true;
        pauseImage.SetActive(gamePaused);

        if(gamePaused)
        {
            eventSystem.SetSelectedGameObject(null);
            firstButton.GetComponent<Button>().Select();
        }

        Time.timeScale = gamePaused ? 0.0f : 1.0f;
    }
}
