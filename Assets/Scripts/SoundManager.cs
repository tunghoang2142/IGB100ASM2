using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource backgroundSource;
    public AudioSource effectSource;
    private static SoundManager _instance;

    void Awake()
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

    public static SoundManager Instance { get { return _instance; } }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.isNight && gameObject.GetComponentInChildren<Heatbeat>() == null)
        {
            GameObject heatbeat = Instantiate(new GameObject(), this.transform);
            heatbeat.name = heatbeat.gameObject.AddComponent<Heatbeat>().GetType().Name;
        }
    }

    public void PlayEffect(string effectName, float volumn = 1f)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(Config.soundPath + effectName);
        if(audioClip != null)
        {
            AudioSource audioSource = Instantiate(new GameObject(), this.transform).AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.loop = false;
            audioSource.volume = volumn;
            audioSource.Play();
        }
    }

    public void ChangeBGMVolumn(float volumn)
    {
        backgroundSource.volume = volumn;
    }

    public void ChangeBGMClip(AudioClip clip)
    {
        backgroundSource.clip = clip;
    }

    // TODO Remove this
    public void ChangeEffectVolumn(float volumn)
    {
        effectSource.volume = volumn;
    }

    public void ChangeEffectClip(AudioClip clip)
    {
        effectSource.clip = clip;
    }

    public void PlayEffect()
    {
        effectSource.Play();
    }
    //
}
