using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {

    public static int p1Deaths, p2Deaths, p3Deaths, p4Deaths, p1Kills, p2Kills, p3Kills, p4Kills, Lives, mode, rounds, gameStart;
    public float gameTime, countdownNum, menuCount;
     public static bool roundStart, countdown, countDone;

    public GUIText Round, Count;
    public TextMesh Timer;
  public Animation anim;
  

    // Use this for initialization
    void Start () {
        p1Deaths = 0;
        p2Deaths = 0;
        p3Deaths = 0;
        p4Deaths = 0;

        p1Kills = 0;
        p2Kills = 0;
        p3Kills = 0;
        p4Kills = 0;

        //Mode 0 = Time based, 1 = Round based, 2 = lives based
        mode = 0;
        Lives = 0;
        rounds = 0;
        gameTime = 300;

        //reset
        gameStart = 0;
        countDone = false;
        roundStart = false;

        //startvalue for timers
        countdownNum = 3;
        menuCount = 0;

       


    }
	
	// Update is called once per frame
	void FixedUpdate () {

        //countdown sequence 
        countdownNum = CountDown(countdownNum);
        

      
        if (roundStart == false && countDone == true)
        {
            roundStart = true;
        }

      gameTime = GameTime(gameTime);

        if (gameTime > 0 && roundStart == true)
        {
            //Timer. = true;
            gameTime -= Time.fixedDeltaTime;
        }

        if( gameTime < 0 )
        {
            //Timer.enabled = false;
            roundStart = false;
            gameStart = 2;
        }

        if (gameStart == 2)
        {

            menuCount = MenuCount(menuCount);
        }

    }

    float CountDown(float number)
    {
        if (number > 0)
        {
            Count.enabled = true;
            number -= Time.fixedDeltaTime;
            Count.text = number.ToString("F0");
        

        }

      if (number < 0)
        {
            countDone = true;
            Count.enabled = false;
            gameStart = 1;
        }

        return number;
    }
    
    float GameTime(float totalSeconds)
    {
        float minutes = Mathf.Floor(totalSeconds / 60);
        float seconds = totalSeconds % 60;
          Timer.text = minutes.ToString() + ":" + seconds.ToString("F0");
        return totalSeconds;
    }

    float MenuCount(float sec)
    {
        if(sec < 10f)
        {
            sec += Time.fixedDeltaTime;
        }

        else
        {
            SceneManager.LoadScene("LocalLobby");
        }

        return sec;
    }

   }
