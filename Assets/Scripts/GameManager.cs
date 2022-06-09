using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [Header("Setting")]
    public float stressAccumulateRate = 0.5f;
    //public float madnessAccumulateRate = 10f;
    public readonly float stressReward = -30f;
    public readonly float stressPenalty = 20f;
    public readonly float stressMinimumThreshold = 0f;
    public readonly float stressMaximumThreshold = 100f;
    
    // TODO relocate this to sound manager
    [Header("Sound Setting")]
    public float maxVolumeDistance = 5f;
    public float minVolumeDistannce = 50f;
    public float maxVolume = 0.5f;
    public float minVolume = 0.1f;
    //

    [Header("Game Object")]
    public GameObject existPoint;
    public GameObject hint;

    [Header("Game State")]
    public static bool isGameover = false;
    public bool isGamePaused = false;
    public bool isNight = true;
    bool isLoadingNextScene = false;

    GameObject player;
    LineRenderer lineRenderer;
    static GameManager _instance;
    //static float money = 0f;
    static float stress = 0f;
    //static float madness = 0f;
    //static string sceneName;

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

    public static GameManager Instance { get { return _instance; } }
    //public static string SceneName  { get { return sceneName; } }
    public static float Stress { get { return stress; } }
    //public static float Madness { get { return madness; } }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lineRenderer = gameObject.GetComponent<LineRenderer>();

        if (isNight)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (isGamePaused)
            {
                Pause();
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("Enemy") && isNight && !isLoadingNextScene)
        {
            isLoadingNextScene = true;
            StartCoroutine(LoadNextScene());
        }

        if (isGameover)
        {
            return;
        }

        // TODO replace this
        hint = GameObject.FindGameObjectWithTag("Hint");
        if(stress >= stressMaximumThreshold)
        {
            Gameover();
        }
        //

        if (isNight)
        {
            IncreaseStress();
            ChangeBGMSound();
            DrawPath();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(UIManager.Instance.textDisplayTime);
        ScenarioManager.Instance.LoadNextScene();
    }

    //public void ChangeSceneName(string name)
    //{
    //    sceneName = name;
    //}

    void IncreaseStress()
    {
        if (!isGamePaused && isNight && GameObject.FindGameObjectWithTag("Enemy"))
        {
            stress += Time.deltaTime * stressAccumulateRate;
        }
    }

    public void IncreaseStress(float amount)
    {
        stress += amount;
        if(stress < stressMinimumThreshold)
        {
            stress = stressMinimumThreshold;
        }
        string stressAnnounce;
        if(amount >= 0)
        {
            stressAnnounce = "+ " + amount + "stress";
        }
        else
        {
            stressAnnounce = amount + "stress";
        }
        UIManager.Instance.DisplayAnnouncement(stressAnnounce);
    }

    //public void IncreaseMadness(float amount)
    //{
    //    madness += amount;
    //}

    void Gameover()
    {
        Time.timeScale = 0;
        isGameover = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //UIManager.Instance.Gameover();
        // TODO use config link
        ScenarioManager.Instance.LoadScene("GameoverScene");
    }

    public void Pause()
    {
        if (!isGamePaused)
        {
            Time.timeScale = 0;
            isGamePaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UIManager.Instance.Pause();

        }
        else
        {
            Time.timeScale = 1;
            isGamePaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            UIManager.Instance.Pause();
        }
    }

    public void Reset()
    {
        stress = 0f;
        isGameover = false;
        isGamePaused = false;
    }

    // TODO relocate this to soundmanager
    void ChangeBGMSound()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = minVolumeDistannce;
        foreach (var enemy in enemies)
        {
            float distance = (enemy.transform.position - player.transform.position).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }

        minDistance = Mathf.Clamp(minDistance, maxVolumeDistance, minVolumeDistannce);
        float volumnRatio = 1 - (minDistance - maxVolumeDistance) / (minVolumeDistannce - maxVolumeDistance);
        float volumn = (maxVolume - minVolume) * volumnRatio + minVolume;
        volumn = Mathf.Clamp(volumn, minVolume, maxVolume);
        SoundManager.Instance.ChangeBGMVolumn(volumn);
    }
    //

    void DrawPath()
    {
        if (!hint)
        {
            lineRenderer.enabled = false;
            return;
        }
        lineRenderer.enabled = true;
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(player.transform.position, hint.transform.position, NavMesh.AllAreas, path);
        lineRenderer.positionCount = path.corners.Length;
        
        for (int i = 0; i < path.corners.Length; i++)
        {
            lineRenderer.SetPosition(i, path.corners[i]);
        }
    }
}
