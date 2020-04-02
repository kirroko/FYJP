using UnityEngine;

public enum AudioType
{
    BGM,
    SFX
};

[System.Serializable]
public class Audio
{
    public string audioName;
    public AudioType type;
    public AudioClip clip;
}