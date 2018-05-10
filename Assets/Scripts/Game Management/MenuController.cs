using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject loadingImage;

    public void StartSelectedScene()
    {
        SelectMap selectMap = this.GetComponent<SelectMap>();
        this.LoadScene(selectMap.SelectedSceneIndex());
    }

    public void LoadScene(int levelIndex)
    {
        loadingImage.SetActive(true);
        SceneManager.LoadScene(levelIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
