using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text dialog;

    static UIManager _instance;


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
        yield return new WaitForSeconds(2);
        dialog.gameObject.SetActive(false);
    }
}
