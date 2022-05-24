using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenarioManager : MonoBehaviour
{
    static ScenarioManager _instance;
    string[] sceneOrder = { "Menu", "Day 1", "Day 2", "NightScene", "Night 2" };
    static int sceneIndex = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public static ScenarioManager Instance { get { return _instance; } }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadNextScene()
    {
        sceneIndex += 1;
        if (sceneOrder.Length > sceneIndex)
        {
            LoadScene();
        }
        else
        {
            Menu();
        }
    }

    public void LoadCurrentScene()
    {
        LoadScene();
    }

    public void Menu()
    {
        sceneIndex = 0;
        LoadCurrentScene();
    }

    void LoadScene()
    {
        string sceneName = sceneOrder[sceneIndex];
        GameManager.Instance.ChangeSceneName(sceneName);
        if (Resources.Load<TextAsset>(Config.textPath + sceneName))
        {
            LoadScene(Config.storyScene);
        }
        else
        {
            LoadScene(sceneOrder[sceneIndex]);
        }
    }
}
