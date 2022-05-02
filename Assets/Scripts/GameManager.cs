using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Game State")]
    public bool isGamePaused = false;
    public bool isDay = false;


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
        //GameObject.Find("Waypoints").transform.child
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseStress();
        ChangeBGMSound();
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

    void ChangeBGMSound()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = minVolumnDistannce;
        foreach (var enemy in enemies)
        {
            float distance = (enemy.transform.position - player.transform.position).magnitude;
            if(distance < minDistance)
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
}
