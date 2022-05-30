using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingNoteGenerator : MonoBehaviour
{
    public GameObject[] notes;
    public float spawnDelay = 0.3f;
    public float noteLifeTime = 2f;
    public float coneAngle = 60f;
    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        CalculateRotation();
    }

    // Update is called once per frame
    void Update()
    {
        while (timer <= 0)
        {
            timer += spawnDelay;
            int random = Random.Range(0, notes.Length);
            GameObject note = Instantiate(notes[random], this.transform);
            note.gameObject.transform.localScale = new Vector3(20, 20, 20);
            FlyingNote flyingNote = note.gameObject.GetComponent<FlyingNote>();
            if (flyingNote == null)
            {
                flyingNote = note.gameObject.AddComponent<FlyingNote>();
            }
            flyingNote.lifeTime = noteLifeTime;
            note.transform.forward = CalculateRotation();

        }

        timer -= Time.deltaTime;
    }

    Vector3 CalculateRotation()
    {
        float xRotation = Random.Range(- coneAngle / 2f, coneAngle / 2f);
        float yRotation = Random.Range(0f, 360f);
        Vector3 forwardVector = Quaternion.Euler(xRotation, yRotation, 0) * Vector3.forward;
        return forwardVector;
    }
}
