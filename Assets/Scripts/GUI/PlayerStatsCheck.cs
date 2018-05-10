using UnityEngine;
using System.Collections;

public class PlayerStatsCheck : MonoBehaviour
{

    Texture2D Rectangle;
    SpriteRenderer sprite;
    public TextMesh guiMesh;

    string tagName;
    public string playerIndex;
    public float opacity;
    bool animIsDone, endIsDone;
    float animSpeed;

    // Use this for initialization
    void Start()
    {
        animSpeed = 0.0050f;
        opacity = 0;

        animIsDone = false;
        endIsDone = false;

        //initpos screenstats 
        switch (playerIndex)
        {
          case "1": objPos(-16f, 6.3f, "MiddleCenter", "Left"); break;
          case "2": objPos(-16f, -6.3f, "MiddleCenter", "Left"); break;
          case "3": objPos(16f, 6.3f, "MiddleCenter",  "Right"); break;
          case "4": objPos(16f, -6.3f, "MiddleCenter",  "Right"); break;
        }

        sprite = GetComponent<SpriteRenderer>();

    }

    void OnGUI()
    {

        //GUI.color = new Color(1.0f, 0, 0);
        //GUI.DrawTexture(new Rect(gameObject.transform.position.x, gameObject.transform.position.y, 25, 25), Rectangle);
        //GUI.color = Color.white;        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (animIsDone == false)
        {
            gameObject.GetComponent<Animation>().Play("SlideInPlayer" + playerIndex);
            animIsDone = true;

        }

            switch (playerIndex)
            {
                case "1": guiMesh.text = "P1\nKills: " + GameController.p1Kills.ToString() + "\nDeaths: " + GameController.p1Deaths.ToString(); break;
                case "2": guiMesh.text = "P2\nKills: " + GameController.p2Kills.ToString() + "\nDeaths: " + GameController.p2Deaths.ToString(); break;
                case "3": guiMesh.text = "P3\nKills: " + GameController.p3Kills.ToString() + "\nDeaths: " + GameController.p3Deaths.ToString(); break;
                case "4": guiMesh.text = "P4\nKills: " + GameController.p4Kills.ToString() + "\nDeaths: " + GameController.p4Deaths.ToString(); break;

            }
        


        //resultscreen 
        if (GameController.gameStart == 2 && endIsDone == false)
        {
            switch (playerIndex)
            {
                case "1": objPos(-10f, -0.5f, "MiddleCenter", "center"); break;
                case "2": objPos(-5f, 0.5f, "MiddleCenter", "center"); break;
                case "3": objPos(5f, -0.5f, "MiddleCenter", "center"); break;
                case "4": objPos(10f, 0.5f, "MiddleCenter", "center"); break;

            }
            gameObject.GetComponent<Animation>().PlayQueued("Endscreen" + playerIndex);
            endIsDone = true;
            sprite.sprite = Resources.Load("bgendstats", typeof(Sprite)) as Sprite;
        }

    }

    void objPos(float VecX, float vecY, string pos, string align)
    {
        switch (pos)
        {
            default: break;
            case "LowerCenter": guiMesh.anchor = TextAnchor.LowerCenter; break;
            case "MiddleCenter": guiMesh.anchor = TextAnchor.MiddleCenter; break;
            case "UpperCenter": guiMesh.anchor = TextAnchor.UpperCenter; break;
        }

        switch(align)
        {
            case "Left": guiMesh.alignment = TextAlignment.Left; gameObject.GetComponentInChildren<Transform>().transform.position = new Vector2(-0.2f, 0); break;
            case "Right": guiMesh.alignment = TextAlignment.Right; gameObject.GetComponentInChildren<Transform>().transform.position = new Vector2(0.2f, 0);  break;
            case "Center": guiMesh.alignment = TextAlignment.Center; break;
        }




        gameObject.transform.position = new Vector2(VecX, vecY);

        //sprite.sprite = Resources.Load("bgplayerstats", typeof(Sprite)) as Sprite;


    }
}
    
