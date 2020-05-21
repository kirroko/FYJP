using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/**
 * This Class controls all Audio related matters such as
 * 
 * Playing a Clip.
 * 
 * Pausing a Clip.
 * 
 * Stopping a Clip.
 * 
 * It will only be created once during mainmenu and is set to 
 * not destroy on load.
 */
public class AudioManager : MonoBehaviour
{
    public bool HasBGM { get { return hasBGM; } }
    public bool HasSFX { get { return hasSFX; } }

    public static AudioManager instance = null;

    //[SerializeField] AudioMixerGroup master = null;
    [SerializeField] private AudioMixerGroup mixerBGM = null;
    [SerializeField] private AudioMixerGroup mixerSFX = null;

    [SerializeField] Audio[] audios = null;

    private Dictionary<string, AudioSource> BGM = new Dictionary<string, AudioSource>();
    private Dictionary<string, AudioSource> SFX = new Dictionary<string, AudioSource>();

    private bool hasBGM = true;
    private bool hasSFX = true;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);

        //Sort all the audioClips into their respective dictionary
        foreach (Audio audio in audios)
        {
            AudioSource temp = gameObject.AddComponent<AudioSource>();
            temp.clip = audio.clip;
            
            if (audio.type == AudioType.BGM)
            {
                temp.outputAudioMixerGroup = mixerBGM;
                BGM.Add(audio.audioName, temp);
            }
            else if (audio.type == AudioType.SFX)
            {
                temp.outputAudioMixerGroup = mixerSFX;
                SFX.Add(audio.audioName, temp);
            }
        }

        int bgmVol = 1;
        int sfxVol = 1;

        // Set the volume of BGM and SFX to whatever the player has set it to at the start.
        if (!PlayerPrefs.HasKey("SFX"))
            PlayerPrefs.SetInt("SFX", 1);
        else
            sfxVol = PlayerPrefs.GetInt("SFX");

        if (!PlayerPrefs.HasKey("BGM"))
            PlayerPrefs.SetInt("BGM", 1);
        else
            bgmVol = PlayerPrefs.GetInt("BGM");
        
        hasSFX = System.Convert.ToBoolean(sfxVol);
        hasBGM = System.Convert.ToBoolean(bgmVol);

        foreach (AudioSource source in SFX.Values)
        {
            source.volume = sfxVol;
        }
        foreach (AudioSource source in BGM.Values)
        {
            source.volume = bgmVol;
        }
    }

    public static void PlayBGM(string audioName, bool loop)
    {
        instance.BGM[audioName].Play();
        instance.BGM[audioName].loop = loop;
    }

    public static void PlaySFX(string audioName, bool loop)
    {
        instance.SFX[audioName].Play();
        instance.SFX[audioName].loop = loop;
    }

    public void ToggleMuteSFX()
    {
        hasSFX = !hasSFX;

        int value = System.Convert.ToInt32(hasSFX);
        PlayerPrefs.SetInt("SFX", value);

        foreach (AudioSource source in SFX.Values)
        {
            source.volume = value;
        }
    }

    public void ToggleMuteBGM()
    {
        hasBGM = !hasBGM;

        int value = System.Convert.ToInt32(hasBGM);
        PlayerPrefs.SetInt("BGM", value);

        foreach (AudioSource source in BGM.Values)
        {
            source.volume = value;
        }
    }

    public static void StopBGM(string audioName)
    {
        instance.BGM[audioName].Stop();
    }

    public static void StopSFX(string audioName)
    {
        instance.SFX[audioName].Stop();
    }

    public void PlayClick(string audioName)
    {
        instance.SFX[audioName].Play();
    }
}