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
    public float escapeDistance = 3f;

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
            // TODO do something about the dialog, consider making a function for it
            UIManager.Instance.DisplayDialog("Who are you!");
            SoundManager.Instance.PlayEffect(Config.unpleasantSound);
            //

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

        // TODO do something about the dialog, consider making a function for it
        UIManager.Instance.DisplayDialog("Who are you!");
        SoundManager.Instance.PlayEffect(Config.unpleasantSound);
        //

        isPlayerDetected = true;
    }

    void Escape()
    {
        agent.SetDestination(GameManager.Instance.existPoint.transform.position);
        float distance = (GameManager.Instance.existPoint.transform.position - this.transform.position).magnitude;
        if (distance <= escapeDistance)
        {
            SoundManager.Instance.PlayEffect(Config.unpleasantSound);
            UIManager.Instance.DisplayDialog("Me: Damn! They escaped!");
            GameManager.Instance.IncreaseStress(GameManager.Instance.stressPenalty);
            Destroy(this.gameObject);
        }
    }


    public void TakeDamage(float dam)
    {
        // TODO do something about the dialog, consider making a function for it
        List<string> dialogs = new();
        dialogs.Add("Help!");
        dialogs.Add("Argggggggg!!!");
        dialogs.Add("Noooooooo!!!");
        dialogs.Add("Murder!");
        int choose = Random.Range(0, dialogs.Capacity - 1);
        UIManager.Instance.DisplayDialog(dialogs[choose]);
        //

        isPlayerDetected = true;
        health -= dam;
        if (health <= 0)
        {
            // TODO Change the dialog
            UIManager.Instance.DisplayDialog("Me: Ah! What a beautiful sound.");
            //
            SoundManager.Instance.PlayEffect(Config.pleasantPianoSFX);
            GameManager.Instance.IncreaseStress(GameManager.Instance.stressReward);
            Destroy(this.gameObject);
        }
        else
        {
            // TODO Remove magic number
            SoundManager.Instance.PlayEffect(Config.pleasantSound, 0.5f);
        }
    }
}
