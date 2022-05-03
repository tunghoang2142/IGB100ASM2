using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public float textDelayTime = 2f;
    public TMP_Text dialog;

    static UIManager _instance;

    // Test
    bool isDisplayDialog = false;


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

    public static UIManager Instance { get { return _instance; } }

    // Start is called before the first frame update
    void Start()
    {
        // Test
        //dialog.gameObject.SetActive(true);
        //dialog.text = "a\nb";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayDialog(string text)
    {
        StartCoroutine(DialogEnumerator(text));
    }

    IEnumerator DialogEnumerator(string text)
    {
        dialog.text = text;
        dialog.gameObject.SetActive(true);
        yield return new WaitForSeconds(textDelayTime);
        dialog.gameObject.SetActive(false);
    }
}
