using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public string[] dialogs;
    public float dialogDisplaySpeed = 1f;

    bool isTriggered = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DisplayDialog());
        }
    }

    IEnumerator DisplayDialog()
    {
        if (isTriggered)
        {
            yield break;
        }
        isTriggered = true;
        SoundManager.Instance.PlayEffect();
        foreach (var dialog in dialogs)
        {
            UIManager.Instance.DisplayDialog(dialog);
            yield return new WaitForSeconds(dialogDisplaySpeed);
        }
        Destroy(this.gameObject);
    }
}
