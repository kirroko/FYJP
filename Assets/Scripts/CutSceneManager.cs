using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutSceneManager : MonoBehaviour
{
    // References
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private Animator ani;
    private RawImage rImage;

    private bool skip = false;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();
        ani = GetComponent<Animator>();
        rImage = GetComponent<RawImage>();
        transform.GetChild(0).gameObject.SetActive(true);

        if(PlayerPrefs.HasKey("CutScene"))
        {
            Debug.Log("Have already seen the cutScene...");
            gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("CutScene", 1);
            PlayerPrefs.Save();
            rImage.enabled = true;
            StartCoroutine(RunVideo());
        }
    }
    
    public void StopVideo()
    {
        skip = true;
    }

    private IEnumerator RunVideo()
    {
        videoPlayer.Prepare();
        while(!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing Video");
            yield return null;
        }
        Debug.Log("Video Prepared");

        rImage.texture = videoPlayer.texture;

        videoPlayer.Play();
        audioSource.Play();

        while(videoPlayer.isPlaying)
        {
            Debug.Log("Playback time: " + Mathf.FloorToInt((float)videoPlayer.time));
            if (Mathf.FloorToInt((float)videoPlayer.time) == 28)
               ani.SetTrigger("Fade");

            if(Mathf.FloorToInt((float)videoPlayer.time) > 28)
            {
                if(audioSource.volume != 0)
                {
                    audioSource.volume -= Time.deltaTime;
                }
            }

            if(skip)
            {
                videoPlayer.Stop();
                rImage.enabled = false;
                audioSource.Stop();
                break;
            }

            yield return null;
        }

        Debug.Log("Video playback finish");
        gameObject.SetActive(false);
    }
}
