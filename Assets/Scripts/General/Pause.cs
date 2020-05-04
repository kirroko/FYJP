using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen = null;
    [SerializeField] private Button restartBtn = null;
    [SerializeField] private Button toggleMusicBtn = null;
    [SerializeField] private Button toggleSFXBtn = null;
    [SerializeField] private Button exitBtn = null;

    private float prevTimeScale = 0f;
    private float prevFixedDelta = 0f;

    [HideInInspector] public bool isPaused = false;

    private void Start()
    {
        if (pauseScreen != null) pauseScreen.SetActive(false);

        restartBtn.onClick.AddListener(LevelManager.instance.RestartLevel);
        restartBtn.onClick.AddListener(Pausing);

        SwapSprites swapSpritesBGM = toggleMusicBtn.GetComponent<SwapSprites>();
        swapSpritesBGM.Swap(AudioManager.instance.HasBGM);
        toggleMusicBtn.onClick.AddListener(AudioManager.instance.ToggleMuteBGM);
        toggleMusicBtn.onClick.AddListener(delegate() { swapSpritesBGM.Swap(AudioManager.instance.HasBGM); });

        SwapSprites swapSpritesSFX = toggleSFXBtn.GetComponent<SwapSprites>();
        swapSpritesSFX.Swap(AudioManager.instance.HasSFX);
        toggleSFXBtn.onClick.AddListener(AudioManager.instance.ToggleMuteSFX);
        toggleSFXBtn.onClick.AddListener(delegate () { swapSpritesSFX.Swap(AudioManager.instance.HasSFX); });

        exitBtn.onClick.AddListener(delegate() { SceneTransition.instance.LoadSceneInBG("LevelSelection"); });
        exitBtn.onClick.AddListener(delegate() { LevelManager.instance.ResetLevelVariables(); });
        exitBtn.onClick.AddListener(Pausing);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && isPaused)
            Pausing();
    }

    public void Pausing()
    {
        isPaused = !isPaused;

        if(isPaused)
        {
            prevTimeScale = Time.timeScale;
            prevFixedDelta = Time.fixedDeltaTime;

            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0f;
            pauseScreen.SetActive(true);
        }
        else
        {
            Time.timeScale = prevTimeScale;
            Time.fixedDeltaTime = prevFixedDelta;
            pauseScreen.SetActive(false);
        }
    }
}
