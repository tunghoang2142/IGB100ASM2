using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.UI;

public class Storyteller : MonoBehaviour
{
    public Image background;
    public TMP_Text charName;
    public Image charImage;
    public TMP_Text dialog;

    string textPath;
    int linePointer = 1;
    TextAsset story;
    string[] lines;

    // Start is called before the first frame update
    void Start()
    {
        textPath = Config.textPath + ScenarioManager.Instance.GetSceneName();
        story = Resources.Load<TextAsset>(textPath);
        lines = ParseText(story.text);
        string backgroundPath = Config.imagePath + lines[0];
        background.sprite = Resources.Load<Sprite>(backgroundPath);
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
                charImage.gameObject.SetActive(false);
            }
        }
        else
        {
            charName.text = "";
            charImage.gameObject.SetActive(false);
        }

        dialog.text = line.Substring(character.Length + 1);
        dialog.text = Regex.Replace(dialog.text, "^ ", "");
    }
}
