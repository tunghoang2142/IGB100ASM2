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

    // Start is called before the first frame update
    void Start()
    {
        //GameObject.Find("Waypoints").transform.child
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseStress();
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
}
