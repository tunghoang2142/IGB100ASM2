using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heatbeat : MonoBehaviour
{
    public float stressMinThreshold = 40f;
    public float stressHighThreshold = 80f;
    public float stressMaxThreshold = GameManager.Instance.stressMaximumThreshold;
    public float minVolume = 0.5f;
    public float maxVolume = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            AudioClip audioClip = GetHeartbeatNormalAudio();
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();

        if (GameManager.Stress < stressHighThreshold && audioSource.clip.name != Config.heatbeatSFX)
        {
            audioSource.clip = GetHeartbeatNormalAudio();           
        }
        else if (GameManager.Stress >= stressHighThreshold && audioSource.clip.name != Config.heatbeatSFX2)
        {
            audioSource.clip = GetHeartbeatHighlAudio();
        }


        if (GameManager.Stress >= stressMinThreshold)
        {
            float volumnRatio = 1 - (GameManager.Stress - stressMaxThreshold) / (stressMinThreshold - stressMaxThreshold);
            float volumn = (maxVolume - minVolume) * volumnRatio + minVolume;
            volumn = Mathf.Clamp(volumn, minVolume, maxVolume);
            audioSource.volume = volumn;

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    AudioClip GetHeartbeatNormalAudio()
    {
        return Resources.Load<AudioClip>(Config.soundPath + Config.heatbeatSFX);
    }

    AudioClip GetHeartbeatHighlAudio()
    {
        return Resources.Load<AudioClip>(Config.soundPath + Config.heatbeatSFX2);
    }

}
