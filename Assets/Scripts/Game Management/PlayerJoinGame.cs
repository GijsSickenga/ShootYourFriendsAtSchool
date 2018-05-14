using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using System;

public class PlayerJoinGame : MonoBehaviour
{
    public GameObject playerPrefab, PlayerStatsBG;
    public PlayerIndex playerIndex;

    private GameObject playerInstance;
    private SpawnManager spawnManager;
    GamePadState currentState;

    public int secondsToSpawnPlayers = 3;

    void Start()
    {
        spawnManager = GetComponentInParent<SpawnManager>();

        for(int i = 0; i < 4; i++)
        {
            GameData.players[i] = null;
        }

        StartCoroutine(spawnPlayer());
    }

    /* Only for Debugging
    void Update()
    {
        currentState = GamePad.GetState(playerIndex);
        
        if (currentState.Buttons.Start == ButtonState.Pressed)
        {
            if (playerInstance == null && playerPrefab != null)
            {
                playerInstance = InitializePlayer();
                
                SetPlayerStat();
            }            
        }
    } */

    private IEnumerator spawnPlayer()
    {
        yield return new WaitForSeconds(secondsToSpawnPlayers);

        if (playerInstance == null && ColorTracker.playerActive[(int)playerIndex])
        {
            playerInstance = InitializePlayer();
            //SetPlayerStat(); wesley's tyfus UI
        }

        yield return new WaitForEndOfFrame();
        FindObjectOfType<WeaponGenerator>().FirstGenerate();
    }

    private GameObject InitializePlayer()
    {
        GameObject spawnPoint = spawnManager.PickRandomRespawn();
        GameObject aPlayer = Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity) as GameObject;
        
        LocalPlayerController player = aPlayer.GetComponent<LocalPlayerController>();

        player.pIndex = playerIndex;
        player.playerIndex = (int)playerIndex;

        GameData.players[player.playerIndex] = player;

        Vibration.VibrateForSeconds(0.3f, 0.5f, playerIndex);
        spawnManager.createParticleSystem(aPlayer);

        return aPlayer;
    }

    private GameObject SetPlayerStat()
    {
        GameObject statPlayer = Instantiate(PlayerStatsBG, Vector3.zero, Quaternion.identity) as GameObject;
        statPlayer.GetComponent<PlayerStatsCheck>().playerIndex = ((int)playerIndex + 1).ToString();
        //   statPlayer.AddComponent<PlayerStatsCheck>();
        return statPlayer;
    }
}
