using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public float textDisplayTime = 2f;
    public TMP_Text dialog;

    static UIManager _instance;
    float displayTimer = 0f;

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
        DisplayDialog();
    }

    void DisplayDialog()
    {
        displayTimer -= Time.deltaTime;
        if (displayTimer > 0)
        {
            dialog.gameObject.SetActive(true);
        }
        else
        {
            dialog.gameObject.SetActive(false);
        }
    }

    public void DisplayDialog(string text)
    {
        print(text);
        dialog.text += text + "\n";
        displayTimer = textDisplayTime;
        StartCoroutine(DialogEnumerator());
    }

    IEnumerator DialogEnumerator()
    {
        yield return new WaitForSeconds(textDisplayTime);
        dialog.text = RemoveFirstLine(dialog.text);
    }

    string RemoveFirstLine(string str)
    {
        //print(str);
        string[] lines = Regex.Split(str, "\n");
        string result = "";
        for(int i = 0; i < lines.Length; i++)
        {
            if(i == 0)
            {
                continue;
            }
            if(lines[i] == "")
            {
                continue;
            }
            result += lines[i] + "\n";
        }
        return result;
    }
}
