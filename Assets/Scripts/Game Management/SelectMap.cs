using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectMap : MonoBehaviour {
    public string[] levels;
    public Sprite[] mapPictures;
    public int firstSceneNumber;
    public Text mapText;
    public Image mapPicture;
    private int levelIndex;

    void Start()
    {
        levelIndex = 0;
        setMapText();
    }

    public void CycleMap()
    {
        levelIndex++;

        if (levelIndex >= levels.Length)
        {
            levelIndex = 0;
        }

        setMapText();
        setMapPicture();
    }

    public int SelectedSceneIndex()
    {
        return levelIndex + firstSceneNumber;
    }

    private void setMapText()
    {
        mapText.text = "Map: " + levels[levelIndex];
    }

    private void setMapPicture()
    {
        mapPicture.sprite = mapPictures[levelIndex];
    }
}
