using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenarioManager : MonoBehaviour
{
    static ScenarioManager _instance;
    string[] sceneOrder = { "Menu", "Day 1-1", "Day 1-2", "Day 1-3", "Day 1-4", "GuideScene", "NightScene", "Day 1-5", "Day 2-1", "Day 2-2", "NightScene2", "Day X", "EndScene"};
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

    public string GetSceneName()
    {
        return sceneOrder[sceneIndex];
    }

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

    public void Resume()
    {
        GameManager.Instance.Reset();
        LoadCurrentScene();
    }

    public void Menu()
    {
        sceneIndex = 0;
        GameManager.Instance.Reset();
        LoadCurrentScene();
    }

    public void StartGame()
    {
        sceneIndex = 1;
        LoadCurrentScene();
    }

    void LoadScene()
    {
        string sceneName = sceneOrder[sceneIndex];
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
