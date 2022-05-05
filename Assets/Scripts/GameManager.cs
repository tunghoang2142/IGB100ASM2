using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [Header("Stats")]
    public static float money = 0f;
    public static float stress = 0f;
    public static float madness = 0f;

    [Header("Setting")]
    public float stressAccumulateRate = 0.3f;
    public float madnessAccumulateRate = 10f;
    public float stressPenaltyRate = 30f;
    public GameObject existPoint;

    [Header("Sound Setting")]
    public float maxVolumnDistance = 5f;
    public float minVolumnDistannce = 50f;
    public float maxVolumn = 1f;
    public float minVolumn = 0.5f;

    [Header("Game Object")]
    public GameObject hint;

    [Header("Game State")]
    public bool isGameover = false;
    public bool isGamePaused = false;
    public bool isDay = false;

    GameObject player;
    LineRenderer lineRenderer;
    private static GameManager _instance;

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
    public static float Stress { get { return stress; } }
    public static float Madness { get { return madness; } }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameover)
        {
            Time.timeScale = 0;
            return;
        }

        // TODO replace this
        hint = GameObject.FindGameObjectWithTag("Hint");
        if(stress >= 100)
        {
            Gameover();
        }
        //

        IncreaseStress();
        ChangeBGMSound();
        DrawPath();
    }

    void IncreaseStress()
    {
        if (!isGamePaused && !isDay)
        {
            stress += Time.deltaTime * stressAccumulateRate;
        }
    }

    public void IncreaseStress(float amount)
    {
        stress += amount;
    }

    public void IncreaseMadness(float amount)
    {
        madness += amount;
    }

    void Gameover()
    {
        isGameover = true;
        UIManager.Instance.Gameover();
    }

    void ChangeBGMSound()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = minVolumnDistannce;
        foreach (var enemy in enemies)
        {
            float distance = (enemy.transform.position - player.transform.position).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }

        minDistance = Mathf.Clamp(minDistance, maxVolumnDistance, minVolumnDistannce);
        float volumnRatio = 1 - (minDistance - maxVolumnDistance) / (minVolumnDistannce - maxVolumnDistance);
        float volumn = (maxVolumn - minVolumn) * volumnRatio + minVolumn;
        volumn = Mathf.Clamp(volumn, minVolumn, maxVolumn);
        SoundManager.Instance.ChangeBGMVolumn(volumn);

    }

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
