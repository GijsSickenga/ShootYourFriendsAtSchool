using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XInputDotNetPure;

public class PlayerManager : MonoBehaviour {
    public GameObject[] playerBlocks;
    public Color[] availableColors;

    private GamePadState[] currentStates;
    private GamePadState[] previousStates;

    void Start()
    {
        for(int i = 0; i < playerBlocks.Length; i++)
        {
            playerBlocks[i].GetComponent<PlayerBlock>().changeColor(i);
            ColorTracker.playerActive[i] = false;
        }

        currentStates = new GamePadState[4];
        previousStates = new GamePadState[4];
    }

    void Update()
    {
        for (int i = 0; i < currentStates.Length; i++)
        {
            currentStates[i] = GamePad.GetState((PlayerIndex)i);

            //Handle "Start" button input
            if (previousStates[i].Buttons.Start == ButtonState.Pressed && currentStates[i].Buttons.Start == ButtonState.Released)
            {
                if (!playerBlocks[i].activeSelf)
                {
                    playerBlocks[i].SetActive(true);
                    ColorTracker.playerActive[i] = true;
                    ChangeIfNeeded(playerBlocks[i]);
                }
            }

            //Handle "B" button input
            if (previousStates[i].Buttons.B == ButtonState.Pressed && currentStates[i].Buttons.B == ButtonState.Released)
            {
                if (playerBlocks[i].activeSelf)
                {
                    playerBlocks[i].SetActive(false);
                    ColorTracker.playerActive[i] = false;
                }
            }

            //Handle "Y" button input
            if (previousStates[i].Buttons.Y == ButtonState.Pressed && currentStates[i].Buttons.Y == ButtonState.Released)
            {
                if (playerBlocks[i].activeSelf)
                {
                    CycleColor(playerBlocks[i]);
                }
            }

            previousStates[i] = currentStates[i];
        }
    }

    private void CycleColor(GameObject block)
    {
        int index = block.GetComponent<PlayerBlock>().colorIndex;

        index++;

        if (index >= availableColors.Length)
        {
            index = 0;
        }

        index = checkSame(block, index);

        block.GetComponent<PlayerBlock>().changeColor(index);
    }

    private void ChangeIfNeeded(GameObject block)
    {
        int index = block.GetComponent<PlayerBlock>().colorIndex;

        index = checkSame(block, index);

        block.GetComponent<PlayerBlock>().changeColor(index);
    }

    private int checkSame(GameObject block, int index)
    {
        bool isSame;

        do
        {
            isSame = false;

            for (int i = 0; i < playerBlocks.Length; i++)
            {
                if (!playerBlocks[i].activeSelf || playerBlocks[i] == block)
                {
                    continue;
                }

                int otherIndex = playerBlocks[i].GetComponent<PlayerBlock>().colorIndex;

                if (index == otherIndex)
                {
                    isSame = true;

                    index++;

                    if (index >= availableColors.Length)
                    {
                        index = 0;
                    }
                    break;
                }
            }
        } while (isSame);

        return index;
    }
}
