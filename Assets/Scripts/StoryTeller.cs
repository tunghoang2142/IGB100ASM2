using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.UI;

public class StoryTeller : MonoBehaviour
{
    public TMP_Text charName;
    public Image charImage;
    public TMP_Text dialog;

    string textPath;
    int linePointer = 0;
    TextAsset story;
    string[] lines;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.SceneName == null)
        {
            GameManager.Instance.ChangeSceneName("Day 1");
        }
        textPath = Config.textPath + GameManager.SceneName;
        story = Resources.Load<TextAsset>(textPath);
        lines = ParseText(story.text);
        ParseLine(lines[linePointer]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            linePointer += 1;
            if(lines.Length <= linePointer)
            {
                ScenarioManager.Instance.LoadNextScene();
                return;
            }
            ParseLine(lines[linePointer]);
        }
    }

    string[] ParseText(string str)
    {
        return Regex.Split(str, "\n");
    }

    void ParseLine(string line)
    {
        string character = Regex.Match(line, ".*?(?=:){1}").ToString();
        dialog.text = line;
        if (character.Length > 0) {
            charName.text = character;
            Sprite sprite = Resources.Load<Sprite>(Config.imagePath + character);
            if (sprite)
            {
                charImage.gameObject.SetActive(true);
                charImage.sprite = sprite;
            }
            else
            {
                charName.text = character;
                charImage.gameObject.SetActive(false);
            }

            dialog.text = line.Substring(character.Length + 1);
            dialog.text = Regex.Replace(dialog.text, "^ ", "");
        }
    }
}
