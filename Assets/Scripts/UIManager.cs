using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public float textDisplayTime = 2f;
    public TMP_Text dialog;
    public GameObject gameover;
    public TMP_Text stress;
    public TMP_Text announceText;

    static UIManager _instance;
    float displayTimer = 0f;
    float announceTimer = 0f;

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
        DisplayAnnouncement();
        if (stress)
        {
            stress.text = "Stress: " + (int)GameManager.Stress;
        }
    }

    public void Gameover()
    {
        gameObject.SetActive(true);
    }

    public void DisplayDialog(string text)
    {
        dialog.text += text + "\n";
        displayTimer = textDisplayTime;
        StartCoroutine(DialogEnumerator());
    }

    public void DisplayAnnouncement(string text)
    {
        announceText.text += text + "\n";
        announceTimer = textDisplayTime;
        StartCoroutine(AnnouncementEnumerator());
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

    void DisplayAnnouncement()
    {
        announceTimer -= Time.deltaTime;
        if (announceTimer > 0)
        {
            announceText.gameObject.SetActive(true);
        }
        else
        {
            announceText.gameObject.SetActive(false);
        }
    }

    IEnumerator DialogEnumerator()
    {
        yield return new WaitForSeconds(textDisplayTime);
        dialog.text = RemoveFirstLine(dialog.text);
    }

    IEnumerator AnnouncementEnumerator()
    {
        yield return new WaitForSeconds(textDisplayTime);
        announceText.text = RemoveFirstLine(dialog.text);
    }

    string RemoveFirstLine(string str)
    {
        //print(str);
        string[] lines = Regex.Split(str, "\n");
        string result = "";
        for (int i = 0; i < lines.Length; i++)
        {
            if (i == 0)
            {
                continue;
            }
            if (lines[i] == "")
            {
                continue;
            }
            result += lines[i] + "\n";
        }
        return result;
    }
}
