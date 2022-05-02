using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health = 3f;
    public float detectionRange = 30f;
    public float detectionAngle = 30f;
    public float absoluteDetectionRadius = 3f;

    bool isPlayerDetected = false;
    GameObject player;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }

        Detect();
    }

    void Detect()
    {
        if (isPlayerDetected)
        {
            Escape();
            return;
        }

        Vector3 playerDirection = player.transform.position - this.transform.position;

        Ray ray = new(this.transform.position, playerDirection);
        RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit);

        if (!raycastHit.collider.transform.CompareTag("Player"))
        {
            return;
        }

        if (playerDirection.magnitude <= absoluteDetectionRadius)
        {
            isPlayerDetected = true;
            return;
        }

        float playerDistance = playerDirection.magnitude;
        if (playerDistance > detectionRange)
        {
            return;
        }

        float angleDifference = Vector3.Angle(this.transform.forward, playerDirection);
        if (angleDifference > detectionAngle)
        {
            return;
        }

        isPlayerDetected = true;
    }

    void Escape()
    {
        agent.SetDestination(GameManager.Instance.existPoint.transform.position);
        float distance = (GameManager.Instance.existPoint.transform.position - this.transform.position).magnitude;
        if(distance <= 1)
        {
            GameManager.Instance.IncreaseStress(GameManager.Instance.stressPenaltyRate);
            Destroy(this.gameObject);
        }
    }


    public void TakeDamage(float dam)
    {
        // Makeshift
        List<string> dialogs = new List<string>();
        dialogs.Add("Help!");
        dialogs.Add("Argggggggg!!!");
        dialogs.Add("No!!!");
        dialogs.Add("Murder!");
        int choose = Random.Range(0, dialogs.Capacity - 1);
        UIManager.Instance.DisplayDialog(dialogs[choose]);
        //

        isPlayerDetected = true;
        health -= dam;
        if (health <= 0)
        {
            GameManager.Instance.IncreaseMadness(GameManager.Instance.madnessAccumulateRate);
            Destroy(this.gameObject);
        }
    }
}
